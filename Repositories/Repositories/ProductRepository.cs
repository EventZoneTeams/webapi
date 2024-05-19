using Microsoft.EntityFrameworkCore;
using Repositories.Entities;

namespace Repositories.Repositories
{
    public class ProductRepository
    {
        private StudentEventForumDbContext _context;
        public ProductRepository(StudentEventForumDbContext studentEventForumDbContext)
        {
            _context = studentEventForumDbContext;
        }

        public async Task<List<EventProduct>> GetAllAsync()
        {
            return await _context.EventProducts.ToListAsync();
        }
    }
}
