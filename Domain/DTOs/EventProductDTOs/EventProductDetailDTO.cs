using EventZone.Domain.DTOs.ImageDTOs;

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