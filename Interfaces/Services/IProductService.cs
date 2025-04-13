using Interfaces.DTOs.InventoryDTOs;
using Interfaces.DTOs.ProductDTOs;
using Interfaces.Repository;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Interfaces.Services
{
    public interface IProductService
    {
        Task<int> AddNewProduct(
            Dictionary<int, int> sizeQuantity,
            ProductDTO productDTO
            ,IEnumerable<IFormFile>  files);

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
