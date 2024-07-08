using Repositories.Models.ImageDTOs;

namespace Repositories.Models
{
    public class EventProductDetailDTO
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public string? Description { get; set; }
        public Int64 Price { get; set; }

        public DateTime CreatedAt { get; set; }
        public virtual ICollection<ImageReturnDTO>? ProductImages { get; set; }
    }
}