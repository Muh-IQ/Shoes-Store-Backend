using Interfaces.DTOs;
using Interfaces.DTOs.Basket_DTOs;
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
    public class BasketData : IBasket
    {
        public async Task<bool> Add(Add_ChangeToBasketDTO basket)
        {
            bool isSuccess = false;
            try
            {
                using (SqlConnection conn = new SqlConnection(Connection.connectionString))
                {
                    using (SqlCommand cmd = new SqlCommand("SP_AddToBasket", conn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;

                        cmd.Parameters.AddWithValue("@ProductID", basket.ProductID);
                        cmd.Parameters.AddWithValue("@UserID", basket.UserID);
                        cmd.Parameters.AddWithValue("@Quantity", basket.Quantity);

                        await conn.OpenAsync();
                        isSuccess =  await cmd.ExecuteNonQueryAsync() > 0;
                    }
                }

                
            }
            catch (SqlException ex)
            {
                Console.WriteLine($"SQL Error: {ex.Message}");
                return isSuccess;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
                return isSuccess;
            }

            return isSuccess;
        }

        public async Task<IEnumerable<BasketItemDTO>> GetBasketPaged(int userId, int pageNumber, int pageSize)
        {
            List<BasketItemDTO> basketItems = new List<BasketItemDTO>();

            try
            {
                using (SqlConnection conn = new SqlConnection(Connection.connectionString))
                {
                    using (SqlCommand cmd = new SqlCommand("SP_GetBasketPaged", conn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;

                        cmd.Parameters.AddWithValue("@UserID", userId);
                        cmd.Parameters.AddWithValue("@PageNumber", pageNumber);
                        cmd.Parameters.AddWithValue("@PageSize", pageSize);

                        await conn.OpenAsync();
                        using (SqlDataReader reader = await cmd.ExecuteReaderAsync())
                        {
                            while (await reader.ReadAsync())
                            {
                                basketItems.Add(new BasketItemDTO
                                {
                                    ID = reader.GetInt32(reader.GetOrdinal("ID")),
                                    ProductID = reader.GetInt32(reader.GetOrdinal("ProductID")),
                                    ProductTitle = reader.GetString(reader.GetOrdinal("ProductTitle")),
                                    Quantity = reader.GetInt32(reader.GetOrdinal("Quantity")),
                                    ImagePath = reader.GetString(reader.GetOrdinal("ImagePath")),
                                    Price = reader.GetDecimal(reader.GetOrdinal("Price"))
                                });
                            }
                        }
                    }
                }
            }
            catch (SqlException ex)
            {
                Console.WriteLine($"SQL Error: {ex.Message}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
            }

            return basketItems;
        }

        public async Task<bool> ChangeBasketItemQuantity(Add_ChangeToBasketDTO basket)
        {
            bool isSuccess = false;
            try
            {
                using (SqlConnection conn = new SqlConnection(Connection.connectionString))
                {
                    using (SqlCommand cmd = new SqlCommand("SP_ChangeBasketItemQuantity", conn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;

                        cmd.Parameters.AddWithValue("@ProductID", basket.ProductID);
                        cmd.Parameters.AddWithValue("@UserID", basket.UserID);
                        cmd.Parameters.AddWithValue("@Quantity", basket.Quantity);

                        await conn.OpenAsync();
                        isSuccess = await cmd.ExecuteNonQueryAsync() > 0;
                    }
                }
            }
            catch (SqlException ex)
            {
                Console.WriteLine($"SQL Error: {ex.Message}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
            }

            return isSuccess;
        }

        public async Task<bool> DeleteAllBasketItem(int userId)
        {
            bool isSuccess = false;
            try
            {
                using (SqlConnection conn = new SqlConnection(Connection.connectionString))
                {
                    using (SqlCommand cmd = new SqlCommand("SP_DeleteAllBasketItem", conn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@UserID", userId);

                        await conn.OpenAsync();
                        isSuccess = await cmd.ExecuteNonQueryAsync() > 0;
                    }
                }
            }
            catch (SqlException ex)
            {
                Console.WriteLine($"SQL Error: {ex.Message}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
            }

            return isSuccess;
        }

    }
}
