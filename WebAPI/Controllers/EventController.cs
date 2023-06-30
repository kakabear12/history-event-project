﻿using DTOs.Request;
using DTOs.Response;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Repositories.Service;
using Swashbuckle.AspNetCore.Annotations;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace WebAPI.Controllers
{
    [Route("api/events")]
    [ApiController]
    public class EventController : ControllerBase
    {
        private readonly IEventService _eventService;

        public EventController(IEventService eventService)
        {
            _eventService = eventService;
        }

        [HttpGet("{id}")]
        [Authorize(Roles = "Editor")]
        [SwaggerOperation(Summary = "For get envent by id")]
        public async Task<ActionResult<ResponseObject<EventResponseModel>>> GetEventById(int id)
        {
            var response = await _eventService.GetByIdAsync(id);
            if (response.Data == null)
            {
                return NotFound(response);
            }

            return Ok(response);
        }

        [HttpGet]
       
        [SwaggerOperation(Summary = "For get list of events")]
        public async Task<ActionResult<ResponseObject<IEnumerable<EventResponseModel>>>> GetAllEvents()
        {
            var response = await _eventService.GetAllAsync();
            return Ok(response);
        }

        [HttpPost]
        [Authorize(Roles = "Editor")]
        [SwaggerOperation(Summary = "For create event")]
        public async Task<ActionResult<ResponseObject<int>>> CreateEvent(EventRequestModel eventModel)
        {
            var response = await _eventService.CreateAsync(eventModel);
            return  Ok(response);
        }

        [HttpPut("{id}")]
        [Authorize(Roles = "Editor")]
        [SwaggerOperation(Summary = "For update envent by id")]
        public async Task<ActionResult<ResponseObject<EventResponseModel>>> UpdateEvent(int id, EventRequestModel eventModel)
        {
            var response = await _eventService.UpdateAsync(id, eventModel);
            if (response.Data == null)
            {
                return NotFound(response);
            }

            return Ok(response);
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "Editor")]
        [SwaggerOperation(Summary = "For delete event by id")]
        public async Task<ActionResult<ResponseObject<bool>>> DeleteEvent(int id)
        {
            var response = await _eventService.DeleteAsync(id);
            if (!response.Data)
            {
                return NotFound(response);
            }

            return Ok(response);
        }
    }
}