using Microsoft.EntityFrameworkCore;
using Repositories.DTO;
using Repositories.Entities;
using Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

        public async Task<List<ProductInPackage>> CreatePackageWithProducts(int eventId, List<int> productIds)
        {

            try
            {
                var Event = await _context.Events.FindAsync(eventId);
                if (Event == null)
                {
                    return null;
                }

                var newPackage = new EventPackage
                {
                    EventId = eventId,
                    CreatedAt = _timeService.GetCurrentTime(),
                    CreatedBy = _claimsService.GetCurrentUserId,
                    Description = ""
                };

                await _context.EventPackages.AddAsync(newPackage);
                await _context.SaveChangesAsync();

                int totalPrice = 0;
                List<ProductInPackage> productsInPackage = new List<ProductInPackage>();
                var productsList = await _context.EventProducts.Where(x => productIds.Contains(x.Id)).ToListAsync();
                foreach (var id in productIds) //kiểu này gà quá
                {
                    var newProduct = new ProductInPackage
                    {
                        ProductId = id,
                        PackageId = newPackage.Id,
                        Quantity = 0
                    };
                    productsInPackage.Add(newProduct);
                    totalPrice += (int)productsList.Find(x => x.Id == id).Price;

                }
                newPackage.TotalPrice = totalPrice;
                _context.Update(newPackage);
                //var productsInPackage = productIds.Select(id => new ProductInPackage
                //{
                //    ProductId = id,
                //    PackageId = newPackage.Id
                //}).ToList();
                await _context.AddRangeAsync(productsInPackage);
                await _context.SaveChangesAsync();
                //await transaction.CommitAsync();
                return await _context.ProductInPackages.ToListAsync();

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






    }
}
