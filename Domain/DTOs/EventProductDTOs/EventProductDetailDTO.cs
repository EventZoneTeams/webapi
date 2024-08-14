using Domain.Entities;
using EventZone.Domain.DTOs.ImageDTOs;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace EventZone.Domain.DTOs.EventProductDTOs
{
    public class EventProductDetailDTO : EventProductUpdateDTO
    {
        public Guid Id { get; set; }
        public Guid EventId { get; set; }

        public bool? IsDeleted { get; set; }
        public DateTime? CreatedAt { get; set; }

        public virtual ICollection<ImageReturnDTO>? ProductImages { get; set; }
    }
}