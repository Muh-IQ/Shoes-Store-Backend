using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Interfaces.DTOs.InventoryDTOs
{
    public class ProductSizeQuantityCreateDTO
    {
        public int ProductID { get; set; }
        public byte Size { get; set; }
        public byte Quantity { get; set; }
        public ProductSizeQuantityCreateDTO(int productID,byte size,byte quantity)
        {
            ProductID = productID;
            Size = size;
            Quantity = quantity;
        }
    }
}
