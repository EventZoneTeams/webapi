using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Repositories.Commons;
using Repositories.Interfaces;
using Repositories.Models;
using Repositories.Models.PackageModels;
using Repositories.Models.ProductModels;

namespace Repositories.Repositories
{
    public class EventPackageRepository : GenericRepository<EventPackage>, IEventPackageRepository
    {
        private readonly StudentEventForumDbContext _context;
        private readonly ICurrentTime _timeService;
        private readonly IClaimsService _claimsService;

        public EventPackageRepository(StudentEventForumDbContext context, ICurrentTime timeService, IClaimsService claimsService) : base(context, timeService, claimsService)
        {
            _context = context;
            _timeService = timeService;
            _claimsService = claimsService;
        }

        public async Task<List<EventPackage>> GetAllPackgesByEventId(int eventId)
        {
            var result = await _context.EventPackages.Where(x => x.EventId == eventId).ToListAsync();

            return result;
        }

        public async Task<List<ProductInPackage>> CreatePackageWithProducts(int eventId, string description, string thumbnailUrl, List<ProductQuantityDTO> products)
        {
            try
            {
                var newPackage = new EventPackage
                {
                    EventId = eventId,
                    CreatedAt = _timeService.GetCurrentTime(),
                    CreatedBy = _claimsService.GetCurrentUserId,
                    Description = description,
                    ThumbnailUrl = thumbnailUrl
                };

                await _context.EventPackages.AddAsync(newPackage);
                await _context.SaveChangesAsync();

                List<ProductInPackage> productsInPackage = new List<ProductInPackage>();
                var productsList = await _context.EventProducts.Where(x => products.Select(x => x.productid).Contains(x.Id)).ToListAsync();
                if (productsList.Count == 0)
                {
                    throw new Exception("Product is not existing in system, please add product");
                }
                foreach (var product in products)
                {
                    var newProduct = new ProductInPackage
                    {
                        ProductId = product.productid,
                        PackageId = newPackage.Id,
                        Quantity = product.quantity
                    };
                    productsInPackage.Add(newProduct);
                    newPackage.TotalPrice += (productsList.Find(x => x.Id == product.productid).Price * product.quantity);
                }
                _context.Entry(newPackage).State = EntityState.Modified; // UPDATE TỔNG SỐ TIỀN

                await _context.AddRangeAsync(productsInPackage);
                await _context.SaveChangesAsync();
                //await transaction.CommitAsync(); //IMPROVISE CODE
                return productsInPackage;
            }
            catch (Exception ex)
            {
                // Rollback transaction nếu có lỗi xảy ra
                //  await transaction.RollbackAsync();
                throw new Exception(ex.Message);
            }
        }

        public async Task<List<ProductInPackage>> GetProductsInPackagesWithProduct()
        {
            try
            {
                var result = await _context.ProductInPackages.Include(x => x.EventPackage).Include(x => x.EventProduct).ToListAsync();
                return result;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<List<EventPackageDetailDTO>> GetAllPackageWithProducts()
        {
            try
            {
                var eventPackages = _context.EventPackages;

                var productsInPackage = await (from package in eventPackages
                                               join productInPackage in _context.ProductInPackages on package.Id equals productInPackage.PackageId
                                               join product in _context.EventProducts on productInPackage.ProductId equals product.Id
                                               group new { package, product } by package.Id into packageProductsGroup
                                               select new EventPackageDetailDTO
                                               {
                                                   Id = packageProductsGroup.Key,
                                                   EventId = packageProductsGroup.First().package.EventId,
                                                   TotalPrice = packageProductsGroup.First().package.TotalPrice,
                                                   Description = packageProductsGroup.First().package.Description,
                                                   Products = packageProductsGroup.Select(x => new EventProductDetailDTO
                                                   {
                                                       Id = x.product.Id,
                                                       Name = x.product.Name,
                                                       Price = x.product.Price
                                                   }).ToList(),
                                                   ThumbnailUrl = packageProductsGroup.First().package.ThumbnailUrl
                                               }
                    ).ToListAsync();

                return productsInPackage;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<List<EventPackage>> GetAllPackageWithProductsByEventId(int eventId) // SỬ DỤNG EAGER LOADING
        {
            try
            {
                var eventPackages = await _context.EventPackages
                .Where(x => x.EventId == eventId)
                .Include(x => x.ProductsInPackage)
                .ThenInclude(x => x.EventProduct).ThenInclude(x => x.ProductImages)
                .ToListAsync();

                return eventPackages;
            }
            catch (Exception)
            {
                throw;
            }
        }

        private IQueryable<EventPackage> ApplySorting(IQueryable<EventPackage> query, PackageFilterModel packageFilterModel)
        {
            try
            {
                switch (packageFilterModel.SortBy.ToLower())
                {
                    case "price":
                        query = (packageFilterModel.SortDirection.ToLower() == "asc") ? query.OrderBy(a => a.TotalPrice) : query.OrderByDescending(a => a.TotalPrice);
                        break;

                    default:
                        query = (packageFilterModel.SortDirection.ToLower() == "asc") ? query.OrderBy(a => a.Id) : query.OrderByDescending(a => a.Id);
                        break;
                }
                return query;
            }
            catch (Exception)
            {
                throw;
            }
        }

        private IQueryable<EventPackage> ApplyFilterSortAndSearch(IQueryable<EventPackage> query, PackageFilterModel productFilterModel)
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

            if (productFilterModel.MinTotalPrice.HasValue)
            {
                query = query.Where(p => p.TotalPrice >= productFilterModel.MinTotalPrice);
            }

            if (productFilterModel.MaxTotalPrice.HasValue)
            {
                query = query.Where(p => p.TotalPrice <= productFilterModel.MaxTotalPrice);
            }

            return query;
        }

        public async Task<Pagination<EventPackage>> GetPackagessByFiltersAsync(PaginationParameter paginationParameter, PackageFilterModel packageFilterModel)
        {
            try
            {
                var PackagesQuery = _context.EventPackages
                    .Include(x => x.ProductsInPackage).
                    ThenInclude(x => x.EventProduct).ThenInclude(x => x.ProductImages)
                    .AsNoTracking();
                PackagesQuery = ApplyFilterSortAndSearch(PackagesQuery, packageFilterModel);
                var sortedQuery = await ApplySorting(PackagesQuery, packageFilterModel).ToListAsync();

                if (sortedQuery != null)
                {
                    var totalCount = sortedQuery.Count;
                    var UsersPagination = sortedQuery
                        .Skip((paginationParameter.PageIndex - 1) * paginationParameter.PageSize)
                        .Take(paginationParameter.PageSize)
                        .ToList();
                    return new Pagination<EventPackage>(UsersPagination, totalCount, paginationParameter.PageIndex, paginationParameter.PageSize);
                }
                return null;
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}