using EventZone.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventZone.Domain.DTOs.EventOrderDTOs
{
    public class EventOrderDetailDTO
    {
        public Guid Id { get; set; }
        public Guid EventProductId { get; set; }
        public Guid EventOrderId { get; set; }
        public int Quantity { get; set; }
        public long Price { get; set; }
        public bool? IsReceived { get; set; }
        public virtual EventOrderReponseDTO? EventOrder { get; set; }
    }
}