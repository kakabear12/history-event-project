using BusinessObjectsLayer.Models;
using DataAccessLayer;
using DTOs.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly UserDAO userDAO;
        private readonly RefreshTokenDAO refreshTokenDAO;
        private readonly AccessTokenBlacklistDAO tokenBlacklistDAO;
        public UserRepository(UserDAO userDAO, RefreshTokenDAO refreshTokenDAO, AccessTokenBlacklistDAO tokenBlacklistDAO)
        {
            this.userDAO = userDAO;
            this.refreshTokenDAO = refreshTokenDAO;
            this.tokenBlacklistDAO = tokenBlacklistDAO; 
        }

        public async Task<User> GetCurrentUserById(int id)
        {
           return userDAO.GetUserByUserId(id);
        }

        public async Task<User> Login(string email, string password)
        {
            return userDAO.Login(email, password);
        }

        public string Register(User user)
        {
            return userDAO.Register(user);
        }
        public async Task<LogoutResponse> Logout(int userId)
        {
            var refreshToken = await refreshTokenDAO.GetRefreshTokenByUserIdAsync(userId);

            if (refreshToken == null)
            {
                return new LogoutResponse { Success = true };
            }

            var saveResponse = await refreshTokenDAO.RemoveRefreshToken(refreshToken);

            if (saveResponse >= 0)
            {
                return new LogoutResponse { Success = true }; ;
            }

            return new LogoutResponse { Success = false, Error = "Unable to logout user", ErrorCode = "L04" };
        }

        public async Task<IEnumerable<AccessTokenBlacklist>> GetAccessTokenBlacklists()
        {
            return await tokenBlacklistDAO.GetBlacklistsAsync();
        }

        public void AddAccessTokenToBlacklist(AccessTokenBlacklist accessToken)
        {
            tokenBlacklistDAO.AddBlacklist(accessToken);
        }

    }
}
