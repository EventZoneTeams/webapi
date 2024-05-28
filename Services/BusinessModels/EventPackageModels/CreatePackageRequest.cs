﻿using Repositories.DTO;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.BusinessModels.EventPackageModels
{
    public class CreatePackageRequest
    {
        public string Description { get; set; } = string.Empty;
        [Required(ErrorMessage = "Please input product list to add package")]
        public List<ProductQuantityDTO> Products { get; set; }
    }
}