using AutoMapper;
using EventZone.Domain.DTOs.EventBoardDTOs;
using EventZone.Domain.Entities;
using EventZone.Repositories.Interfaces;
using EventZone.Services.Interface;

namespace EventZone.Services.Services
{
    public class EventBoardService : IEventBoardService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public EventBoardService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<EventBoardResponseDTO> CreateBoard(EventBoardCreateDTO eventBoardModel)
        {
            var eventBoard = _mapper.Map<EventBoard>(eventBoardModel);
            var newBoard = await _unitOfWork.EventBoardRepository.AddAsync(eventBoard);

            await _unitOfWork.SaveChangeAsync();
            return _mapper.Map<EventBoardResponseDTO>(newBoard);
        }

        public async Task<bool> DeleteBoard(Guid id)
        {
            var eventBoard = await _unitOfWork.EventBoardRepository.GetByIdAsync(id);

            if (eventBoard == null)
            {
                throw new Exception("Event board not found");
            }

            var isDeleted = await _unitOfWork.EventBoardRepository.SoftRemove(eventBoard);
            await _unitOfWork.SaveChangeAsync();
            return isDeleted;
        }

        public async Task<List<EventBoardResponseDTO>> GetBoardsByEventId(Guid eventId)
        {
            var eventBoards = await _unitOfWork.EventBoardRepository.GetBoardsByEventId(eventId);
            return _mapper.Map<List<EventBoardResponseDTO>>(eventBoards);
        }

        public async Task<EventBoardResponseDTO> GetBoardById(Guid id)
        {
            var eventBoard = await _unitOfWork.EventBoardRepository.GetByIdAsync(id);

            if (eventBoard == null)
            {
                throw new Exception("Event board not found");
            }

            return _mapper.Map<EventBoardResponseDTO>(eventBoard);
        }

        public async Task<EventBoardResponseDTO> UpdateBoard(Guid id, EventBoardUpdateDTO eventBoardModel)
        {
            var eventBoard = await _unitOfWork.EventBoardRepository.GetByIdAsync(id);

            if (eventBoard == null)
            {
                throw new Exception("Event board not found");
            }

            _mapper.Map(eventBoardModel, eventBoard);

            var isUpdated = await _unitOfWork.EventBoardRepository.Update(eventBoard);
            if (!isUpdated)
            {
                throw new Exception("Failed to update event board");
            }

            await _unitOfWork.SaveChangeAsync();
            return _mapper.Map<EventBoardResponseDTO>(eventBoard);
        }
    }
}
