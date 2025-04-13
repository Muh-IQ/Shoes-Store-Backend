using Interfaces.IDTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Interfaces.DTOs.ProductDTOs
{
    public class ProductDetailsDTO : IProductDTO
    {
        public int ID { get; set; }
        public string Title { get; set; }
        public string CategoryName { get; set; }
        public string BrandName { get; set; }
        public decimal Price { get; set; }
        public int MinQuantity { get; set; }
    }
}
