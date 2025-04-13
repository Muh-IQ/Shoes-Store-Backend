using Interfaces.DTOs.ProductDTOs;
using Interfaces.Repository;
using Interfaces.Utiltiy.IMisc;
using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess
{
    public class FilterData : IFilter
    {
        public async Task<IEnumerable<ProductCardDTO>> GetPagedFilteredProducts(IDirection Direction, IProperty property, int pageNumber, int pageSize)
        {
            List<ProductCardDTO> products = new List<ProductCardDTO>();

            try
            {
                using (SqlConnection conn = new SqlConnection(Connection.connectionString))
                {
                    using (SqlCommand cmd = new SqlCommand("SP_GetPagedFilteredProducts", conn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@Dirction", Direction.Get());
                        cmd.Parameters.AddWithValue("@ProprtyName", property.Get());
                        cmd.Parameters.AddWithValue("@PageNumber", pageNumber);
                        cmd.Parameters.AddWithValue("@PageSize", pageSize);

                        await conn.OpenAsync();

                        using (SqlDataReader reader = await cmd.ExecuteReaderAsync())
                        {
                            while (await reader.ReadAsync())
                            {
                                products.Add(new ProductCardDTO
                                {
                                    ID = reader.GetInt32(reader.GetOrdinal("ID")),
                                    Title = reader.GetString(reader.GetOrdinal("Title")),
                                    Rate = reader.GetDecimal(reader.GetOrdinal("Rate")),
                                    Price = reader.GetDecimal(reader.GetOrdinal("Price")),
                                    BrandName = reader.GetString(reader.GetOrdinal("BrandName")),
                                    CategoryName = reader.GetString(reader.GetOrdinal("CategoryName")),
                                    ImagePath = reader.IsDBNull(reader.GetOrdinal("ImagePath")) ? null : reader.GetString(reader.GetOrdinal("ImagePath"))
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
                Console.WriteLine($"General Error: {ex.Message}");
            }

            return products;
        }

      
      
    }
}
