using Domain.DTOs.EventProductDTOs;

namespace Domain.DTOs.EventPackageDTOs
{
    public class EventPackageDetailDTO
    {
        public Guid Id { get; set; }
        public Guid EventId { get; set; }
        public string? Title { get; set; }

        public long TotalPrice { get; set; }
        public string Description { get; set; }
        public string? ThumbnailUrl { get; set; }
        public bool? IsDeleted { get; set; } = false;
        public virtual ICollection<ProductInPackageDTO>? ProductsInPackage { get; set; }

        //tam thoi
        public List<EventProductDetailDTO>? Products { get; set; }
    }
}