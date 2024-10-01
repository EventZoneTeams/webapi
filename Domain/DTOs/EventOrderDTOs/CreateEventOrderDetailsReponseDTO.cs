using System.ComponentModel.DataAnnotations;

namespace EventZone.Domain.DTOs.EventOrderDTOs
{
    public class CreateEventOrderDetailsReponseDTO
    {
        [Required(ErrorMessage = "EventProductId is required")]
        public Guid EventProductId { get; set; }

        [Range(1, 10, ErrorMessage = "Quantity must be greater than 0 and lower than 10")]
        [Required(ErrorMessage = "Quantity is required")]
        public int Quantity { get; set; }
    }
}