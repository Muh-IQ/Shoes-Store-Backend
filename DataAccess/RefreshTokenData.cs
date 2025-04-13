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
    public class RefreshTokenData : IRefreshToken
    {
        public async Task<bool> AddNewRefreshToken(int userId, string refreshToken, DateTime? expiryDate)
        {
            bool isSuccess = false;

            try
            {
                using (SqlConnection conn = new SqlConnection(Connection.connectionString))
                {
                    using (SqlCommand cmd = new SqlCommand("AddNewRefreshToken", conn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@UserId", userId);
                        cmd.Parameters.AddWithValue("@Token", refreshToken);
                        cmd.Parameters.AddWithValue("@ExpiryDate", expiryDate ?? (object)DBNull.Value);

                        await conn.OpenAsync();
                        int rowsAffected = await cmd.ExecuteNonQueryAsync();
                        isSuccess = rowsAffected > 0;
                    }
                }
            }
            catch (SqlException ex)
            {
                Console.WriteLine($"Database error: {ex.Message}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred: {ex.Message}");
            }

            return isSuccess;
        }

        public async Task<RefreshTokenDTO> GetRefreshToken(string refreshToken)
        {
            RefreshTokenDTO? tokenDetails = null;

            try
            {
                using (SqlConnection conn = new SqlConnection(Connection.connectionString))
                {
                    using (SqlCommand cmd = new SqlCommand("GetRefreshTokenDetails", conn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@Token", refreshToken);

                        await conn.OpenAsync();
                        using (SqlDataReader reader = await cmd.ExecuteReaderAsync())
                        {
                            if (await reader.ReadAsync())
                            {
                                tokenDetails = new RefreshTokenDTO
                                {
                                    UserId = reader.GetInt32("UserId"),
                                    ExpiryDate = reader.GetDateTime("ExpiryDate"),
                                    IsRevoked = reader.GetBoolean("IsRevoked")
                                };
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
                Console.WriteLine($"Error occurs: {ex.Message}");
            }

            return tokenDetails ?? new RefreshTokenDTO(); 
        }
       
    }
}
