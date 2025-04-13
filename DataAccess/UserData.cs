using Interfaces.DTOs;
using Interfaces.DTOs.Custom_DTO;
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
    public class UserData : IUser
    {
        public async Task<int> AddUser(UserDTO user)
        {
            int userId = 0; 

            try
            {
                using (SqlConnection conn = new SqlConnection(Connection.connectionString))
                {
                    await conn.OpenAsync();
                    using (SqlCommand cmd = new SqlCommand("SP_AddNewUser", conn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;

                        cmd.Parameters.AddWithValue("@UserName", user.UserName);
                        cmd.Parameters.AddWithValue("@Email", user.Email);
                        cmd.Parameters.AddWithValue("@Password", user.Password);
                        cmd.Parameters.AddWithValue("@IsAdmine", user.IsAdmine);
                        cmd.Parameters.AddWithValue("@ImagePath", user.ImagePath);


                        object result = await cmd.ExecuteScalarAsync();

                        if (result != null && int.TryParse(result.ToString(), out int insertedId))
                        {
                            userId = insertedId;
                        }
                    }
                }
            }
            catch (SqlException ex)
            {
                Console.WriteLine($"Database Error: {ex.Message}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Unexpected Error: {ex.Message}");
            }

            return userId;
        }


        public async Task<bool> IsExistsEmail(string email)
        {
            bool exists = false; 

            try
            {
                using (SqlConnection conn = new SqlConnection(Connection.connectionString))
                {
                    using (SqlCommand cmd = new SqlCommand("[dbo].[SP_IsExistsEmail]", conn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;

                        cmd.Parameters.AddWithValue("@Email", email);

                        await conn.OpenAsync();

                        object result = await cmd.ExecuteScalarAsync();

                        exists = result != null; 
                    }
                }
            }
            catch (SqlException ex)
            {
                Console.WriteLine($"Database Error: {ex.Message}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Unexpected Error: {ex.Message}");
            }

            return exists;
        }

        public async Task<int> Login(string email, string password)
        {
            int userId = -1; // Default value if authentication fails

            try
            {
                using (SqlConnection conn = new SqlConnection(Connection.connectionString))
                {
                    using (SqlCommand cmd = new SqlCommand("[dbo].[SP_Login]", conn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;

                        cmd.Parameters.AddWithValue("@Email", email);
                        cmd.Parameters.AddWithValue("@Password", password);

                        await conn.OpenAsync();

                        object result = await cmd.ExecuteScalarAsync();

                        if (result != null && int.TryParse(result.ToString(), out int id))
                        {
                            userId = id; // Assign the retrieved ID
                        }
                    }
                }
            }
            catch (SqlException ex)
            {
                Console.WriteLine($"Database Error: {ex.Message}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Unexpected Error: {ex.Message}");
            }

            return userId;
        }

        public async Task<CUserDTO> GetUserByEmail(string email)
        {
            CUserDTO user = null; // Default value if user not found

            try
            {
                using (SqlConnection conn = new SqlConnection(Connection.connectionString))
                {
                    using (SqlCommand cmd = new SqlCommand("[dbo].[SP_GetUserByEmail]", conn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;

                        cmd.Parameters.AddWithValue("@Email", email);

                        await conn.OpenAsync();
                        using (SqlDataReader reader = await cmd.ExecuteReaderAsync())
                        {
                            if (await reader.ReadAsync())
                            {
                                user = new CUserDTO
                                {
                                    ID = reader["ID"] as int?,
                                    UserName = reader["UserName"].ToString(),
                                    ImagePath = reader["ImagePath"].ToString()
                                };
                            }
                        }
                    }
                }
            }
            catch (SqlException ex)
            {
                Console.WriteLine($"Database Error: {ex.Message}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Unexpected Error: {ex.Message}");
            }

            return user;
        }

        public async Task<UserDTO> GetUser(int id)
        {
            UserDTO user = null; // Default value if user not found

            try
            {
                using (SqlConnection conn = new SqlConnection(Connection.connectionString))
                {
                    using (SqlCommand cmd = new SqlCommand("SP_GetUser", conn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;

                        cmd.Parameters.AddWithValue("@ID", id);

                        await conn.OpenAsync();
                        using (SqlDataReader reader = await cmd.ExecuteReaderAsync())
                        {
                            if (await reader.ReadAsync())
                            {
                                user = new UserDTO
                                {
                                    Email = reader["Email"].ToString(),
                                    UserName = reader["UserName"].ToString(),
                                    ImagePath = reader["ImagePath"].ToString()
                                };
                            }
                        }
                    }
                }
            }
            catch (SqlException ex)
            {
                Console.WriteLine($"Database Error: {ex.Message}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Unexpected Error: {ex.Message}");
            }

            return user;
        }
    }
}
