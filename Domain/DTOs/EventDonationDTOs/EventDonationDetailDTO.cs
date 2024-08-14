using Domain.Entities;
using EventZone.Domain.DTOs.UserDTOs;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventZone.Domain.DTOs.EventDonationDTOs
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