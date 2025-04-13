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
    public class OTPData : IOTP
    {
        public async Task<bool> AddOTP(string email, string otp, DateTime expiryTime)
        {
            bool isSuccess = false;

            try
            {
                using (SqlConnection conn = new SqlConnection(Connection.connectionString))
                {
                    using (SqlCommand cmd = new SqlCommand("SP_AddOTP", conn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@Email", email);
                        cmd.Parameters.AddWithValue("@OTP", otp);
                        cmd.Parameters.AddWithValue("@ExpiryTime", expiryTime);

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

        public async Task<bool> VerifyOTP(string email, string otp)
        {
            bool isVerified = false;

            try
            {
                using (SqlConnection conn = new SqlConnection(Connection.connectionString))
                {
                    using (SqlCommand cmd = new SqlCommand("SP_VerifyOTP", conn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@Email", email);
                        cmd.Parameters.AddWithValue("@OTP", otp);

                        await conn.OpenAsync();
                        int rowsAffected = await cmd.ExecuteNonQueryAsync();
                        isVerified = rowsAffected > 0;
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

            return isVerified;
        }

    }
}
