using Interfaces.DTOs.InventoryDTOs;
using Interfaces.DTOs.ProductDTOs;
using Interfaces.Repository;
using Interfaces.Services;
using Interfaces.Utiltiy;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;

namespace Business
{
    public class ProductService(IProduct repository, IImageService imageService) : IProductService
    {
     
        public async Task<int> AddNewProduct(Dictionary<int, int> sizeQuantity, ProductDTO productDTO, IEnumerable<IFormFile> files)
        {
            CheckTypeOfImages(files);
            IEnumerable<string> ImagesURL = await imageService.UploadRangeOfImagesAsync(files);
            if (ImagesURL == null)
                throw new Exception("An error occurred while post image in third parties");

            DataTable dt = ConvertDictionaryToDataTable(sizeQuantity);
            DataTable urls = ConverIEnumerableToDataTable(ImagesURL);
            return await repository.AddNewProduct(dt, productDTO, urls);
        }

        private DataTable ConvertDictionaryToDataTable(Dictionary<int, int> sizeQuantity)
        {
            // Create a new DataTable and define its columns
            DataTable dt = new DataTable();
            dt.Columns.Add("Size", typeof(byte));      // SQL tinyint maps to C# byte
            dt.Columns.Add("Quantity", typeof(byte));  // SQL tinyint maps to C# byte

            // Iterate through each key-value pair in the dictionary
            foreach (var kvp in sizeQuantity)
            {
                DataRow row = dt.NewRow();
                row["Size"] = (byte)kvp.Key;         // Cast the key to byte
                row["Quantity"] = (byte)kvp.Value;     // Cast the value to byte
                dt.Rows.Add(row);
            }

            return dt;
        }
        private DataTable ConverIEnumerableToDataTable(IEnumerable<string> imagesURL)
        {
            // Create a new DataTable and define its columns
            DataTable dt = new DataTable();
            dt.Columns.Add("Path", typeof(string));
            foreach (string url in imagesURL)
            {
                DataRow row = dt.NewRow();
                row["Path"] = url;    
                dt.Rows.Add(row);
            }

            return dt;
        }
        private void CheckTypeOfImages(IEnumerable<IFormFile> images)
        {
            foreach (IFormFile image in images)
            {
                if (!ImageType.allowedTypes.Contains(image.ContentType))
                {
                    throw new Exception("*enire only images");
                }
            }

        }

        public async Task<List<ProductDetailsDTO>> GetProductsPaged(int pageNumber, int pageSize)
        {
            return await repository.GetProductsPaged(pageNumber, pageSize);
        }

        public async Task<int> GetCountProducts()
        {
            return await repository.GetCountProducts();
        }

        public async Task<ProductSummaryDTO> GetProduct(int ProductID)
        {
            return await repository.GetProduct(ProductID);
        }

        public async Task<List<string>> GetProductImages(int ProductID)
        {
            return await repository.GetProductImages(ProductID);
        }

        public async Task<bool> DeleteProduct(int ProductID)
        {
            return await repository.DeleteProduct(ProductID);
        }

        public async Task<List<ProductInventoryDTO>> GetAllSizeQuantity(int ProductID)
        {
            return await repository.GetAllSizeQuantity(ProductID);
        }

        public async Task<int> AddNewSizeQuantity(ProductSizeQuantityCreateDTO SQ_Data)
        {
            return await repository.AddNewSizeQuantity(SQ_Data);
        }

        public async Task<bool> UpdateProductQuantity(int storageID, byte Quantity)
        {
            return await repository.UpdateProductQuantity(storageID, Quantity);
        }

        public async Task<bool> UpdateProductPrice(int ProductID, decimal Price)
        {
            return await repository.UpdateProductPrice(ProductID, Price);
        }

    }
}
