using Interfaces.DTOs;
using Interfaces.Repository;
using Interfaces.Services;
using Microsoft.Extensions.Configuration;
using System.Security.Cryptography;
using System.Text;

namespace Business.AuthsService
{
    public class RefreshTokenService(IRefreshToken repository, IConfiguration configuration) : IRefreshTokenService
    {
        private readonly char[] ValidCharacters =
       "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789".ToCharArray();

        public async Task<(bool IsAdded, string Token)> AddNewRefreshToken(int userId)
        {
            string token = GenerateRefreshToken();
            bool IsAdded = await repository.AddNewRefreshToken(userId, token, GetRefreshTokenExpiration());
            return (IsAdded, token);
        }

        public async Task<RefreshTokenDTO> GetRefreshToken(string refreshToken)
        {
            return await repository.GetRefreshToken(refreshToken);
        }

        public DateTime GetRefreshTokenExpiration()
        {
            int days = int.Parse(configuration["JwtSettings:RefreshTokenExpirationDays"]);
            return DateTime.UtcNow.AddDays(days);
        }

        public string GenerateRefreshToken()
        {
            int length = 32;
            var token = new StringBuilder(length);
            var buffer = new byte[length];

            RandomNumberGenerator.Fill(buffer); // توليد أرقام عشوائية

            for (int i = 0; i < length; i++)
            {
                token.Append(ValidCharacters[buffer[i] % ValidCharacters.Length]);
            }

            return token.ToString();
        }

       
    }
}
