using EventZone.Domain.Entities;
using EventZone.Repositories.Interfaces;
using EventZone.Services.Interface;

namespace EventZone.Services.Services
{
    public class EventStaffService : IEventStaffService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IClaimsService _claimsService;
        public EventStaffService(IUnitOfWork unitOfWork, IClaimsService claimsService)
        {
            _unitOfWork = unitOfWork;
            _claimsService = claimsService;
        }
        public async Task<List<Event>> GetEventByCurrentStaff()
        {
            if (_claimsService.GetCurrentUserId == null)
            {
                throw new Exception("User not found");
            }
            var eventStaffs = await _unitOfWork.EventStaffRepository.GetAllAsync(x => x.Event);
            var events = eventStaffs.Where(x => x.UserId == _claimsService.GetCurrentUserId).Select(x => x.Event).ToList();
            return events;
        }

        public async Task<List<EventStaff>> GetEventStaffAsync(Guid eventId)
        {
            var eventStaffs = await _unitOfWork.EventStaffRepository.GetAllAsync(x => x.User);
            return eventStaffs.Where(x => x.EventId == eventId).Where(x => !x.IsDeleted).ToList();
        }

        public async Task<EventStaff> AddStaffIntoEvent(Guid eventId, Guid userId, string note)
        {
            //check it exists
            var eventStaffs = await _unitOfWork.EventStaffRepository.GetAllAsync();
            var eventStaff = eventStaffs.FirstOrDefault(x => x.EventId == eventId && x.UserId == userId);
            //check event
            var events = await _unitOfWork.EventRepository.GetByIdAsync(eventId);
            if (events == null)
            {
                throw new Exception("Event not found");
            }
            if (eventStaff != null)
            {
                throw new Exception("Staff already exists");
            }
            var result = await _unitOfWork.EventStaffRepository.AddAsync(new EventStaff
            {
                EventId = eventId,
                UserId = userId,
                Note = note
            });
            await _unitOfWork.SaveChangeAsync();
            return result;
        }

        public async Task<EventStaff> RemoveStaffFromEvent(Guid eventId, Guid userId)
        {
            //Find
            var eventStaffs = await _unitOfWork.EventStaffRepository.GetAllAsync();
            var staff = eventStaffs.FirstOrDefault(x => x.EventId == eventId && x.UserId == userId);
            if (staff == null)
            {
                throw new Exception("Staff not found");
            }
            //remove
            await _unitOfWork.EventStaffRepository.SoftRemove(staff);
            await _unitOfWork.SaveChangeAsync();

            return staff;
        }
    }
}
