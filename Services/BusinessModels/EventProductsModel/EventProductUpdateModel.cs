using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Services.BusinessModels.EventProductsModel
{
    public class EventProductUpdateModel
    {

        [Required(ErrorMessage = "Name is required!")]
        public string? Name { get; set; }
        public string? Description { get; set; }

        [Required(ErrorMessage = "Price is required!")]
        [Range(0, 10000000)]
        public decimal Price { get; set; }
        //  [Required(ErrorMessage = "QuantityInStock is required!")]
        [Range(0, 10000000)]
        public int QuantityInStock { get; set; }

    }
}
