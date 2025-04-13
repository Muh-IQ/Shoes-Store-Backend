using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Interfaces.DTOs.InventoryDTOs
{
    public class ProductInventoryDTO
    {
        public int StorageID { get; set; }
        public byte Size { get; set; }
        public byte Quantity { get; set; }
        public ProductInventoryDTO(int storageId,byte size,byte quantity)
        {
            StorageID = storageId;
            Size = size;
            Quantity = quantity;
        }
    }
}
