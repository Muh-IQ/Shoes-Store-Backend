using Interfaces.DTOs.InventoryDTOs;
using Interfaces.DTOs.ProductDTOs;
using Interfaces.Repository;
using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess
{
    public class ProductData : IProduct
    {
        public async Task<int> AddNewProduct(DataTable sizeQuantityTable, ProductDTO productDTO, DataTable images)
        {
            int productIDOut = 0; // Variable to hold the output ProductID

            using (SqlConnection connection = new SqlConnection(Connection.connectionString))
            {
                try
                {
                    await connection.OpenAsync();

                    // Create a command object for the stored procedure
                    using (SqlCommand cmd = new SqlCommand("SP_AddNewProductWithRelated", connection))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;

                        // Adding input parameters
                        cmd.Parameters.AddWithValue("@Title", productDTO.Title);
                        cmd.Parameters.AddWithValue("@CategoryID", productDTO.CategoryID);
                        cmd.Parameters.AddWithValue("@BrandID", productDTO.BrandID);
                        cmd.Parameters.AddWithValue("@Price", productDTO.Price);

                        // Adding the Table-Valued Parameter for size and quantity
                        SqlParameter tvpParam = cmd.Parameters.AddWithValue("@DataForSize_QuantityAsTable", sizeQuantityTable);
                        tvpParam.SqlDbType = SqlDbType.Structured;
                        SqlParameter tvpParamImage = cmd.Parameters.AddWithValue("@ImagesData", images);
                        tvpParamImage.SqlDbType = SqlDbType.Structured;

                        // Adding the output parameter for ProductID
                        SqlParameter outputProductID = new SqlParameter("@ProductIDOut", SqlDbType.Int);
                        outputProductID.Direction = ParameterDirection.Output;
                        cmd.Parameters.Add(outputProductID);

                        // Execute the stored procedure
                        await cmd.ExecuteNonQueryAsync();

                        // Get the output value for ProductID
                        productIDOut = (int)outputProductID.Value;
                    }
                }
                catch (SqlException ex)
                {
                    // Log or rethrow the error as needed
                    Console.WriteLine($"Error occurred: {ex.Message}");
                    throw;// Rethrow the exception if you need to handle it elsewhere
                }
            }

            return productIDOut;
        }

        public async Task<bool> DeleteProduct(int ProductID)
        {
            using (SqlConnection connection = new SqlConnection(Connection.connectionString))
            {
                try
                {
                    await connection.OpenAsync();

                    using (SqlCommand cmd = new SqlCommand("SP_DeleteProduct", connection))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@ProductID", ProductID);

                        int rowsAffected = await cmd.ExecuteNonQueryAsync();
                        return rowsAffected > 0;
                    }
                }
                catch (SqlException ex)
                {
                    Console.WriteLine($"Error occurred: {ex.Message}");
                    throw;
                }
            }
        }


        public async Task<int> GetCountProducts()
        {
            int productCount = 0;

            using (SqlConnection connection = new SqlConnection(Connection.connectionString))
            {
                try
                {
                    await connection.OpenAsync();

                    using (SqlCommand cmd = new SqlCommand("SP_GetCountProducts", connection))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;

                        productCount = (int)await cmd.ExecuteScalarAsync();
                    }
                }
                catch (SqlException ex)
                {
                    Console.WriteLine($"Error occurred: {ex.Message}");
                    throw;
                }
            }

            return productCount;
        }

        public async Task<ProductSummaryDTO> GetProduct(int productID)
        {
            ProductSummaryDTO product = null;

            using (SqlConnection connection = new SqlConnection(Connection.connectionString))
            {
                try
                {
                    await connection.OpenAsync();

                    using (SqlCommand cmd = new SqlCommand("SP_GetProduct", connection))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@productID", productID);

                        using (SqlDataReader reader = await cmd.ExecuteReaderAsync())
                        {
                            if (await reader.ReadAsync())
                            {
                                product = new ProductSummaryDTO
                                {
                                    ID = reader.GetInt32("ID"), // Products.ID
                                    Title = reader.GetString("Title"), // Products.Title
                                    Rate = reader.IsDBNull(reader.GetOrdinal("Rate")) ? (byte)0 : reader.GetByte(reader.GetOrdinal("Rate")), // معالجة القيم null
                                    Price = reader.GetDecimal("Price"), // Products.Price
                                    CategoryName = reader.GetString("CategoryName"), // Categories.Name
                                    BrandName = reader.GetString("BrandName") // Brands.Name 
                                };
                            }
                        }
                    }
                }
                catch (SqlException ex)
                {
                    Console.WriteLine($"Error occurred: {ex.Message}");
                    throw;
                }
            }

            return product;
        }

        public async Task<List<string>> GetProductImages(int productID)
        {
            List<string> images = new List<string>();

            using (SqlConnection connection = new SqlConnection(Connection.connectionString))
            {
                try
                {
                    await connection.OpenAsync();

                    using (SqlCommand cmd = new SqlCommand("SP_GetProductImages", connection))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@productID", productID);

                        using (SqlDataReader reader = await cmd.ExecuteReaderAsync())
                        {
                            while (await reader.ReadAsync())
                            {
                                images.Add(reader.GetString("ImagePath")); 
                            }
                        }
                    }
                }
                catch (SqlException ex)
                {
                    Console.WriteLine($"Error occurred: {ex.Message}");
                    throw;
                }
            }

            return images;
        }
        
        public async Task<List<ProductDetailsDTO>> GetProductsPaged(int pageNumber, int pageSize)
        {
            List<ProductDetailsDTO> products = new List<ProductDetailsDTO>();

            try
            {
                using (SqlConnection conn = new SqlConnection(Connection.connectionString))
                {
                    using (SqlCommand cmd = new SqlCommand("GetProductsPaged", conn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@PageNumber", pageNumber);
                        cmd.Parameters.AddWithValue("@PageSize", pageSize);

                        await conn.OpenAsync();
                        using (SqlDataReader reader = await cmd.ExecuteReaderAsync())
                        {
                            while (await reader.ReadAsync())
                            {
                                products.Add(new ProductDetailsDTO
                                {
                                    ID = reader.GetInt32("ID"),
                                    Title = reader.GetString("Title"),
                                    CategoryName = reader.GetString("CategoryName"),
                                    BrandName = reader.GetString("BrandName"),
                                    Price = reader.GetDecimal("Price"),
                                    MinQuantity = reader.IsDBNull(5) ? 0 : reader.GetByte("MinQuantity")
                                });
                            }
                        }
                    }
                }
            }
            catch (SqlException ex)
            {
                Console.WriteLine($"Error in database: {ex.Message}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($" Error ouccurs: {ex.Message}");
            }

            return products;
        }
        
        public async Task<List<ProductInventoryDTO>> GetAllSizeQuantity(int ProductID)
        {
            List<ProductInventoryDTO> SizeQuantityList = new List<ProductInventoryDTO>();

            try
            {
                using (SqlConnection conn = new SqlConnection(Connection.connectionString))
                {
                    await conn.OpenAsync();

                    using (SqlCommand command = new SqlCommand("SP_GetSizeQuantity", conn))
                    {
                        command.CommandType = CommandType.StoredProcedure;

                        command.Parameters.AddWithValue("@ProductId", ProductID);

                        using (SqlDataReader reader = await command.ExecuteReaderAsync())
                        {
                            while (await reader.ReadAsync())
                            {
                                ProductInventoryDTO SQ_Row = new ProductInventoryDTO(reader.GetInt32(reader.GetOrdinal("StorageID")),
                                    reader.GetByte(reader.GetOrdinal("Size")),
                                    reader.GetByte(reader.GetOrdinal("Quantity")));

                                SizeQuantityList.Add(SQ_Row);
                            }
                        }
                    }
                }

            }
            catch (Exception ex) { }



            return SizeQuantityList;
        }

        public async Task<int> AddNewSizeQuantity(ProductSizeQuantityCreateDTO SQ_Data)
        {
            int NewStorageID = 0;

            try
            {
                using (SqlConnection conn = new SqlConnection(Connection.connectionString))
                {
                    await conn.OpenAsync();

                    using (SqlCommand command = new SqlCommand("SP_AddNewSizeQuantity", conn))
                    {
                        command.CommandType = CommandType.StoredProcedure;

                        command.Parameters.AddWithValue("@ProductId", SQ_Data.ProductID);
                        command.Parameters.AddWithValue("@NewSize", SQ_Data.Size);
                        command.Parameters.AddWithValue("@NewQuantity", SQ_Data.Quantity);

                        SqlParameter newStorageIdParam = new SqlParameter("@NewStorageID", SqlDbType.Int);
                        newStorageIdParam.Direction = ParameterDirection.Output;
                        command.Parameters.Add(newStorageIdParam);

                        await command.ExecuteNonQueryAsync();

                        NewStorageID = (int)newStorageIdParam.Value;
                    }
                }

            }
            catch (Exception ex) { }



            return NewStorageID;
        }

        public async Task<bool> UpdateProductQuantity(int storageID, byte Quantity)
        {
            bool updateResult = false;
            try
            {
                using (SqlConnection conn = new SqlConnection(Connection.connectionString))
                {
                    await conn.OpenAsync();

                    using (SqlCommand command = new SqlCommand("SP_UpdateProductQuantity", conn))
                    {
                        command.CommandType = CommandType.StoredProcedure;

                        command.Parameters.AddWithValue("@StorageID", storageID);
                        command.Parameters.AddWithValue("@NewQuantity", Quantity);

                        int effectedRows = await command.ExecuteNonQueryAsync();

                        updateResult = (effectedRows > 0);
                    }
                }
            }
            catch (Exception ex)
            { Console.WriteLine("Data Access Error" + ex.Message); }

            return updateResult;
        }

        public async Task<bool> UpdateProductPrice(int ProductID, decimal Price)
        {
            bool updateResult = false;
            try
            {
                using (SqlConnection conn = new SqlConnection(Connection.connectionString))
                {
                    await conn.OpenAsync();

                    using (SqlCommand command = new SqlCommand("SP_UpdateProductPrice", conn))
                    {
                        command.CommandType = CommandType.StoredProcedure;

                        command.Parameters.AddWithValue("@ProductID", ProductID);
                        command.Parameters.AddWithValue("@NewPrice", Price);

                        int effectedRows = await command.ExecuteNonQueryAsync();

                        updateResult = (effectedRows > 0);
                    }
                }
            }
            catch (Exception ex)
            { Console.WriteLine("Data Access Error" + ex.Message); }

            return updateResult;
        }

    }
}