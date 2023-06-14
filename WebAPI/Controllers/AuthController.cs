using AutoMapper;
using BusinessObjectsLayer.Models;
using DTOs.Request;
using DTOs.Response;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Repositories.JWT;
using Repositories;
using System.Threading.Tasks;
using Swashbuckle.AspNetCore.Annotations;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using System.Linq;
using System;

namespace WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IUserRepository userRepository;
        private readonly IMapper mapper;
        private readonly TokenGenerator tokenGenerator;
        public AuthController(IUserRepository userRepository, IMapper mapper, TokenGenerator tokenGenerator)
        {
            this.userRepository = userRepository;
            this.mapper = mapper;
            this.tokenGenerator = tokenGenerator;
        }
        private int UserID => int.Parse(FindClaim(ClaimTypes.NameIdentifier));
        private string FindClaim(string claimName)
        {

            var claimsIdentity = HttpContext.User.Identity as ClaimsIdentity;

            var claim = claimsIdentity.FindFirst(claimName);

            if (claim == null)
            {
                return null;
            }

            return claim.Value;

        }

        [AllowAnonymous]
        [SwaggerOperation(Summary = "For register")]
        [HttpPost("register")]
        public async Task<ActionResult<string>> Register([FromBody] RegisterModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            User user = mapper.Map<User>(model);
            user.Role = Role.Member;
            var check = userRepository.Register(user);
            if (check == "OK")
            {
                return Ok("Register is successfully.");
            }
            return BadRequest(check);
        }
        [AllowAnonymous]
        [SwaggerOperation(Summary = "For login")]
        [HttpPost("login")]
        public async Task<ActionResult<TokenResponse>> Login([FromBody] LoginModel loginModel)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var user = await userRepository.Login(loginModel.Email, loginModel.Password);
            var token = await System.Threading.Tasks.Task.Run(() => tokenGenerator.GenerateTokensAsync(user));
            return Ok(new TokenResponse
            {
                Message = "Login successfully",
                AccessToken = token.Item1,
                RefreshToken = token.Item2
            });

        }
        [HttpPost]
        [Authorize]
        [SwaggerOperation(Summary = "For get new access token")]
        [Route("refreshToken")]
        public async Task<IActionResult> RefreshToken([FromBody]RefreshTokenRequest refreshTokenRequest)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var validateRefreshTokenResponse = await tokenGenerator.ValidateRefreshTokenAsync(refreshTokenRequest);

            if (!validateRefreshTokenResponse.Success)
            {
                return BadRequest(validateRefreshTokenResponse);
            }
            User user = await userRepository.GetCurrentUserById(validateRefreshTokenResponse.UserId);
            var tokenResponse = tokenGenerator.CreateAccessToken(user);

            return Ok(new TokenResponse {Message = "Refresh token successfully." ,AccessToken = tokenResponse, RefreshToken = refreshTokenRequest.RefreshToken });
        }
        [Authorize]
        [HttpPost]
        [SwaggerOperation(Summary = "For logout")]
        [Route("logout")]
        public async Task<IActionResult> Logout()
        {
            var logout = await userRepository.Logout(UserID);

            if (!logout.Success)
            {
                return UnprocessableEntity(logout);
            }
            // Xóa các claim đã được đặt trong HttpContext
            var identity = HttpContext.User.Identity as ClaimsIdentity;
            if (identity != null)
            {
                identity.Claims.ToList().ForEach(c => identity.RemoveClaim(c));
            }
            var accessToken = HttpContext.Request.Headers["Authorization"].ToString().Replace("Bearer ", "");
            var time = tokenGenerator.GetAccessTokenExpiration(accessToken);
            if(time >= DateTime.Now)
            {
                AccessTokenBlacklist tokenBlacklist = new AccessTokenBlacklist
                {
                    ExpiryDate = time,
                    Token = accessToken
                };
                userRepository.AddAccessTokenToBlacklist(tokenBlacklist);
            }
            return Ok("Logout successfully.");
        }
        [Authorize(Roles = "Admin")]
        [HttpGet]
        [SwaggerOperation(Summary = "For get information of current user.")]
        [ServiceFilter(typeof(AccessTokenBlacklistFilter))]
        [Route("info")]
        public async Task<IActionResult> Info()
        {
            var userResponse = await userRepository.GetCurrentUserById(UserID);
            if (userResponse == null)
            {
                return NotFound();
            }
            CurrentUserResponse currentUser = mapper.Map<CurrentUserResponse>(userResponse);
            currentUser.Message = "Get information of current user successfully.";
            return Ok(currentUser);

        }
       
    }
}
