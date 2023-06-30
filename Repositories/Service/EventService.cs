using AutoMapper;
using BusinessObjectsLayer.Models;
using DTOs.Request;
using DTOs.Response;
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
    }

    public class EventService : IEventService
    {
        private readonly EventsRepository _eventRepository;
        private readonly IMapper _mapper;

        public EventService(EventsRepository eventRepository, IMapper mapper)
        {
            _eventRepository = eventRepository;
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
    }

}
