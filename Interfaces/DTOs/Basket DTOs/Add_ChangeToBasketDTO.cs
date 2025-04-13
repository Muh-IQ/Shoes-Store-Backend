using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Interfaces.DTOs.Basket_DTOs
{
    public class Add_ChangeToBasketDTO
    {
        public int ProductID { get; set; }
        public int Quantity { get; set; }
        public int UserID { get; set; }
    }
}
