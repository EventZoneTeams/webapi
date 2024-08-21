using AutoMapper;
using EventZone.Domain.DTOs.EventBoardDTOs.EventBoardColumnDTOs;
using EventZone.Domain.Entities;
using EventZone.Repositories.Interfaces;
using EventZone.Services.Interface;

namespace EventZone.Services.Services
{
    public class EventBoardColumnService : IEventBoardColumnService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public EventBoardColumnService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<EventBoardColumnDTO> CreateColumn(EventBoardColumnCreateDTO eventBoardColumnModel)
        {
            var eventBoardColumn = _mapper.Map<EventBoardColumn>(eventBoardColumnModel);
            var newColumn = await _unitOfWork.EventBoardColumnRepository.AddAsync(eventBoardColumn);

            await _unitOfWork.SaveChangeAsync();
            return _mapper.Map<EventBoardColumnDTO>(newColumn);
        }

        public async Task<bool> DeleteColumn(Guid id)
        {
            var eventBoardColumn = await _unitOfWork.EventBoardColumnRepository.GetByIdAsync(id);

            if (eventBoardColumn == null)
            {
                throw new Exception("Event board column not found");
            }

            var isDeleted = await _unitOfWork.EventBoardColumnRepository.SoftRemove(eventBoardColumn);
            await _unitOfWork.SaveChangeAsync();
            return isDeleted;
        }

        public async Task<List<EventBoardColumnDTO>> GetColumnsByEventBoardId(Guid eventBoardId)
        {
            var eventBoardColumns = await _unitOfWork.EventBoardColumnRepository.GetColumnsByEventBoardId(eventBoardId);
            return _mapper.Map<List<EventBoardColumnDTO>>(eventBoardColumns);
        }

        public async Task<EventBoardColumnDTO> GetColumnById(Guid id)
        {
            var eventBoardColumn = await _unitOfWork.EventBoardColumnRepository.GetByIdAsync(id);

            if (eventBoardColumn == null)
            {
                throw new Exception("Event board column not found");
            }

            return _mapper.Map<EventBoardColumnDTO>(eventBoardColumn);
        }

        public async Task<EventBoardColumnDTO> UpdateColumn(Guid id, EventBoardColumnUpdateDTO eventBoardColumnModel)
        {
            var eventBoardColumn = await _unitOfWork.EventBoardColumnRepository.GetByIdAsync(id);

            if (eventBoardColumn == null)
            {
                throw new Exception("Event board column not found");
            }

            _mapper.Map(eventBoardColumnModel, eventBoardColumn);

            var isUpdated = await _unitOfWork.EventBoardColumnRepository.Update(eventBoardColumn);
            if (!isUpdated)
            {
                throw new Exception("Failed to update event board column");
            }

            await _unitOfWork.SaveChangeAsync();
            return _mapper.Map<EventBoardColumnDTO>(eventBoardColumn);
        }
    }
}
