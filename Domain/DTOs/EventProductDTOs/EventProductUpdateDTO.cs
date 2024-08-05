using System.ComponentModel.DataAnnotations;

namespace Domain.DTOs.EventProductDTOs
{
    public class EventProductUpdateDTO
    {
        [Required(ErrorMessage = "Name is required!")]
        public string? Name { get; set; }

        public string? Description { get; set; }

        [Required(ErrorMessage = "Price is required!")]
        [Range(0, 10000000)]
        public long Price { get; set; }

        //  [Required(ErrorMessage = "QuantityInStock is required!")]
        [Range(0, 10000000)]
        public int QuantityInStock { get; set; }
    }
}