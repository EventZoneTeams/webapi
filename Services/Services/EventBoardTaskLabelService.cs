using AutoMapper;
using EventZone.Domain.DTOs.EventBoardDTOs.EventBoardTaskLabelDTOs;
using EventZone.Domain.Entities;
using EventZone.Repositories.Interfaces;
using EventZone.Services.Interface;

namespace EventZone.Services.Services
{
    public class EventBoardTaskLabelService : IEventBoardTaskLabelService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public EventBoardTaskLabelService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<EventBoardTaskLabelDTO> CreateLabel(EventBoardTaskLabelCreateDTO eventBoardTaskLabelModel)
        {
            var eventBoardTaskLabel = _mapper.Map<EventBoardTaskLabel>(eventBoardTaskLabelModel);
            var newLabel = await _unitOfWork.EventBoardTaskLabelRepository.AddAsync(eventBoardTaskLabel);

            await _unitOfWork.SaveChangeAsync();
            return _mapper.Map<EventBoardTaskLabelDTO>(newLabel);
        }

        public async Task<bool> DeleteLabel(Guid id)
        {
            var eventBoardTaskLabel = await _unitOfWork.EventBoardTaskLabelRepository.GetByIdAsync(id);

            if (eventBoardTaskLabel == null)
            {
                throw new Exception("Event board task label not found");
            }

            var isDeleted = await _unitOfWork.EventBoardTaskLabelRepository.SoftRemove(eventBoardTaskLabel);
            await _unitOfWork.SaveChangeAsync();
            return isDeleted;
        }

        public async Task<List<EventBoardTaskLabelDTO>> GetLabelsByEventBoardId(Guid eventBoardId)
        {
            var eventBoardTaskLabels = await _unitOfWork.EventBoardTaskLabelRepository.GetLabelsByEventBoardId(eventBoardId);
            return _mapper.Map<List<EventBoardTaskLabelDTO>>(eventBoardTaskLabels);
        }

        public async Task<EventBoardTaskLabelDTO> GetLabelById(Guid id)
        {
            var eventBoardTaskLabel = await _unitOfWork.EventBoardTaskLabelRepository.GetByIdAsync(id);

            if (eventBoardTaskLabel == null)
            {
                throw new Exception("Event board task label not found");
            }

            return _mapper.Map<EventBoardTaskLabelDTO>(eventBoardTaskLabel);
        }

        public async Task<EventBoardTaskLabelDTO> UpdateLabel(Guid id, EventBoardTaskLabelUpdateDTO eventBoardTaskLabelModel)
        {
            var eventBoardTaskLabel = await _unitOfWork.EventBoardTaskLabelRepository.GetByIdAsync(id);

            if (eventBoardTaskLabel == null)
            {
                throw new Exception("Event board task label not found");
            }

            _mapper.Map(eventBoardTaskLabelModel, eventBoardTaskLabel);

            var isUpdated = await _unitOfWork.EventBoardTaskLabelRepository.Update(eventBoardTaskLabel);
            if (!isUpdated)
            {
                throw new Exception("Failed to update event board task label");
            }

            await _unitOfWork.SaveChangeAsync();
            return _mapper.Map<EventBoardTaskLabelDTO>(eventBoardTaskLabel);
        }
    }
}
