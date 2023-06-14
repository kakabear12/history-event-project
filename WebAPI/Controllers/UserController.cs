using AutoMapper;
using BusinessObjectsLayer.Models;
using DTOs.Request;
using DTOs.Response;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.OpenApi.Validations.Rules;
using Repositories;
using Repositories.JWT;
using Swashbuckle.AspNetCore.Annotations;
using System;
using System.ComponentModel;
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

    }
}
