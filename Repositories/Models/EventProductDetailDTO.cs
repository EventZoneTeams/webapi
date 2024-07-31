using Repositories.Models.ImageDTOs;

namespace Repositories.Models
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
        public Int64 Price { get; set; }
        public int QuantityInStock { get; set; }
    }
}