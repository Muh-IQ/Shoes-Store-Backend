using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Interfaces.DTOs
{
    public class UserDTO
    {
        public int? ID { get; set; } = null;
        public string UserName { get; set; } = "";
        public string Email { get; set; } = "";
        public string Password { get; set; } = "";
        public string ImagePath { get; set; } = "";
        public bool IsAdmine { get; set; } = false;

    }
}
