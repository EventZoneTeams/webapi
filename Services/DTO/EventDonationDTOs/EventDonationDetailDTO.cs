using Domain.Entities;
using Repositories.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.DTO.EventDonationDTOs
{
    public class EventDonationDetailDTO : EventDonationCreateDTO
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public bool? IsDeleted { get; set; }
        public DateTime? CreatedAt { get; set; }

        public virtual UserDetailsModel? User { get; set; }
    }
}