using AutoMapper;
using BusinessObjectsLayer.Models;
using DTOs.Request;
using DTOs.Response;
using Microsoft.EntityFrameworkCore;
using Repositories.Interfaces;
using Repositories.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositories.Service
{
    public interface IEventService
    {
        Task<ResponseObject<EventResponseModel>> GetByIdAsync(int id);
        Task<ResponseObject<IEnumerable<EventResponseModel>>> GetAllAsync();
        Task<ResponseObject<EventResponseModel>> CreateAsync(EventRequestModel eventModel);
        Task<ResponseObject<EventResponseModel>> UpdateAsync(int id, EventRequestModel eventModel);
        Task<ResponseObject<bool>> DeleteAsync(int id);

        Task<ResponseObject<IEnumerable<EventResponseModel>>> GetEventsByPostId(int postId);
        Task<ResponseObject<EventResponseModel>> CreateEventForPost(int postId, EventRequestModel eventModel);
        Task<ResponseObject<EventResponseModel>> UpdateEventForPost(int postId, int eventId, EventRequestModel eventModel);
        Task<ResponseObject<bool>> DeleteEventFromPost(int postId, int eventId);
    }

    public class EventService : IEventService
    {
        private readonly EventsRepository _eventRepository;
        private readonly PostRepository _postRepository;
        private readonly IMapper _mapper;

        public EventService(EventsRepository eventRepository,PostRepository postRepository, IMapper mapper)
        {
            _eventRepository = eventRepository;
            _postRepository = postRepository;
            _mapper = mapper;
        }

        public async Task<ResponseObject<EventResponseModel>> GetByIdAsync(int id)
        {
            var eventEntity = await _eventRepository.GetById(id);
            if (eventEntity == null)
            {
                return new ResponseObject<EventResponseModel>
                {
                    Message = "Event not found",
                    Data = null
                };
            }

            var eventModel = _mapper.Map<EventResponseModel>(eventEntity);
            return new ResponseObject<EventResponseModel>
            {
                Message = "Success",
                Data = eventModel
            };
        }

        public async Task<ResponseObject<IEnumerable<EventResponseModel>>> GetAllAsync()
        {
            var events = await _eventRepository.GetAllAsync();
            var eventModels = _mapper.Map<IEnumerable<EventResponseModel>>(events);
            return new ResponseObject<IEnumerable<EventResponseModel>>
            {
                Message = "Success",
                Data = eventModels
            };
        }

        public async Task<ResponseObject<EventResponseModel>> CreateAsync(EventRequestModel eventModel)
        {
            var eventEntity = _mapper.Map<Event>(eventModel);

            // Validate startDate here (if needed)
            // Example: DateTime.TryParse(eventModel.StartDate, out DateTime startDate)

            await _eventRepository.AddAsync(eventEntity);

            var eventResponseModel = _mapper.Map<EventResponseModel>(eventEntity);
            return new ResponseObject<EventResponseModel>
            {
                Message = "Event created successfully",
                Data = eventResponseModel
            };
        }

        public async Task<ResponseObject<EventResponseModel>> UpdateAsync(int id, EventRequestModel eventModel)
        {
            var eventEntity = await _eventRepository.GetById(id);
            if (eventEntity == null)
            {
                return new ResponseObject<EventResponseModel>
                {
                    Message = "Event not found",
                    Data = null
                };
            }

            _mapper.Map(eventModel, eventEntity);
            await _eventRepository.UpdateAsync(eventEntity);

            var updatedEventModel = _mapper.Map<EventResponseModel>(eventEntity);
            return new ResponseObject<EventResponseModel>
            {
                Message = "Event updated successfully",
                Data = updatedEventModel
            };
        }

        public async Task<ResponseObject<bool>> DeleteAsync(int id)
        {
            var eventEntity = await _eventRepository.GetById(id);
            if (eventEntity == null)
            {
                return new ResponseObject<bool>
                {
                    Message = "Event not found",
                    Data = false
                };
            }

            await _eventRepository.DeleteAsync(eventEntity);
            return new ResponseObject<bool>
            {
                Message = "Event deleted successfully",
                Data = true
            };
        }

        public async Task<ResponseObject<IEnumerable<EventResponseModel>>> GetEventsByPostId(int postId)
        {
            var events = await _eventRepository.GetEventsByPostId(postId);
            var eventModels = _mapper.Map<IEnumerable<EventResponseModel>>(events);
            return new ResponseObject<IEnumerable<EventResponseModel>>
            {
                Message = "Success",
                Data = eventModels
            };
        }

        public async Task<ResponseObject<EventResponseModel>> CreateEventForPost(int postId, EventRequestModel eventModel)
        {

            // Find the existing post with the given postId
            var postEntity = await _postRepository.GetById(postId);
            if (postEntity == null)
            {
                return new ResponseObject<EventResponseModel>
                {
                    Message = "Post not found",
                    Data = null
                };
            }
            var eventEntity = _mapper.Map<Event>(eventModel);

            if (eventEntity.Posts == null)
            {
                eventEntity.Posts = new List<Post>();
            }
            // Add the post to the event's collection of posts
            eventEntity.Posts.Add(postEntity);

            await _eventRepository.AddAsync(eventEntity);

            var eventResponseModel = _mapper.Map<EventResponseModel>(eventEntity);
            return new ResponseObject<EventResponseModel>
            {
                Message = "Event created and linked to the post successfully",
                Data = eventResponseModel
            };
        }







        public async Task<ResponseObject<EventResponseModel>> UpdateEventForPost(int postId, int eventId, EventRequestModel eventModel)
        {
            var eventEntity = await _eventRepository.GetById(eventId);
            if (eventEntity == null)
            {
                return new ResponseObject<EventResponseModel>
                {
                    Message = "Event not found",
                    Data = null
                };
            }

            _mapper.Map(eventModel, eventEntity);

            // Update the post by setting the PostId property
            eventEntity.Posts = new List<Post> { new Post { PostId = postId } };

            await _eventRepository.UpdateAsync(eventEntity);

            var updatedEventModel = _mapper.Map<EventResponseModel>(eventEntity);
            return new ResponseObject<EventResponseModel>
            {
                Message = "Event updated and linked to the post successfully",
                Data = updatedEventModel
            };
        }

        public async Task<ResponseObject<bool>> DeleteEventFromPost(int postId, int eventId)
        {
            var eventEntity = await _eventRepository.GetById(eventId);
            if (eventEntity == null)
            {
                return new ResponseObject<bool>
                {
                    Message = "Event not found",
                    Data = false
                };
            }

            // Remove the link between the event and the post
            eventEntity.Posts = eventEntity.Posts.Where(p => p.PostId != postId).ToList();

            await _eventRepository.UpdateAsync(eventEntity);

            return new ResponseObject<bool>
            {
                Message = "Event unlinked from the post successfully",
                Data = true
            };
        }
    }

}
