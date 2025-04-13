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
    public class CategoryData : ICategory
    {
        async public Task<List<CategoryDTO>> GetCategories()
        {
            List<CategoryDTO> categories = new List<CategoryDTO>();

            try
            {
                using (SqlConnection connection = new SqlConnection("Server=.;Database=ShoesStoreDB;Integrated Security=True;TrustServerCertificate=True"))
                {
                    await connection.OpenAsync();

                    using (SqlCommand command = new SqlCommand("sp_GetCategories", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;

                        using (SqlDataReader reader = await command.ExecuteReaderAsync())
                        {
                            while (await reader.ReadAsync())
                            {
                                CategoryDTO category = new CategoryDTO(reader.GetInt32("ID"), reader.GetString("Name"));
                                categories.Add(category);

                            }
                        }
                    }
                }
            }
            catch
            {
                //print error
            }

            return categories;
        }
    }
}
