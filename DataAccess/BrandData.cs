using Interfaces.DTOs;
using Interfaces.Repository;
using System.Data;
using Microsoft.Data.SqlClient;

namespace DataAccess
{
    public class BrandData :IBrand
    {
        async public Task<List<BrandDTO>> GetBrands()
        {
            List<BrandDTO> brands = new List<BrandDTO>();

            try
            {
                using (SqlConnection connection = new SqlConnection(Connection.connectionString))
                {
                    await connection.OpenAsync();

                    using (SqlCommand command = new SqlCommand("SP_GetBrands", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;

                        using (SqlDataReader reader = await command.ExecuteReaderAsync())
                        {
                            while (await reader.ReadAsync())
                            {
                                BrandDTO brand = new BrandDTO(reader.GetInt32("ID"), reader.GetString("Name"));
                                brands.Add(brand);
                                
                            }
                        }
                    }
                }
            }
            catch
            {
                //print error
            }

            return brands;
        }

        public async Task<int> AddNewBrand(string brandName)
        {
            int newID = -1;
            try
            {
                using (SqlConnection connection = new SqlConnection("Server=.;Database=ShoesStoreDB;Integrated Security=True;TrustServerCertificate=True")) {
                    await connection.OpenAsync();
                    using (SqlCommand command = new SqlCommand("sp_AddNewBrand", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        command.Parameters.AddWithValue("@BrandName", brandName);

                        object? result = await command.ExecuteScalarAsync();

                        if(result != null && int.TryParse(result.ToString(), out int id))
                        {
                            newID = id;
                        }
                        
                    }
                }
            }
            catch { }

            return newID;
        }
    }
}
