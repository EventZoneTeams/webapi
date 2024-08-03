using Domain.DTOs.ImageDTOs;

namespace Domain.DTOs.EventProductDTOs
{
    public class EventProductDetailDTO
    {
        public Guid Id { get; set; }
        public Guid EventId { get; set; }
        public bool? IsDeleted { get; set; }
        public DateTime? CreatedAt { get; set; }
        public virtual ICollection<ImageReturnDTO>? ProductImages { get; set; }

        public string? Name { get; set; }
        public string? Description { get; set; }
        public long Price { get; set; }
        public int QuantityInStock { get; set; }
    }
}