﻿using AutoMapper;
using BusinessObjectsLayer.Models;
using DTOs.Request;
using DTOs.Response;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.DataAnnotations;
using Microsoft.OpenApi.Validations.Rules;
using Repositories;
using Repositories.JWT;
using Swashbuckle.AspNetCore.Annotations;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserRepository _userRepository;
        private readonly IMapper mapper;
        public UserController(IUserRepository userRepository, IMapper mapper)
        {
            this._userRepository = userRepository;
            this.mapper = mapper;
        }
        [HttpGet]
        [Authorize(Roles = "Admin")]
        [SwaggerOperation(Summary = "For get list of user")]
        public async Task<IActionResult> GetAllUsers()
        {
            var users = await _userRepository.GetUsersAsync();
            if (users.Count() == 0)
            {
                return NotFound("List of user null");
            }
            var usersRes = mapper.Map<List<UserReponse>>(users);
            return Ok(new ResponseObject
            {
                Message = "Get all users successfully.",
                Data = usersRes
            });
        }
        [HttpDelete("DeleteUser")]
        [Authorize(Roles = "Admin")]
        [SwaggerOperation(Summary = "For delete a user")]
        public async Task<IActionResult> DeleteUser(int id)
        {
            await _userRepository.DeleteUserAsync(id);
            return Ok(new ResponseObject
            {
                Message = "Delete successfully",
                Data = null
            });
        }
        [HttpPut("UpdateInfo")]
        [Authorize]
        [SwaggerOperation(Summary = "For update info user")]
        public async Task<IActionResult> UpdateUser([FromBody] UpdateUserModel userModel)
        {
            if(!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var user = mapper.Map<User>(userModel);
            var userUp = await _userRepository.UpdateUserAsync(user);
            var userRes = mapper.Map<UserReponse>(userUp);
            return Ok(new ResponseObject
            {
                Message = "Update user successfully",
                Data= userRes
            });
        }
        [HttpPut("UpdateRole")]
        [Authorize(Roles = "Admin")]
        [SwaggerOperation(Summary = "For update user role")]
        public async Task<IActionResult> UpdateUserRole(UpdateRoleRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            await _userRepository.UpdateRoleUser(request.UserId, request.Role);
            return Ok(new ResponseObject
            {
                Message = "Update role successfully",
                Data = null
            });
        }
        [HttpGet("GetTopTenUser")]
        [AllowAnonymous]
        [SwaggerOperation(Summary = "Get top ten user by total score")]
        public async Task<IActionResult> GetTopTenUsers()
        {
            var users = await _userRepository.GetTopTenUsers();
            List<GetRankByResponse> res = new List<GetRankByResponse>();
            foreach (var user in users)
            {
                if(user.TotalScore == null)
                {
                    GetRankByResponse rank = new GetRankByResponse
                    {
                        Email = user.Email,
                        Name = user.Name,
                        Score = 0
                    };
                    res.Add(rank);
                }
                else
                {
                    GetRankByResponse rank = new GetRankByResponse
                    {
                        Email = user.Email,
                        Name = user.Name,
                        Score = (int)user.TotalScore
                    };
                    res.Add(rank);
                }
            }
            return Ok(new ResponseObject
            {
                Message = "Get list top ten user successfully",
                Data = res
            });
        }
        [HttpGet("GetTopTenUsersByMonth")]
        [AllowAnonymous]
        [SwaggerOperation(Summary = "Get top ten user by total score")]
        public async Task<IActionResult> GetTopTenUsersByMonth()
        {
            List<GetRankByResponse> res = await _userRepository.GetTopTenUsersByMonth();
            
            if(res.Count == 0)
            {
                return BadRequest(new ResponseObject
                {
                    Message = "List null",
                    Data = null
                });
            }
            return Ok(new ResponseObject
            {
                Message = "Get the list of top 10 users by month successfully",
                Data = res
            });
        }
        [HttpGet("SearchUser/{keyword}")]
        [AllowAnonymous]
        [SwaggerOperation(Summary = "For search user by name")]
        public async Task<IActionResult> SearchUsers(string keyword)
        {
            var users = await _userRepository.SearchUsers(keyword);
            if(users.Count == 0)
            {
                return BadRequest(new ResponseObject
                {
                    Message = "List null",
                    Data = null
                });
            }
            var res = mapper.Map<List<UserReponse>>(users);
            return Ok(new ResponseObject
            {
                Message = "Search successfully",
                Data = res
            });
        }
    }
}
