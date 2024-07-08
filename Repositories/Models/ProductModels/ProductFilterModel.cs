using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositories.Models.ProductModels
{
    public class ProductFilterModel
    {
        public string SortBy { get; set; } = "id";
        public string SortDirection { get; set; } = "desc";
        public string? SearchName { get; set; }
        public int? EventId { get; set; }
        public Int64? MinPrice { get; set; }
        public Int64? MaxPrice { get; set; }

        public bool? isDeleted { get; set; } = null;
    }
}