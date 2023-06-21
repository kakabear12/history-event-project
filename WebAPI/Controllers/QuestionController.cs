﻿using AutoMapper;
using BusinessObjectsLayer.Models;
using DTOs.Request;
using DTOs.Response;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Repositories.Interfaces;
using Swashbuckle.AspNetCore.Annotations;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class QuestionController : ControllerBase
    {
        private readonly IQuestionRepository questionRepository;
        private readonly IEventRepository eventRepository;
        private readonly IMapper mapper;
        public QuestionController(IQuestionRepository questionRepository,
            IEventRepository eventRepository, IMapper mapper)
        {
            this.questionRepository = questionRepository;
            this.eventRepository = eventRepository;
            this.mapper = mapper;
        }

        [HttpGet("getAllQuestions")]
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
        [HttpGet("getAllCompletedQuestions")]
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
        [HttpPost("createQuestion")]
        [Authorize(Roles = "Editor")]
        [SwaggerOperation(Summary = "For create question.")]
        public async Task<IActionResult> CreateQuestion([FromBody]CreateQuestionRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var ev = await eventRepository.GetEventById(request.EventId);
            var question = mapper.Map<Question>(request);
            question.Event = ev;
            var creQ = await questionRepository.CreateQuestion(question);
            var resQ = mapper.Map<QuestionResponse>(creQ);
            resQ.EventId = creQ.Event.EventId;
            return Ok(new ResponseObject
            {
                Message = "Creat question successfully",
                Data = resQ
            });
        }
        [HttpPut("updateQuestion")]
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
        [HttpDelete("deleteQuestion")]
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
    }
}