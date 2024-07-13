﻿using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositories.Repositories
{
    public class EventDonationRepository : GenericRepository<EventDonation>, IEventDonationRepository
    {
        private readonly StudentEventForumDbContext _context;
        private readonly ICurrentTime _timeService;
        private readonly IClaimsService _claimsService;

        public EventDonationRepository(StudentEventForumDbContext context, ICurrentTime timeService, IClaimsService claimsService) : base(context, timeService, claimsService)
        {
            _context = context;
            _timeService = timeService;
            _claimsService = claimsService;
        }

        public async Task<List<EventDonation>> GetAllDonationByCampaignId(int id)
        {
            var data = await _context.EventDonations.Include(x => x.User).Where(c => c.EventCampaignId == id).ToListAsync();
            return data;
        }
    }
}