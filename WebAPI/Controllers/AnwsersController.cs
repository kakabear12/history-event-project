using AutoMapper;
using BusinessObjectsLayer.Models;
using DTOs.Request;
using DTOs.Response;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Repositories.Interfaces;
using Swashbuckle.AspNetCore.Annotations;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AnwsersController : ControllerBase
    {
        private readonly IQuestionRepository questionRepository;
        private readonly IAnswerRepository answerRepository;
        private readonly IMapper mapper;
        public AnwsersController(IQuestionRepository questionRepository, IAnswerRepository answerRepository, IMapper mapper)
        {
            this.questionRepository = questionRepository;
            this.answerRepository = answerRepository;
            this.mapper = mapper;
        }
        [HttpGet("GetAllAnswersByQuestionId")]
        [Authorize(Roles = "Editor")]
        [SwaggerOperation(Summary = "For get list of answers by question id")]
        public async Task<IActionResult> GetAllAnswersByQuestionId(int id)
        {
            var ans = await answerRepository.GetAnswersByQuestionId(id);
            if(ans.Count == 0)
            {
                return NotFound();
            }
            var res = mapper.Map<List<AnswerResponse>>(ans);
            foreach(var a in ans)
            {
                foreach(var r in res)
                {
                    r.QuestionId = a.Question.QuestionId;
                }
            }
            return Ok(new ResponseObject
            {
                Message = "Get list answers successfully",
                Data = res
            });
        }
        [HttpPost("CreateAnswer")]
        [Authorize(Roles = "Editor")]
        [SwaggerOperation(Summary = "For create answer")]
        public async Task<IActionResult> CreateAnswer([FromBody] CreateAnswerRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var quest = await questionRepository.GetQuestionById(request.QuestionId);

            var ans = mapper.Map<Answer>(request);
            ans.Question = quest;
            var createA = await answerRepository.CreateAnswer(ans);
            var res = mapper.Map<AnswerResponse>(createA);
            res.QuestionId = createA.Question.QuestionId;
            return Ok(new ResponseObject
            {
                Message = "Create answer successfully",
                Data = res
            });
        }
        [HttpPut("UpdateAnswer")]
        [Authorize(Roles = "Editor")]
        [SwaggerOperation(Summary = "For update answer")]
        public async Task<IActionResult> UpdateAnswer([FromBody]UpdateAnswerRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var quest = await questionRepository.GetQuestionById(request.QuestionId);
            var ans = mapper.Map<Answer>(request);
            ans.Question = quest;
            var updateA = await answerRepository.UpdateAnswer(ans);
            var res = mapper.Map<AnswerResponse>(updateA);
            res.QuestionId = updateA.Question.QuestionId;
            return Ok(new ResponseObject
            {
                Message = "Update answer successfully",
                Data = res
            });
        }
        [HttpDelete("DeleteAnswer")]
        [Authorize(Roles = "Editor")]
        [SwaggerOperation(Summary = "For delete answer")]
        public async Task<IActionResult> DeleteAnswer(int id)
        {
            if(id == null)
            {
                return BadRequest("Id is a required field");
            }
            await answerRepository.DeleteAnswer(id);
            return Ok(new ResponseObject
            {
                Message = "Delete answer successfully",
                Data = null
            });
        }
    }
}
