using Repositories.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositories.DTO
{
    public class ProductInPackageDTO
    {
        public int ProductId { get; set; }
        public int PackageId { get; set; }
        public int Quantity { get; set; }

        public virtual EventProductDetailDTO? EventProduct { get; set; }
        public virtual EventPackageDetailDTO? EventPackage { get; set; }


    }
}
