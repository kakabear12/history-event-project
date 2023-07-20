using BusinessObjectsLayer.Models;
using DTOs.Exceptions;
using DTOs.Helpers;
using DTOs.Request;
using DTOs.Response;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Repositories.JWT
{
    public class TokenGenerator
    {
        private readonly IConfiguration _configuration;
        private readonly HistoryEventDBContext context;
        public TokenGenerator(IConfiguration configuration, HistoryEventDBContext historyEventDBContext)
        {
            this._configuration = configuration;
            context = historyEventDBContext; 
        }
        public string CreateAccessToken(User user)
        {
            try
            {
                List<Claim> claims = new List<Claim>
                {
                    new Claim(ClaimTypes.Email,  user.Email),
                    new Claim(ClaimTypes.NameIdentifier, user.UserId.ToString()),
                    new Claim(ClaimTypes.Role, user.Role.ToString())
                };
                var key = new SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes(
                    _configuration.GetSection("JWT:Key").Value));
                var cred = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);
                var token = new JwtSecurityToken(_configuration["JWT:Issuer"],
                    _configuration["JWT:Audience"],
                    claims: claims,
                    expires: DateTime.Now.AddHours(1),
                    signingCredentials: cred);
                var jwt = new JwtSecurityTokenHandler().WriteToken(token);
                return jwt;
            }
            catch (Exception ex)
            {
                throw new CustomException(ex.Message);
            }
        }
        public static async Task<string> GenerateRefreshToken()
        {
            var secureRandomBytes = new byte[32];

            using var randomNumberGenerator = RandomNumberGenerator.Create();
            await System.Threading.Tasks.Task.Run(() => randomNumberGenerator.GetBytes(secureRandomBytes));

            var refreshToken = Convert.ToBase64String(secureRandomBytes);
            return refreshToken;
        }
        public async Task<Tuple<string, string>> GenerateTokensAsync(User user)
        {
            var accessToken = CreateAccessToken(user);
            var refreshToken = await GenerateRefreshToken();

            var userRecord = await context.Users.Include(o => o.RefreshTokens).FirstOrDefaultAsync(e => e.UserId == user.UserId);

            if (userRecord == null)
            {
                return null;
            }

            var salt = PasswordHelper.GetSecureSalt();

            var refreshTokenHashed = PasswordHelper.HashUsingPbkdf2(refreshToken, salt);

            if (userRecord.RefreshTokens != null && userRecord.RefreshTokens.Any())
            {
                await RemoveRefreshTokenAsync(userRecord);

            }
            userRecord.RefreshTokens?.Add(new RefreshToken
            {
                ExpiryDate = DateTime.Now.AddDays(14),
                Ts = DateTime.Now,
                UserId = user.UserId,
                TokenHash = refreshTokenHashed,
                TokenSalt = Convert.ToBase64String(salt)

            });

            await context.SaveChangesAsync();

            var token = new Tuple<string, string>(accessToken, refreshToken);

            return token;
        }
        public async Task<bool> RemoveRefreshTokenAsync(User user)
        {
            var userRecord = await context.Users.Include(o => o.RefreshTokens).FirstOrDefaultAsync(e => e.UserId == user.UserId);

            if (userRecord == null)
            {
                return false;
            }

            if (userRecord.RefreshTokens != null && userRecord.RefreshTokens.Any())
            {
                var currentRefreshToken = userRecord.RefreshTokens.First();

                context.RefreshTokens.Remove(currentRefreshToken);
            }

            return false;
        }


        public async Task<ValidateRefreshTokenResponse> ValidateRefreshTokenAsync(RefreshTokenRequest refreshTokenRequest)
        {
            var refreshToken = await context.RefreshTokens.FirstOrDefaultAsync(o => o.UserId == refreshTokenRequest.UserId);

            var response = new ValidateRefreshTokenResponse();
            if (refreshToken == null)
            {
                response.Success = false;
                response.Error = "Invalid session or user is already logged out";
                response.ErrorCode = "invalid_grant";
                return response;
            }

            var refreshTokenToValidateHash = PasswordHelper.HashUsingPbkdf2(refreshTokenRequest.RefreshToken, Convert.FromBase64String(refreshToken.TokenSalt));

            if (refreshToken.TokenHash != refreshTokenToValidateHash)
            {
                response.Success = false;
                response.Error = "Invalid refresh token";
                response.ErrorCode = "invalid_grant";
                return response;
            }

            if (refreshToken.ExpiryDate < DateTime.Now)
            {
                response.Success = false;
                response.Error = "Refresh token has expired";
                response.ErrorCode = "invalid_grant";
                return response;
            }

            response.Success = true;
            response.UserId = refreshToken.UserId;

            return response;
        }
        public DateTime GetAccessTokenExpiration(string accessToken)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var jwtToken = tokenHandler.ReadToken(accessToken) as JwtSecurityToken;

            if (jwtToken == null)
            {
                throw new ArgumentException("Invalid access token");
            }

            // Lấy giá trị Exp (Expiration Time) từ mã JWT
            var expValue = jwtToken.Payload.Exp;

            // Kiểm tra xem giá trị Exp có hợp lệ hay không
            if (expValue == null || !long.TryParse(expValue.ToString(), out long exp))
            {
                throw new ArgumentException("Invalid expiration time");
            }

            // Chuyển đổi từ giá trị Unix timestamp sang đối tượng DateTime
            var expirationDateTime = DateTimeOffset.FromUnixTimeSeconds(exp).DateTime;

            // Lấy múi giờ của Việt Nam từ các thông tin múi giờ chuẩn sẵn có trong .NET
            var vietnamTimeZone = TimeZoneInfo.FindSystemTimeZoneById("SE Asia Standard Time");

            // Chuyển đổi thời gian sang múi giờ Việt Nam
            var vietnamExpirationDateTime = TimeZoneInfo.ConvertTimeFromUtc(expirationDateTime, vietnamTimeZone);

            return vietnamExpirationDateTime;
        }





    }
}
