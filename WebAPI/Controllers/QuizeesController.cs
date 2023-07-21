using AutoMapper;
using BusinessObjectsLayer.Models;
using DTOs.Request;
using DTOs.Response;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Repositories;
using Repositories.Interfaces;
using Swashbuckle.AspNetCore.Annotations;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class QuizeesController : ControllerBase
    {
        private readonly IQuizRepository quizRepository;
        private readonly IUserRepository userRepository;
     
        private readonly IMapper mapper;
        public QuizeesController(IQuizRepository quizRepository, IMapper mapper, IUserRepository userRepository)
        {
            this.quizRepository = quizRepository;
            this.mapper = mapper;
            this.userRepository = userRepository;
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
        [HttpPost("CreateQuiz")]
        [Authorize(Roles = "Member")]
        [SwaggerOperation(Summary = "For create quiz for user")]
        public async Task<IActionResult> CreateQuiz(int eventId, [FromBody] CreateQuizRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);  
            }
            var quiz = mapper.Map<Quiz>(request);
            var user = await userRepository.GetCurrentUserById(UserID);
            quiz.User = user;
            var createQuiz = await quizRepository.CreateQuiz(eventId, quiz);
            var res = mapper.Map<QuizResponse>(createQuiz);
            return Ok(new ResponseObject
            {
                Message = "Create quiz successfully",
                Data = res
            });
        }
        [HttpGet("GetQuiz")]
        [Authorize(Roles = "Member")]
        [SwaggerOperation(Summary = "For get quiz for member to do")]
        public async Task<IActionResult> GetQuiz(int quizId)
        {
            if(quizId == null)
            {
                return BadRequest("Quiz id is a required field");
            }
            var quiz = await quizRepository.GetQuizToDo(quizId);
            var resquiz = mapper.Map<GetQuizResponse>(quiz);
            return Ok(resquiz);
        }
        [HttpPost("GetResultQuiz")]
        [Authorize(Roles = "Member")]
        [SwaggerOperation(Summary = "For get result of quiz")]
        public async Task<IActionResult> GetQuizResult([FromBody]ResultQuizRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            if(request.Quizzes.Count() == 0)
            {
                return BadRequest("The quiz failed");
            }
            foreach(var quiz in request.Quizzes)
            {
                await quizRepository.GetResultQuiz(request.QuizId, quiz.QuestionId, quiz.AnswerId);
            }
            
            var quizInfo = await quizRepository.GetQuizById(request.QuizId);


            var res = mapper.Map<QuizResultResponse>(quizInfo);
            return Ok(new ResponseObject
            {
                Message = "Get result of quiz successfully",
                Data = res
            });
        }
        [HttpGet("GetQuizzessByUserId")]
        [Authorize(Roles = "Member")]
        [SwaggerOperation(Summary = "For get list quiz by user id")]
        public async Task<IActionResult> GetQuizzessByUserId()
        {
            var quizzess = await quizRepository.GetAllQuizsByUserId(UserID);
            if(quizzess.Count() == 0)
            {
                return BadRequest(new ResponseObject
                {
                    Message = "List null",
                    Data = null
                });
            }
            var res = mapper.Map<List<QuizResultResponse>>(quizzess);
            return Ok(new ResponseObject
            {
                Message = "Get all quiz by user id successfully",
                Data = res
            });
        }

    }
}
