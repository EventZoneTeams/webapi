using Domain.DTOs.ImageDTOs;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Services.DTO.EventProductsModel
{
    public class EventProductDetailModel : EventProductUpdateModel
    {
        public int Id { get; set; }
        public int EventId { get; set; }

        public bool? IsDeleted { get; set; }
        public DateTime? CreatedAt { get; set; }

        public virtual ICollection<ImageReturnDTO>? ProductImages { get; set; }
    }
}