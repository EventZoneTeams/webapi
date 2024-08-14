using EventZone.Domain.DTOs.ImageDTOs;
using EventZone.Domain.Entities;
using EventZone.Repositories;
using EventZone.Repositories.Commons;
using EventZone.Repositories.Interfaces;
using EventZone.Repositories.Models.ProductModels;
using Microsoft.EntityFrameworkCore;

namespace EventZone.Repositories.Repositories
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

        public async Task<List<EventProduct>> GetAllProductsByEvent(Guid eventId)
        {
            var check = await _context.Events.FindAsync(eventId);
            if (check == null) { return null; }
            return await _context.EventProducts.Where(x => x.EventId == eventId).Include(x => x.ProductImages).ToListAsync();
        }

        public async Task<List<EventProduct>> GetAllProductsWithImages()
        {
            return await _context.EventProducts.Include(x => x.ProductImages).ToListAsync();
        }

        public async Task<List<ProductImage>> AddImagesForProduct(Guid productId, List<ImageReturnDTO> images)
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

        public async Task<Pagination<EventProduct>> GetProductsByFiltersAsync(PaginationParameter paginationParameter, ProductFilterModel productFilterModel)
        {
            try
            {
                var ProductsQuery = _context.EventProducts.Include(x => x.ProductImages).AsNoTracking();
                ProductsQuery = ApplyFilterSortAndSearch(ProductsQuery, productFilterModel);
                var sortedQuery = await ApplySorting(ProductsQuery, productFilterModel).ToListAsync();

                if (sortedQuery != null)
                {
                    var totalCount = sortedQuery.Count;
                    var UsersPagination = sortedQuery
                        .Skip((paginationParameter.PageIndex - 1) * paginationParameter.PageSize)
                        .Take(paginationParameter.PageSize)
                        .ToList();
                    return new Pagination<EventProduct>(UsersPagination, totalCount, paginationParameter.PageIndex, paginationParameter.PageSize);
                }
                return null;
            }
            catch (Exception)
            {
                throw;
            }
        }

        private IQueryable<EventProduct> ApplySorting(IQueryable<EventProduct> query, ProductFilterModel productFilterModel)
        {
            try
            {
                switch (productFilterModel.SortBy.ToLower())
                {
                    case "name":
                        query = productFilterModel.SortDirection.ToLower() == "asc" ? query.OrderBy(a => a.Name) : query.OrderByDescending(a => a.Name);
                        break;

                    case "price":
                        query = productFilterModel.SortDirection.ToLower() == "asc" ? query.OrderBy(a => a.Price) : query.OrderByDescending(a => a.Price);
                        break;

                    default:
                        query = productFilterModel.SortDirection.ToLower() == "asc" ? query.OrderBy(a => a.Id) : query.OrderByDescending(a => a.Id);
                        break;
                }
                return query;
            }
            catch (Exception)
            {
                throw;
            }
        }

        private IQueryable<EventProduct> ApplyFilterSortAndSearch(IQueryable<EventProduct> query, ProductFilterModel productFilterModel)
        {
            if (productFilterModel == null)
            {
                return query;
            }

            if (productFilterModel.isDeleted == true)
            {
                query = query.Where(a => a.IsDeleted == productFilterModel.isDeleted);
            }
            else if (productFilterModel.isDeleted == false)
            {
                query = query.Where(a => a.IsDeleted == productFilterModel.isDeleted);
            }

            if (productFilterModel.EventId.HasValue)
            {
                query = query.Where(p => p.EventId == productFilterModel.EventId);
            }

            if (productFilterModel.MinPrice.HasValue)
            {
                query = query.Where(p => p.Price >= productFilterModel.MinPrice);
            }

            if (productFilterModel.MaxPrice.HasValue)
            {
                query = query.Where(p => p.Price <= productFilterModel.MaxPrice);
            }

            if (!string.IsNullOrEmpty(productFilterModel.SearchName))
            {
                query = query.Where(a =>
                    a.Name.Contains(productFilterModel.SearchName));
            }

            return query;
        }
    }
}