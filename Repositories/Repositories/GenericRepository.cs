using Microsoft.EntityFrameworkCore;
using Repositories.Entities;
using Repositories.Interfaces;

namespace Repositories.Repositories
{
    public class GenericRepository<T> : IGenericRepository<T> where T : BaseEntity
    {
        protected DbSet<T> _dbSet;
        private readonly ICurrentTime _timeService;
        private readonly IClaimsService _claimsService;

        public GenericRepository(StudentEventForumDbContext context, ICurrentTime timeService, IClaimsService claimsService)
        {
            _dbSet = context.Set<T>(); // set entity vào Dbset dựa trên generic
            _timeService = timeService;
            _claimsService = claimsService;
        }
        public async Task AddAsync(T entity)
        {
            entity.CreatedAt = _timeService.GetCurrentTime();
            //entity.CreatedBy = _claimsService.GetCurrentUserId;
            await _dbSet.AddAsync(entity);
        }

        public Task AddRangeAsync(List<T> entities)
        {
            throw new NotImplementedException();
        }

        public Task<List<T>> GetAllAsync()
        {
            return _dbSet.ToListAsync();
        }

        public async Task<T?> GetByIdAsync(int id)
        {
            var result = await _dbSet.FirstOrDefaultAsync(x => x.Id == id);
            // todo should throw exception when not found
            // todo should throw exception when not found
            if (result == null)
            {
                throw new Exception("Not found");
            }
            return result;
        }

        public void SoftRemove(T entity)
        {
            entity.IsDeleted = true;
            //entity.DeletedBy = _claimsService.GetCurrentUserId;
            entity.DeletedAt = _timeService.GetCurrentTime();
            _dbSet.Update(entity);
        }

        public void SoftRemoveRange(List<T> entities)
        {
            foreach (var entity in entities)
            {
                entity.IsDeleted = true;
                //entity.DeletedBy = _claimsService.GetCurrentUserId;
                entity.DeletedAt = _timeService.GetCurrentTime();
            }
            _dbSet.UpdateRange(entities);
        }

        public void Update(T entity)
        {
            entity.ModifiedAt = _timeService.GetCurrentTime();
            //entity.ModifiedBy = _claimsService.GetCurrentUserId;
            _dbSet.Update(entity);
        }

        public void UpdateRange(List<T> entities)
        {
            foreach (var entity in entities)
            {
                entity.CreatedAt = _timeService.GetCurrentTime();
                //entity.CreatedBy = _claimsService.GetCurrentUserId;
            }
            _dbSet.UpdateRange(entities);
        }
    }
}
