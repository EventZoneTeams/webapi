using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.DTO.EventProductsModel
{
    public class EventProductCreateModel
    {
        [Required(ErrorMessage = "EventID is required!")]
        public int EventId { get; set; }

        [Required(ErrorMessage = "Name is required!")]
        public string Name { get; set; }

        public string Description { get; set; } = "";

        [Required(ErrorMessage = "Price is required!")]
        [Range(0, 10000)]
        public decimal Price { get; set; }

        //  [Required(ErrorMessage = "QuantityInStock is required!")]
        public int QuantityInStock { get; set; }

        public List<IFormFile>? fileImages { get; set; }
    }
}