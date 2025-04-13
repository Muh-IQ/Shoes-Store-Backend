using Interfaces.DTOs.InventoryDTOs;
using Interfaces.DTOs.ProductDTOs;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Interfaces.Repository
{
    public interface IProduct
    {
        Task<int> AddNewProduct(
        DataTable sizeQuantityTable, ProductDTO productDTO, DataTable images);
        Task<List<ProductDetailsDTO>> GetProductsPaged(int pageNumber, int pageSize);
        Task<int> GetCountProducts();
        Task<ProductSummaryDTO> GetProduct(int ProductID);
        Task<List<string>> GetProductImages(int ProductID);
        Task<bool> DeleteProduct(int ProductID);
        Task<List<ProductInventoryDTO>> GetAllSizeQuantity(int ProductID);
        Task<int> AddNewSizeQuantity(ProductSizeQuantityCreateDTO SQ_Info);
        Task<bool> UpdateProductQuantity(int storageID, byte Quantity);
        Task<bool> UpdateProductPrice(int ProductID, decimal Price);

    }   
}
