using Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.DTO.EventFeedbackModel
{
    public class CreateFeedbackModel
    {
        public int EventId { get; set; }
        public string Content { get; set; }
        //public int UserId { get; set; }
    }
}