using Domain.Entities;
using Repositories.Models;
using Services.DTO.EventDTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.DTO.EventFeedbackModel
{
    public class EventFeedbackDetailModel
    {
        public int? EventId { get; set; }
        public string? Content { get; set; }
        public int? UserId { get; set; }

        public virtual EventResponseDTO? Event { get; set; } = null;

        public virtual UserDetailsModel? User { get; set; }
    }
}