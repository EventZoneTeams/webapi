using EventZone.Domain.DTOs.ImageDTOs;

namespace EventZone.Domain.DTOs.EventProductDTOs
{
    public class EventProductDetailDTO : EventProductUpdateDTO
    {
        public Guid Id { get; set; }

        public Guid EventId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public long Price { get; set; }
        public int QuantityInStock { get; set; }

        public bool? IsDeleted { get; set; }
        public DateTime? CreatedAt { get; set; }
        public virtual ICollection<ImageReturnDTO>? ProductImages { get; set; }
    }
}