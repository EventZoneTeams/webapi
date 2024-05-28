﻿using Domain.Enums;
using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace Services.BusinessModels.EventModels
{
    public class CreateEventModel
    {
        [Required]
        public string Name { get; set; }
        public string? Description { get; set; }
        public IFormFile? ThumbnailUrl { get; set; }
        public DateTime? DonationStartDate { get; set; }
        public DateTime? DonationEndDate { get; set; }
        public DateTime? EventStartDate { get; set; }
        public DateTime? EventEndDate { get; set; }
        public string? Location { get; set; }
        [Required]
        public int UserId { get; set; }
        [Required]
        public int EventCategoryId { get; set; }
        public string? University { get; set; }
        [Required]
        public EventStatusEnums Status { get; set; } = EventStatusEnums.PENDING;
        [Required]
        public OriganizationStatusEnums OriganizationStatus { get; set; } = OriganizationStatusEnums.PREPARING;
        [Required]
        public bool? IsDonation { get; set; }
        public decimal? TotalCost { get; set; }
    }
}
