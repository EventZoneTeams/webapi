using EventZone.Domain.DTOs.EventBoardDTOs.EventBoardTaskDTOs;
using EventZone.Domain.Entities;
using EventZone.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace EventZone.Repositories.Repositories
{
    public class EventBoardTaskRepository : GenericRepository<EventBoardTask>, IEventBoardTaskRepository
    {
        private readonly StudentEventForumDbContext _context;
        private readonly ICurrentTime _timeService;
        private readonly IClaimsService _claimsService;
        private readonly ILogger<EventBoardTaskRepository> _logger;

        public EventBoardTaskRepository(StudentEventForumDbContext context, ICurrentTime timeService, IClaimsService claimsService, ILogger<EventBoardTaskRepository> logger)
            : base(context, timeService, claimsService)
        {
            _context = context;
            _timeService = timeService;
            _claimsService = claimsService;
            _logger = logger;
        }

        public async Task<List<EventBoardTask>> GetTasksByColumnId(Guid eventBoardColumnId)
        {
            return await _context.EventBoardTasks
                                 .Include(t => t.EventBoardColumn)
                                 .Include(t => t.EventBoardTaskAssignments)
                                    .ThenInclude(a => a.User)
                                 .Include(t => t.EventBoardTaskLabelAssignments)
                                    .ThenInclude(l => l.EventBoardTaskLabel)
                                 .Where(t => t.EventBoardColumnId == eventBoardColumnId && !t.IsDeleted)
                                 .ToListAsync();
        }

        public async Task<EventBoardTask> GetTaskById(Guid id)
        {
            return await _context.EventBoardTasks
                                 .Include(t => t.EventBoardColumn)
                                 .Include(t => t.EventBoardTaskAssignments)
                                    .ThenInclude(a => a.User)
                                 .Include(t => t.EventBoardTaskLabelAssignments)
                                    .ThenInclude(l => l.EventBoardTaskLabel)
                                 .FirstOrDefaultAsync(t => t.Id == id && !t.IsDeleted);
        }

        public async Task<EventBoardTask> CreateTask(EventBoardTaskCreateDTO eventBoardTaskCreateDTO)
        {
            using var transaction = await _context.Database.BeginTransactionAsync();

            try
            {
                // Validate if the column exists
                var existingColumn = await _context.EventBoardColumns
                                                   .Where(c => c.Id == eventBoardTaskCreateDTO.EventBoardColumnId)
                                                   .FirstOrDefaultAsync();

                if (existingColumn == null)
                {
                    throw new Exception("EventBoardColumn not found.");
                }

                // Create the new EventBoardTask entity
                var eventBoardTask = new EventBoardTask
                {
                    EventBoardColumnId = eventBoardTaskCreateDTO.EventBoardColumnId,
                    Title = eventBoardTaskCreateDTO.Name,
                    Description = eventBoardTaskCreateDTO.Description,
                    DueDate = eventBoardTaskCreateDTO.DueDate,
                    Priority = eventBoardTaskCreateDTO.Priority,
                    CreatedAt = _timeService.GetCurrentTime(),
                    CreatedBy = _claimsService.GetCurrentUserId,
                    IsDeleted = false
                };

                // Add the EventBoardTask to the context
                await _context.EventBoardTasks.AddAsync(eventBoardTask);
                await _context.SaveChangesAsync();

                // Handle EventBoardTaskLabelAssignments (many-to-many)
                if (eventBoardTaskCreateDTO.EventBoardTaskLabelIds != null && eventBoardTaskCreateDTO.EventBoardTaskLabelIds.Any())
                {
                    var existingLabels = await _context.EventBoardTaskLabels
                                                       .Select(l => l.Id)
                                                       .ToListAsync();

                    var invalidLabelIds = eventBoardTaskCreateDTO.EventBoardTaskLabelIds
                                                               .Except(existingLabels)
                                                               .ToList();

                    if (invalidLabelIds.Any())
                    {
                        var invalidIdsString = string.Join(", ", invalidLabelIds);
                        _logger.LogError($"Invalid label IDs: {invalidIdsString}");
                        throw new Exception($"The following label IDs are invalid: {invalidIdsString}");
                    }

                    foreach (var labelId in eventBoardTaskCreateDTO.EventBoardTaskLabelIds)
                    {
                        var labelAssignment = new EventBoardTaskLabelAssignment
                        {
                            EventBoardTaskId = eventBoardTask.Id,
                            EventBoardTaskLabelId = labelId
                        };

                        await _context.EventBoardTaskLabelAssignments.AddAsync(labelAssignment);
                    }

                    await _context.SaveChangesAsync();
                }

                // Commit the transaction
                await transaction.CommitAsync();

                // Return the created EventBoardTask with its related data
                return await GetTaskById(eventBoardTask.Id);
            }
            catch (Exception ex)
            {
                // Rollback the transaction in case of an error
                await transaction.RollbackAsync();
                _logger.LogError(ex, "An error occurred while creating the event board task.");
                throw new Exception(ex.Message, ex);
            }
        }

        public async Task<EventBoardTask> UpdateTask(Guid taskId, EventBoardTaskUpdateDTO eventBoardTaskUpdateDTO)
        {
            using var transaction = await _context.Database.BeginTransactionAsync();

            try
            {
                // Retrieve the existing EventBoardTask from the database, including its related labels
                var existingTask = await _context.EventBoardTasks
                                                 .Include(t => t.EventBoardTaskLabelAssignments)
                                                 .Include(t => t.EventBoardTaskAssignments)
                                                 .FirstOrDefaultAsync(t => t.Id == taskId && !t.IsDeleted);

                if (existingTask == null)
                {
                    throw new Exception("EventBoardTask not found or has been deleted.");
                }

                // Update the basic properties
                existingTask.Title = eventBoardTaskUpdateDTO.Name;
                existingTask.Description = eventBoardTaskUpdateDTO.Description;
                existingTask.DueDate = eventBoardTaskUpdateDTO.DueDate;
                existingTask.Priority = eventBoardTaskUpdateDTO.Priority;
                existingTask.ModifiedAt = _timeService.GetCurrentTime();
                existingTask.ModifiedBy = _claimsService.GetCurrentUserId;

                // Handle label assignments
                var newLabelIds = eventBoardTaskUpdateDTO.EventBoardTaskLabelIds?.ToList() ?? new List<Guid>();
                var existingLabelIds = existingTask.EventBoardTaskLabelAssignments.Select(l => l.EventBoardTaskLabelId).ToList();

                var labelsToRemove = existingTask.EventBoardTaskLabelAssignments
                                                  .Where(l => !newLabelIds.Contains(l.EventBoardTaskLabelId))
                                                  .ToList();
                _context.EventBoardTaskLabelAssignments.RemoveRange(labelsToRemove);

                var labelsToAdd = newLabelIds.Where(l => !existingLabelIds.Contains(l))
                                             .Select(l => new EventBoardTaskLabelAssignment
                                             {
                                                 EventBoardTaskId = taskId,
                                                 EventBoardTaskLabelId = l
                                             });

                await _context.EventBoardTaskLabelAssignments.AddRangeAsync(labelsToAdd);

                // Handle user assignments
                var newAssignedUserIds = eventBoardTaskUpdateDTO.AssignedUserIds?.ToList() ?? new List<Guid>();
                var existingAssignedUserIds = existingTask.EventBoardTaskAssignments.Select(a => a.UserId).ToList();

                var assignmentsToRemove = existingTask.EventBoardTaskAssignments
                                                      .Where(a => !newAssignedUserIds.Contains(a.UserId))
                                                      .ToList();
                _context.EventBoardTaskAssignments.RemoveRange(assignmentsToRemove);

                var assignmentsToAdd = newAssignedUserIds.Where(u => !existingAssignedUserIds.Contains(u))
                                                         .Select(u => new EventBoardTaskAssignment
                                                         {
                                                             EventBoardTaskId = taskId,
                                                             UserId = u
                                                         });

                await _context.EventBoardTaskAssignments.AddRangeAsync(assignmentsToAdd);

                // Save the updated EventBoardTask and its assignments
                _context.EventBoardTasks.Update(existingTask);
                await _context.SaveChangesAsync();

                // Commit the transaction
                await transaction.CommitAsync();

                // Return the updated EventBoardTask with related data
                return await GetTaskById(existingTask.Id);
            }
            catch (Exception ex)
            {
                // Rollback the transaction in case of an error
                await transaction.RollbackAsync();
                _logger.LogError(ex, "An error occurred while updating the event board task.");
                throw new Exception(ex.Message);
            }
        }

        public async Task DeleteTask(Guid taskId)
        {
            using var transaction = await _context.Database.BeginTransactionAsync();

            try
            {
                var existingTask = await _context.EventBoardTasks
                                                 .FirstOrDefaultAsync(t => t.Id == taskId && !t.IsDeleted);

                if (existingTask == null)
                {
                    throw new Exception("EventBoardTask not found or has been deleted.");
                }

                existingTask.IsDeleted = true;
                _context.EventBoardTasks.Update(existingTask);
                await _context.SaveChangesAsync();

                await transaction.CommitAsync();
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                _logger.LogError(ex, "An error occurred while deleting the event board task.");
                throw new Exception(ex.Message);
            }
        }
    }
}
