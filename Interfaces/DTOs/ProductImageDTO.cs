using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Interfaces.DTOs
{
    public class ProductImageDTO
    {
        public int? ID { get; set; }
        public int? ProductID { get; set; }
        public  string ImagePath { get; set; }

        public ProductImageDTO(string imagePath ,int? iD, int? productID)
        {
            ID = iD;
            ProductID = productID;
            ImagePath = imagePath;
        }
      
    }
}
