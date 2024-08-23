using AutoMapper;
using EventZone.Domain.DTOs.EventBoardDTOs.EventBoardTaskDTOs;
using EventZone.Repositories.Interfaces;
using EventZone.Services.Interface;
using Microsoft.Extensions.Logging;

namespace EventZone.Services.Services
{
    public class EventBoardTaskService : IEventBoardTaskService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly ILogger<EventBoardTaskService> _logger;

        public EventBoardTaskService(IUnitOfWork unitOfWork, IMapper mapper, ILogger<EventBoardTaskService> logger)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<List<EventBoardTaskResponseDTO>> GetTasksByColumnId(Guid eventBoardColumnId)
        {
            var tasks = await _unitOfWork.EventBoardTaskRepository.GetTasksByColumnId(eventBoardColumnId);
            return _mapper.Map<List<EventBoardTaskResponseDTO>>(tasks);
        }

        public async Task<EventBoardTaskResponseDTO> GetTaskById(Guid id)
        {
            var task = await _unitOfWork.EventBoardTaskRepository.GetTaskById(id);
            if (task == null)
            {
                throw new Exception("Task not found.");
            }
            return _mapper.Map<EventBoardTaskResponseDTO>(task);
        }

        public async Task<EventBoardTaskResponseDTO> CreateTask(EventBoardTaskCreateDTO eventBoardTaskCreateDTO)
        {
            try
            {
                var task = await _unitOfWork.EventBoardTaskRepository.CreateTask(eventBoardTaskCreateDTO);
                await _unitOfWork.SaveChangeAsync();
                return _mapper.Map<EventBoardTaskResponseDTO>(task);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while creating the task.");
                throw;
            }
        }

        public async Task<EventBoardTaskResponseDTO> UpdateTask(Guid taskId, EventBoardTaskUpdateDTO eventBoardTaskUpdateDTO)
        {
            try
            {
                var task = await _unitOfWork.EventBoardTaskRepository.UpdateTask(taskId, eventBoardTaskUpdateDTO);
                await _unitOfWork.SaveChangeAsync();
                return _mapper.Map<EventBoardTaskResponseDTO>(task);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while updating the task.");
                throw;
            }
        }

        public async Task DeleteTask(Guid taskId)
        {
            try
            {
                await _unitOfWork.EventBoardTaskRepository.DeleteTask(taskId);
                await _unitOfWork.SaveChangeAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while deleting the task.");
                throw;
            }
        }
    }
}
