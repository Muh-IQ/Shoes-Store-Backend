using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Interfaces.DTOs.Basket_DTOs
{
    public class BasketItemDTO
    {
        public int ID { get; set; }
        public int ProductID { get; set; }
        public string ProductTitle { get; set; }
        public int Quantity { get; set; }
        public string ImagePath { get; set; }
        public decimal Price { get; set; }
    }

}
