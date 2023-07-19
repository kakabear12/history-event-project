using AutoMapper;
using BusinessObjectsLayer.Models;
using DTOs.Request;
using DTOs.Response;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Formatters;
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
    public class QuestionController : ControllerBase
    {
        private readonly IQuestionRepository questionRepository;
        private readonly IEventRepository eventRepository;
        private readonly IUserRepository userRepository;
        private readonly IMapper mapper;
        public QuestionController(IQuestionRepository questionRepository,
            IEventRepository eventRepository, IMapper mapper,
            IUserRepository userRepository)
        {
            this.questionRepository = questionRepository;
            this.eventRepository = eventRepository;
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
        [HttpGet("GetAllQuestions")]
        [Authorize(Roles = "Editor")]
        [SwaggerOperation(Summary = "For get list of all questions.")]
        public async Task<IActionResult> GetAllQuestions()
        {
            var questions = await questionRepository.GetAllQuestions();
            if(questions.Count() == 0)
            {
                return NotFound();
            }
            var res = mapper.Map<List<QuestionResponse>>(questions);
            foreach (var question in res)
            {
                foreach(var q in questions)
                {
                    if(question.QuestionId == q.QuestionId)
                    {
                        question.EventId = q.Event.EventId;
                    }
                }
            }
            return Ok(new ResponseObject
            {
                Message = "Get list all of the questions successfully",
                Data = res
            }) ;
        }
        [HttpGet("GetAllCompletedQuestions")]
        [Authorize(Roles = "Editor")]
        [SwaggerOperation(Summary = "For get list of all questions.")]
        public async Task<IActionResult> GetFinishedQuestions()
        {
            var ques = await questionRepository.GetAllFinishedQuestions();
            if(ques.Count() == 0)
            {
                return NotFound("List is null");
            }
            var res = mapper.Map<List<QuestionResponse>>(ques);
            foreach (var question in res)
            {
                foreach (var q in ques)
                {
                    if (question.QuestionId == q.QuestionId)
                    {
                        question.EventId = q.Event.EventId;
                    }
                }
            }
            return Ok(new ResponseObject
            {
                Message = "Get list all of the questions successfully",
                Data = res
            });
        }
        [HttpPost("CreateQuestion")]
        [Authorize(Roles = "Editor")]
        [SwaggerOperation(Summary = "For create question.")]
        public async Task<IActionResult> CreateQuestion([FromBody]CreateQuestionRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var user = await userRepository.GetCurrentUserById(UserID);
            var ev = await eventRepository.GetEventById(request.EventId);
            var question = mapper.Map<Question>(request);
            question.Event = ev;
            question.CreatedBy = user;
            var creQ = await questionRepository.CreateQuestion(question);
            var resQ = mapper.Map<QuestionResponse>(creQ);
            resQ.EventId = creQ.Event.EventId;
            return Ok(new ResponseObject
            {
                Message = "Creat question successfully",
                Data = resQ
            });
        }
        [HttpPut("UpdateQuestion")]
        [Authorize(Roles = "Editor")]
        [SwaggerOperation(Summary = "For update question.")]
        public async Task<IActionResult> UpdateQuestion([FromBody]UpdateQuestionRequest request) 
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var updateQ = mapper.Map<Question>(request);
            var quest = await questionRepository.UpdateQuestion(updateQ);
            var res = mapper.Map<QuestionResponse>(quest);
            res.EventId = quest.Event.EventId;
            return Ok(new ResponseObject
            {
                Message = "Update question successfully",
                Data = res
            });
        }
        [HttpDelete("DeleteQuestion")]
        [Authorize(Roles = "Editor")]
        [SwaggerOperation(Summary = "For delete question.")]
        public async Task<IActionResult> DeleteQuestion (int id)
        {
            if(id == null)
            {
                return BadRequest("Id is a required field");
            }
            await questionRepository.DeleteQuestion(id);
            return Ok(new ResponseObject
            {
                Message = "Delete question successfully",
                Data = null
            });
        }
        [HttpGet("GetQuestionsByEventId/{id}")]
        [Authorize(Roles = "Editor,Member")]
        [SwaggerOperation(Summary = "For get questions by eventId.")]
        public async Task<IActionResult> GetQuestionsByEventId(int id)
        {
            if(id == null)
            {
                return BadRequest("Id is a required field");
            }
            var ques = await questionRepository.GetQuestionsByEventId(id);
            if(ques.Count == 0)
            {
                return NotFound(new ResponseObject
                {
                    Data = null,
                    Message = "List null"
                });
            }
            var res = mapper.Map<List<QuestionResponse>>(ques);
            foreach (var question in res)
            {
                foreach (var q in ques)
                {
                    if (question.QuestionId == q.QuestionId)
                    {
                        question.EventId = q.Event.EventId;
                        question.CreatedBy = q.CreatedBy.Email;
                    }
                }
            }
            return Ok(new ResponseObject
            {
                Message = "Get list question by event id successfully",
                Data = res
            });
        }
        [HttpGet("GetQuestionsById/{id}")]
        [Authorize(Roles = "Editor")]
        [SwaggerOperation(Summary = "For get questions by question id.")]
        public async Task<IActionResult> GetQuestionById(int id)
        {
            var quest = await questionRepository.GetQuestionById(id);
            var res = mapper.Map<QuestionResponse>(quest);
            res.CreatedBy = quest.CreatedBy.Email;
            res.EventId = quest.Event.EventId;
            return Ok(new ResponseObject
            {
                Message = "Get question by id successfully",
                Data = res
            });
        }
    }
}
