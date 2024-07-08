using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Repositories.Commons;
using Repositories.Interfaces;
using Repositories.Models.EventCampaignModels;
using Repositories.Models.PackageModels;
using Repositories.Models.ProductModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositories.Repositories
{
    public class EventCampaignRepository : GenericRepository<EventCampaign>, IEventCampaignRepository
    {
        private readonly StudentEventForumDbContext _context;
        private readonly ICurrentTime _timeService;
        private readonly IClaimsService _claimsService;

        public EventCampaignRepository(StudentEventForumDbContext context, ICurrentTime timeService, IClaimsService claimsService) : base(context, timeService, claimsService)
        {
            _context = context;
            _timeService = timeService;
            _claimsService = claimsService;
        }

        public async Task<List<EventCampaign>> GetAllCampaignByEvent(int id)
        {
            var data = await _context.EventCampaigns.Where(c => c.EventId == id).ToListAsync();
            return data;
        }

        public async Task<Pagination<EventCampaign>> GetCampaignsByFilterAsync(PaginationParameter paginationParameter, CampaignFilterModel campaignFilter)
        {
            try
            {
                var CampaignsQuery = _context.EventCampaigns.AsNoTracking();
                CampaignsQuery = ApplyFilterSortAndSearch(CampaignsQuery, campaignFilter);
                var sortedQuery = await ApplySorting(CampaignsQuery, campaignFilter).ToListAsync();

                if (sortedQuery != null)
                {
                    var totalCount = sortedQuery.Count;
                    var CampaignsPagination = sortedQuery
                        .Skip((paginationParameter.PageIndex - 1) * paginationParameter.PageSize)
                        .Take(paginationParameter.PageSize)
                        .ToList();
                    return new Pagination<EventCampaign>(CampaignsPagination, totalCount, paginationParameter.PageIndex, paginationParameter.PageSize);
                }
                return null;
            }
            catch (Exception)
            {
                throw;
            }
        }

        private IQueryable<EventCampaign> ApplySorting(IQueryable<EventCampaign> query, CampaignFilterModel campaignFilterModel)
        {
            try
            {
                switch (campaignFilterModel.SortBy.ToLower())
                {
                    case "name":
                        query = (campaignFilterModel.SortDirection.ToLower() == "asc") ? query.OrderBy(a => a.Name) : query.OrderByDescending(a => a.Name);
                        break;

                    case "date":
                        query = (campaignFilterModel.SortDirection.ToLower() == "asc") ? query.OrderBy(a => a.StartDate) : query.OrderByDescending(a => a.StartDate);
                        break;

                    default:
                        query = (campaignFilterModel.SortDirection.ToLower() == "asc") ? query.OrderBy(a => a.Id) : query.OrderByDescending(a => a.Id);
                        break;
                }
                return query;
            }
            catch (Exception)
            {
                throw;
            }
        }

        private IQueryable<EventCampaign> ApplyFilterSortAndSearch(IQueryable<EventCampaign> query, CampaignFilterModel campaignFilterModel)
        {
            if (campaignFilterModel == null)
            {
                return query;
            }

            if (campaignFilterModel.isDeleted == true)
            {
                query = query.Where(a => a.IsDeleted == campaignFilterModel.isDeleted);
            }
            else if (campaignFilterModel.isDeleted == false)
            {
                query = query.Where(a => a.IsDeleted == campaignFilterModel.isDeleted);
            }

            if (campaignFilterModel.EventId.HasValue)
            {
                query = query.Where(p => p.EventId == campaignFilterModel.EventId);
            }

            if (campaignFilterModel.StartDate.HasValue)
            {
                query = query.Where(p => p.StartDate >= campaignFilterModel.StartDate);
            }

            if (campaignFilterModel.EndDate.HasValue)
            {
                query = query.Where(p => p.EndDate <= campaignFilterModel.EndDate);
            }

            return query;
        }
    }
}