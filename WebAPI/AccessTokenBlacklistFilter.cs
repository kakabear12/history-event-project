using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc;
using Repositories;
using System.Linq;

namespace WebAPI
{
    public class AccessTokenBlacklistFilter : IActionFilter
    {
        private readonly IUserRepository _userRepository;

        public AccessTokenBlacklistFilter(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public void OnActionExecuting(ActionExecutingContext context)
        {
            // Lấy Access Token từ Header Authorization
            var accessToken = context.HttpContext.Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();

            if (!string.IsNullOrEmpty(accessToken))
            {
                // Kiểm tra Access Token trong blacklist
                var blacklists = _userRepository.GetAccessTokenBlacklists().GetAwaiter().GetResult();
                if (blacklists.Any(b => b.Token == accessToken))
                {
                    context.HttpContext.Response.StatusCode = 401;
                    context.Result = new JsonResult("Access token is blacklisted.");
                }
            }
        }

        public void OnActionExecuted(ActionExecutedContext context)
        {
            // Không cần thực hiện bất kỳ hành động nào sau khi action được thực thi
        }
    }
}
