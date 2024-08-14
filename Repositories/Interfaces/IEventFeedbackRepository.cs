﻿using EventZone.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventZone.Repositories.Interfaces
{
    public interface IEventFeedbackRepository : IGenericRepository<EventFeedback>
    {
        Task<EventFeedback> CreateFeedbackAsync(EventFeedback newFeedback);

        Task<List<EventFeedback>> GettAllFeedbacksAsync();
    }
}