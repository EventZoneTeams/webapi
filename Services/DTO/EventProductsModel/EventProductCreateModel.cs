using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace Services.DTO.EventProductsModel
{
    public class EventProductCreateModel
    {
        [Required(ErrorMessage = "EventID is required!")]
        public Guid EventId { get; set; }

        [Required(ErrorMessage = "Name is required!")]
        public string Name { get; set; }

        public string Description { get; set; } = "";

        [Required(ErrorMessage = "Price is required!")]
        [Range(0, 10000000)]
        public Int64 Price { get; set; }

        //  [Required(ErrorMessage = "QuantityInStock is required!")]
        public int QuantityInStock { get; set; }

        public List<IFormFile>? fileImages { get; set; }
    }
}