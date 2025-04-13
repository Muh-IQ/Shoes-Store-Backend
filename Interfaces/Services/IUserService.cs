using Interfaces.DTOs;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Interfaces.Services
{
    public interface IUserService
    {
        Task<int> AddUser(UserDTO user, IFormFile file);
        Task<bool> IsExistsEmail(string email);
        Task<(string AccessToken , string RefreshToken)> Login(string email, string password);
        Task<string> GetAccessToken(string RefreshToken);
        Task<UserDTO> GetUser(int id);
    }
}
