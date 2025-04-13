using Interfaces.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Interfaces.Repository
{
    public interface IRefreshToken
    {
        Task<bool> AddNewRefreshToken(int userId, string refreshToken, DateTime? ExpiryDate);
        Task<RefreshTokenDTO> GetRefreshToken( string refreshToken);
    }
}
