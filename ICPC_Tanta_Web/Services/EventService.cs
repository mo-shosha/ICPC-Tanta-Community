using Core.DTO;
using Core.DTO.EventDTO;
using Core.DTO.ScheduleDto;
using Core.Entities;
using Core.IRepositories;
using Core.IServices;
using ICPC_Tanta_Web.DTO.NewsDTO;

namespace ICPC_Tanta_Web.Services
{
    public class EventService : IEventService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IFileProcessingService _fileProcessingService;

        public EventService(IUnitOfWork unitOfWork, IFileProcessingService fileProcessingService)
        {
            _unitOfWork = unitOfWork;
            _fileProcessingService = fileProcessingService;
        }
        public async Task AddAsync(EventCreateDto createEventDto)
        {
            var eventItem = new Events
            {
                Title = createEventDto.Title,
                Description = createEventDto.Description,
                Location = createEventDto.Location,
                DateTime = createEventDto.DateTime,
                TicketUrl = createEventDto.TicketUrl
            };
            _unitOfWork.EventRepository.AddAsync(eventItem);
            if (createEventDto.Image != null)
            {
                eventItem.ImgUrl = await _fileProcessingService.SaveFileAsync(createEventDto.Image);
            }

            await _unitOfWork.EventRepository.AddAsync(eventItem);
            await _unitOfWork.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var item=_unitOfWork.EventRepository.GetById(id);

            _unitOfWork.EventRepository.Delete(item);
            await _unitOfWork.SaveChangesAsync();
        }

        public async Task<IEnumerable<EventWithSchedulesDto>> GetAllAsync()
        {
            var events = await _unitOfWork.EventRepository.GetAllAsync();
            return events.Select(MapToDto);
        }

        public async Task<EventWithSchedulesDto> GetByIdAsync(int id)
        {
            var eventItem = await _unitOfWork.EventRepository.GetByIdAsync(id);
            if (eventItem == null) throw new KeyNotFoundException("Event not found");

            return MapToDto(eventItem);
        }

        public async Task UpdateAsync(EventUpdateDto updateEventDto)
        {
            var eventItem = await _unitOfWork.EventRepository.GetByIdAsync(updateEventDto.Id);
            if (eventItem == null) throw new KeyNotFoundException("Event not found");

            eventItem.Title = updateEventDto.Title;
            eventItem.Description = updateEventDto.Description;
            eventItem.Location = updateEventDto.Location;
            eventItem.DateTime = updateEventDto.DateTime;
            eventItem.TicketUrl = updateEventDto.TicketUrl;

            if (updateEventDto.Image != null)
            {
                // Delete old image if exists
                if (!string.IsNullOrEmpty(eventItem.ImgUrl))
                {
                    _fileProcessingService.DeleteFile(eventItem.ImgUrl);
                }

                // Save new image
                eventItem.ImgUrl = await _fileProcessingService.SaveFileAsync(updateEventDto.Image);
            }

            _unitOfWork.EventRepository.Update(eventItem);
            await _unitOfWork.SaveChangesAsync();
        }


        private EventWithSchedulesDto MapToDto(Events eventItem)
        {
            return new EventWithSchedulesDto
            {
                Id = eventItem.Id,
                Title = eventItem.Title,
                Description = eventItem.Description,
                Location = eventItem.Location,
                DateTime = eventItem.DateTime,
                TicketUrl = eventItem.TicketUrl,
                ImgUrl = eventItem.ImgUrl,
                DailyPlan = eventItem.DailyPlan.Select(s => new ScheduleDtoo
                {
                    Id = s.Id,
                    Time = s.Time,
                    Activity = s.Activity,
                    EventId = s.EventId
                }).ToList()
            };
        }
    }
}
