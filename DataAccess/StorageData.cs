using Interfaces.DTOs;
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
    public class StorageData : IStorage
    {

        public async Task<List<StorageDTO>> GetSizeQuantity(int productID)
        {
            List<StorageDTO> sizesQuantities = new List<StorageDTO>();

            using (SqlConnection connection = new SqlConnection(Connection.connectionString))
            {
                try
                {
                    await connection.OpenAsync();

                    using (SqlCommand cmd = new SqlCommand("SP_GetSizeQuantity", connection))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@ProductID", productID);

                        using (SqlDataReader reader = await cmd.ExecuteReaderAsync())
                        {
                            while (await reader.ReadAsync())
                            {
                                sizesQuantities.Add(new StorageDTO
                                {
                                    Size = reader.GetByte(reader.GetOrdinal("Size")),
                                    Quantity = reader.GetByte(reader.GetOrdinal("Quantity"))
                                });
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

            return sizesQuantities;
        }

    }
}
