using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Interfaces.Services
{
    public interface IEmailService
    {
        Task<bool> SendOtpAsync(string toEmail, string otpCode);
    }
}
