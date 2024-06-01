using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Repositories.DTO;
using Repositories.Interfaces;

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
                var productsList = await _context.EventProducts.Where(x => products.Select(x => x.ProductID).Contains(x.Id)).ToListAsync();
                foreach (var product in products)
                {
                    var newProduct = new ProductInPackage
                    {
                        ProductId = product.ProductID,
                        PackageId = newPackage.Id,
                        Quantity = product.Quantity
                    };
                    productsInPackage.Add(newProduct);
                    newPackage.TotalPrice += ((int)productsList.Find(x => x.Id == product.ProductID).Price * product.Quantity);

                }
                _context.Entry(newPackage).State = EntityState.Modified; // UPDATE TỔNG SỐ TIỀN

                await _context.AddRangeAsync(productsInPackage);
                await _context.SaveChangesAsync();
                //await transaction.CommitAsync(); //IMPROVISE CODE
                return productsInPackage;

            }
            catch (Exception)
            {
                // Rollback transaction nếu có lỗi xảy ra
                //  await transaction.RollbackAsync();
                throw;
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
                                                   }).ToList()
                                               }
                    ).ToListAsync();

                return productsInPackage;
            }
            catch (Exception)
            {

                throw;
            }
        }

        public async Task<List<EventPackageDetailDTO>> GetAllPackageWithProductsByEventId(int eventId) // SỬ DỤNG EAGER LOADING
        {
            try
            {
                var eventPackages = await _context.EventPackages
                .Where(x => x.EventId == eventId)
                .Include(x => x.ProductsInPackage)
                .ThenInclude(x => x.EventProduct)
                .ToListAsync();

                var productsInPackage = eventPackages.Select(x => new EventPackageDetailDTO
                {
                    Id = x.Id,
                    EventId = x.EventId,
                    TotalPrice = x.TotalPrice,
                    Description = x.Description,
                    Products = x.ProductsInPackage.Select(p => new EventProductDetailDTO
                    {
                        Id = p.EventProduct.Id,
                        Name = p.EventProduct.Name,
                        Price = p.EventProduct.Price
                    }).ToList()
                }).ToList();

                return productsInPackage ?? new List<EventPackageDetailDTO>();
            }
            catch (Exception)
            {

                throw;
            }
        }






    }
}
