using Core.DTO.ScheduleDto;
using Core.Entities;
using Core.IRepositories;
using Core.IServices;

namespace ICPC_Tanta_Web.Services
{
    public class ScheduleService : IScheduleService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IFileProcessingService _fileProcessingService;

        public ScheduleService(IUnitOfWork unitOfWork, IFileProcessingService fileProcessingService)
        {
            _unitOfWork = unitOfWork;
            _fileProcessingService = fileProcessingService;
        }
        public async Task AddAsync(ScheduleCreateDto createScheduleDto)
        {
            var eventExists = await _unitOfWork.EventRepository.GetByIdAsync(createScheduleDto.EventId);
            if (eventExists==null)
                throw new KeyNotFoundException($"Event with ID {createScheduleDto.EventId} not found.");
            var schedule = new Schedule
            {
                Time = createScheduleDto.Time,
                Activity = createScheduleDto.Activity,
                EventId = createScheduleDto.EventId
            };

            await _unitOfWork.ScheduleRepository.AddAsync(schedule);
            await _unitOfWork.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var item = _unitOfWork.ScheduleRepository.GetById(id);
            _unitOfWork.ScheduleRepository.Delete(item);
            await _unitOfWork.SaveChangesAsync();
        }

        public async Task<ScheduleDtoo> GetByIdAsync(int id)
        {
            var item = await _unitOfWork.ScheduleRepository.GetByIdAsync(id);
            if (item == null)
                throw new KeyNotFoundException("Schedule not found");

            return new ScheduleDtoo
            {
                Id = item.Id,
                Time = item.Time,
                Activity = item.Activity,
                EventId = item.EventId
            };
        }

        public async Task<IEnumerable<ScheduleDtoo>> GetSchedulesByEventIdAsync(int eventId)
        {
            var schedules = await _unitOfWork.ScheduleRepository.GetSchedulesByEventIdAsync(eventId);
            return schedules.Select(s => new ScheduleDtoo
            {
                Id = s.Id,
                Time = s.Time,
                Activity = s.Activity,
                EventId = s.EventId
            });
        }

        public async Task UpdateAsync(ScheduleUpdateDto updateScheduleDto)
        {
            var eventExists = await _unitOfWork.EventRepository.GetByIdAsync(updateScheduleDto.EventId);
            if (eventExists == null)
                throw new KeyNotFoundException($"Event with ID {updateScheduleDto.EventId} not found.");
            var schedule = await _unitOfWork.ScheduleRepository.GetByIdAsync(updateScheduleDto.Id);
            if (schedule == null) throw new KeyNotFoundException("Schedule not found");

            schedule.Time = updateScheduleDto.Time;
            schedule.Activity = updateScheduleDto.Activity;
            schedule.EventId = updateScheduleDto.EventId;

            _unitOfWork.ScheduleRepository.Update(schedule);
            await _unitOfWork.SaveChangesAsync();
        }
    }
}
