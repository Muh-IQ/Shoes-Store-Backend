using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Interfaces.DTOs.ProductDTOs
{
    public class ProductDTO
    {
        public int? ID { get; set; }
        public  string Title { get; set; }
        public  int CategoryID { get; set; }
        public  int BrandID { get; set; }
        public byte? Rate { get; set; }
        public  decimal Price { get; set; }

        public ProductDTO(int? iD, string title, int categoryID, int brandID, byte? rate, decimal price)
        {
            ID = iD;
            Title = title;
            CategoryID = categoryID;
            BrandID = brandID;
            Rate = rate;
            Price = price;
        }
    }
}
