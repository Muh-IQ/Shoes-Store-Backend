using Interfaces.DTOs;
using Interfaces.DTOs.Custom_DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Interfaces.Repository
{
    public interface IUser
    {
        Task<int> AddUser(UserDTO user);
        Task<bool> IsExistsEmail(string email);
        Task<int> Login(string email, string password);
        Task<UserDTO> GetUser(int id);
        Task<CUserDTO> GetUserByEmail(string email);
    }
}
