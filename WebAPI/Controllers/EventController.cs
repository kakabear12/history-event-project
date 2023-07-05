using AutoMapper;
using DTOs.Request;
using DTOs.Response;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Repositories.Interfaces;
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
        private readonly IEventRepository eventRepository;
        private readonly IMapper mapper;
        public EventController(IEventService eventService, IEventRepository eventRepository, IMapper mapper)
        {
            _eventService = eventService;
            this.eventRepository = eventRepository;
            this.mapper = mapper;
        }

        [HttpGet("{id}")]
       
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


        [HttpGet("{postId}/events")]
       
        [SwaggerOperation(Summary = "For get events of a specific post")]
        public async Task<ActionResult<ResponseObject<IEnumerable<EventResponseModel>>>> GetEventsByPostId(int postId)
        {
            var response = await _eventService.GetEventsByPostId(postId);
            return Ok(response);
        }

        [HttpPost("{postId}/events")]
        [Authorize(Roles = "Editor")]
        [SwaggerOperation(Summary = "For create an event for a specific post")]
        public async Task<ActionResult<ResponseObject<EventResponseModel>>> CreateEventForPost(int postId, EventRequestModel eventModel)
        {
            var response = await _eventService.CreateEventForPost(postId, eventModel);
            return Ok(response);
        }

        [HttpPut("{postId}/events/{eventId}")]
        [Authorize(Roles = "Editor")]
        [SwaggerOperation(Summary = "For update an event of a specific post")]
        public async Task<ActionResult<ResponseObject<EventResponseModel>>> UpdateEventForPost(int postId, int eventId, EventRequestModel eventModel)
        {
            var response = await _eventService.UpdateEventForPost(postId, eventId, eventModel);
            if (response.Data == null)
            {
                return NotFound(response);
            }

            return Ok(response);
        }

        [HttpDelete("{postId}/events/{eventId}")]
        [Authorize(Roles = "Editor")]
        [SwaggerOperation(Summary = "For delete an event from a specific post")]
        public async Task<ActionResult<ResponseObject<bool>>> DeleteEventFromPost(int postId, int eventId)
        {
            var response = await _eventService.DeleteEventFromPost(postId, eventId);
            if (!response.Data)
            {
                return NotFound(response);
            }

            return Ok(response);
        }
        [HttpGet("searchEvents/{keyword}")]
        [AllowAnonymous]
        [SwaggerOperation(Summary = "For search events by name")]
        public async Task<IActionResult> SearchEvents(string keyword)
        {
            var events = await eventRepository.SearchEventsByName(keyword);
            if (events.Count == 0)
            {
                return BadRequest(new ResponseObject
                {
                    Message = "List null",
                    Data = null
                });
            }
            var res = mapper.Map<List<EventResponseModel>>(events);
            return Ok(new ResponseObject
            {
                Message = "Search successfully",
                Data = res
            });
        }
    }
}
