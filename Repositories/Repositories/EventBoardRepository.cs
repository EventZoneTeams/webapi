using EventZone.Domain.DTOs.EventBoardDTOs;
using EventZone.Domain.Entities;
using EventZone.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;


namespace EventZone.Repositories.Repositories
{
    public class EventBoardRepository : GenericRepository<EventBoard>, IEventBoardRepository
    {
        private readonly StudentEventForumDbContext _context;
        private readonly ICurrentTime _timeService;
        private readonly IClaimsService _claimsService;
        private readonly ILogger<EventBoardRepository> _logger;
        public EventBoardRepository(StudentEventForumDbContext context, ICurrentTime timeService, IClaimsService claimsService, ILogger<EventBoardRepository> logger) : base(context, timeService, claimsService)
        {
            _context = context;
            _timeService = timeService;
            _claimsService = claimsService;
            _logger = logger;
        }

        public async Task<List<EventBoard>> GetBoardsByEventId(Guid eventId)
        {
            return await _context.EventBoards
                                 .Where(b => b.EventId == eventId && !b.IsDeleted)
                                 .ToListAsync();
        }

        public async Task<EventBoard> GetBoardById(Guid id)
        {
            return await _context.EventBoards
                                .Include(b => b.Event) // Include the related Event
                                .Include(b => b.Leader) // Include the related Leader (one-to-one)
                                .Include(b => b.EventBoardColumns) // Include the related EventBoardColumns
                                .Include(b => b.EventBoardTasks) // Include the related EventBoardTasks
                                    .ThenInclude(t => t.EventBoardColumn) // Include the related EventBoardColumn for each task
                                .Include(b => b.EventBoardTasks) // Include the related EventBoardTasks
                                    .ThenInclude(t => t.EventBoardTaskAssignments) // Include task assignments (many-to-many)
                                        .ThenInclude(ta => ta.User) // Include the related User for each assignment
                                .Include(b => b.EventBoardLabelAssignments) // Include the many-to-many relationship with EventBoardLabels
                                    .ThenInclude(l => l.EventBoardLabel) // Include the actual EventBoardLabel in the assignment
                                .Include(b => b.EventBoardTaskLabels) // Include the many-to-many relationship with EventBoardLabels
                                .Include(b => b.EventBoardTasks) // Include the related EventBoardTasks
                                .FirstOrDefaultAsync(b => b.Id == id && !b.IsDeleted);
        }

        public async Task<EventBoard> CreateBoard(EventBoardCreateDTO eventBoardCreateDTO)
        {
            using var transaction = await _context.Database.BeginTransactionAsync();

            try
            {
                var existingEventId = await _context.Events
                    .Where(e => e.Id == eventBoardCreateDTO.EventId)
                    .FirstOrDefaultAsync();

                if (existingEventId == null)
                {
                    throw new Exception("Event not found.");
                }

                // Create the new EventBoard entity
                var eventBoard = new EventBoard
                {
                    EventId = eventBoardCreateDTO.EventId,
                    Name = eventBoardCreateDTO.Name,
                    ImageUrl = eventBoardCreateDTO.ImageUrl,
                    Priority = eventBoardCreateDTO.Priority,
                    Description = eventBoardCreateDTO.Description,
                    CreatedAt = _timeService.GetCurrentTime(),
                    CreatedBy = _claimsService.GetCurrentUserId,
                    IsDeleted = false
                };

                // Add the EventBoard to the context
                await _context.EventBoards.AddAsync(eventBoard);
                await _context.SaveChangesAsync();

                // Handle EventBoardLabelAssignments (many-to-many)
                if (eventBoardCreateDTO.EventBoardLabels != null && eventBoardCreateDTO.EventBoardLabels.Any())
                {
                    var existingLabels = await _context.EventBoardLabels
                        .Select(l => l.Id)
                        .ToListAsync();

                    var invalidLabelIds = eventBoardCreateDTO.EventBoardLabels
                        .Except(existingLabels).ToList();

                    if (invalidLabelIds.Any())
                    {
                        var invalidIdsString = string.Join(", ", invalidLabelIds);
                        _logger.LogError($"Invalid label IDs: {invalidIdsString}");
                        throw new Exception($"The following label IDs are invalid: {invalidIdsString}");
                    }

                    foreach (var labelId in eventBoardCreateDTO.EventBoardLabels)
                    {
                        var labelAssignment = new EventBoardLabelAssignment
                        {
                            EventBoardId = eventBoard.Id,
                            EventBoardLabelId = labelId
                        };

                        await _context.EventBoardLabelAssignments.AddAsync(labelAssignment);
                    }

                    await _context.SaveChangesAsync();
                }

                // Commit the transaction
                await transaction.CommitAsync();

                // Return the created EventBoard with its related data
                return await GetBoardById(eventBoard.Id);
            }
            catch (Exception ex)
            {
                // Rollback the transaction in case of an error
                await transaction.RollbackAsync();
                _logger.LogError(ex, "An error occurred while creating the event board.");
                throw new Exception(ex.Message, ex);
            }
        }

        public async Task<EventBoard> UpdateBoard(Guid boardId, EventBoardUpdateDTO eventBoardUpdateDTO)
        {
            using var transaction = await _context.Database.BeginTransactionAsync();

            try
            {
                // Retrieve the existing EventBoard from the database, including its related labels
                var existingBoard = await _context.EventBoards
                    .Include(b => b.EventBoardLabelAssignments)
                    .FirstOrDefaultAsync(b => b.Id == boardId && !b.IsDeleted);

                if (existingBoard == null)
                {
                    throw new Exception("EventBoard not found or has been deleted.");
                }

                // Update the basic properties
                existingBoard.Name = eventBoardUpdateDTO.Name;
                existingBoard.ImageUrl = eventBoardUpdateDTO.ImageUrl;
                existingBoard.Priority = eventBoardUpdateDTO.Priority;
                existingBoard.Description = eventBoardUpdateDTO.Description;
                existingBoard.ModifiedAt = _timeService.GetCurrentTime();
                existingBoard.ModifiedBy = _claimsService.GetCurrentUserId;

                // Validate the new labels
                var newLabelIds = eventBoardUpdateDTO.EventBoardLabels.ToList();
                var existingLabels = await _context.EventBoardLabels
                    .Where(l => newLabelIds.Contains(l.Id))
                    .Select(l => l.Id)
                    .ToListAsync();

                var invalidLabelIds = newLabelIds.Except(existingLabels).ToList();

                if (invalidLabelIds.Any())
                {
                    var invalidIdsString = string.Join(", ", invalidLabelIds);
                    _logger.LogError($"Invalid label IDs: {invalidIdsString}");
                    throw new Exception($"The following label IDs are invalid: {invalidIdsString}");
                }

                // Remove old labels that are no longer in the new label list
                var labelsToRemove = existingBoard.EventBoardLabelAssignments
                    .Where(l => !newLabelIds.Contains(l.EventBoardLabelId))
                    .ToList();

                _context.EventBoardLabelAssignments.RemoveRange(labelsToRemove);

                // Add new labels that weren't already assigned
                var existingLabelIds = existingBoard.EventBoardLabelAssignments.Select(l => l.EventBoardLabelId).ToList();
                var labelsToAdd = newLabelIds
                    .Where(l => !existingLabelIds.Contains(l))
                    .Select(l => new EventBoardLabelAssignment
                    {
                        EventBoardId = boardId,
                        EventBoardLabelId = l
                    });

                await _context.EventBoardLabelAssignments.AddRangeAsync(labelsToAdd);

                // Save the updated EventBoard and its label assignments
                _context.EventBoards.Update(existingBoard);
                await _context.SaveChangesAsync();

                // Commit the transaction
                await transaction.CommitAsync();

                // Return the updated EventBoard with related data
                return await GetBoardById(existingBoard.Id);
            }
            catch (Exception ex)
            {
                // Rollback the transaction in case of an error
                await transaction.RollbackAsync();
                _logger.LogError(ex, "An error occurred while updating the event board.");
                throw new Exception(ex.Message);
            }
        }

        public async Task SoftDeleteBoard(Guid id)
        {

        }
    }
}
