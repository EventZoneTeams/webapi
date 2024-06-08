using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Repositories.Interfaces;
using Repositories.Models.ImageDTOs;

namespace Repositories.Repositories
{
    public class EventProductRepository : GenericRepository<EventProduct>, IEventProductRepository
    {
        private readonly StudentEventForumDbContext _context;
        private readonly ICurrentTime _timeService;
        private readonly IClaimsService _claimsService;

        public EventProductRepository(StudentEventForumDbContext context, ICurrentTime timeService, IClaimsService claimsService) : base(context, timeService, claimsService)
        {
            _context = context;
            _timeService = timeService;
            _claimsService = claimsService;
        }

        public async Task<List<EventProduct>> GetAllProductsByEvent(int eventId)
        {
            var check = await _context.Events.FindAsync(eventId);
            if ( check == null) {  return null; }
            return await _context.EventProducts.Where(x => x.EventId == eventId).Include(x => x.ProductImages).ToListAsync();
        }

        public async Task<List<EventProduct>> GetAllProductsWithImages()
        {
            return await _context.EventProducts.Include(x => x.ProductImages).ToListAsync();
        }

        public async Task<List<ProductImage>> AddImagesForProduct(int productId, List<ImageReturnDTO> images)
        {
            try
            {
                var product = await GetByIdAsync(productId);
                if (product == null)
                {
                    return null;
                }
                var newImages = new List<ProductImage>();
                foreach (var image in images)
                {
                    if (image == null)
                    {
                        continue;
                    }
                    newImages.Add(new ProductImage()
                    {
                        ProductId = product.Id,
                        ImageUrl = image.ImageUrl,
                        Name = image.Name,
                        EventProduct = product
                    });
                }

                await _context.AddRangeAsync(newImages);
                return newImages;
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}