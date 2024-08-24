using AutoMapper;
using EventZone.Domain.DTOs.EventBoardDTOs.EventBoardLabelDTOs;
using EventZone.Domain.Entities;
using EventZone.Repositories.Interfaces;
using EventZone.Services.Interface;

namespace EventZone.Services.Services
{
    public class EventBoardLabelService : IEventBoardLabelService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public EventBoardLabelService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<EventBoardLabelDTO> CreateLabel(EventBoardLabelCreateDTO eventBoardLabelModel)
        {
            var eventBoardLabel = _mapper.Map<EventBoardLabel>(eventBoardLabelModel);
            var newLabel = await _unitOfWork.EventBoardLabelRepository.AddAsync(eventBoardLabel);

            await _unitOfWork.SaveChangeAsync();
            return _mapper.Map<EventBoardLabelDTO>(newLabel);
        }

        public async Task<bool> DeleteLabel(Guid id)
        {
            var eventBoardLabel = await _unitOfWork.EventBoardLabelRepository.GetByIdAsync(id);

            if (eventBoardLabel == null)
            {
                throw new Exception("Event board label not found");
            }

            var isDeleted = await _unitOfWork.EventBoardLabelRepository.SoftRemove(eventBoardLabel);
            await _unitOfWork.SaveChangeAsync();
            return isDeleted;
        }

        public async Task<List<EventBoardLabelDTO>> GetLabelsByEventId(Guid eventId)
        {
            var eventBoardLabels = await _unitOfWork.EventBoardLabelRepository.GetLabelsByEventId(eventId);
            return _mapper.Map<List<EventBoardLabelDTO>>(eventBoardLabels);
        }

        public async Task<EventBoardLabelDTO> GetLabelById(Guid id)
        {
            var eventBoardLabel = await _unitOfWork.EventBoardLabelRepository.GetByIdAsync(id);

            if (eventBoardLabel == null)
            {
                throw new Exception("Event board label not found");
            }

            return _mapper.Map<EventBoardLabelDTO>(eventBoardLabel);
        }

        public async Task<EventBoardLabelDTO> UpdateLabel(Guid id, EventBoardLabelUpdateDTO eventBoardLabelUpdateDTO)
        {
            var eventBoardLabel = await _unitOfWork.EventBoardLabelRepository.GetByIdAsync(id);

            if (eventBoardLabel == null)
            {
                throw new Exception("Event board label not found");
            }

            _mapper.Map(eventBoardLabelUpdateDTO, eventBoardLabel);

            var isUpdated = await _unitOfWork.EventBoardLabelRepository.Update(eventBoardLabel);
            if (!isUpdated)
            {
                throw new Exception("Failed to update event board label");
            }

            await _unitOfWork.SaveChangeAsync();
            return _mapper.Map<EventBoardLabelDTO>(eventBoardLabel);
        }
    }
}
