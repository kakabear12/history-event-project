﻿using BusinessObjectsLayer.Models;
using DTOs.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositories
{
    public interface IUserRepository
    {
        string Register(User user);
        Task<User> Login(string email, string password);
        Task<User> GetCurrentUserById(int id);
        Task<LogoutResponse> Logout(int id);
        Task<IEnumerable<AccessTokenBlacklist>> GetAccessTokenBlacklists();
        void AddAccessTokenToBlacklist(AccessTokenBlacklist accessToken);
    }
}
