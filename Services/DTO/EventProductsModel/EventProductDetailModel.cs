using Domain.Entities;
using Repositories.Models.ImageDTOs;
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
        public bool? IsDeleted { get; set; }
        public DateTime? CreatedAt { get; set; }

        public virtual ICollection<ImageReturnDTO>? ProductImages { get; set; }
    }
}