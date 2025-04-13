using Interfaces.IDTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Interfaces.DTOs.ProductDTOs
{
    public class ProductCardDTO : IProductDTO
    {
        public int ID { get; set; }
        public string Title { get; set; }
        public decimal Rate { get; set; }
        public decimal Price { get; set; }
        public string BrandName { get; set; }
        public string CategoryName { get; set; }
        public string ImagePath { get; set; }

    }
}
