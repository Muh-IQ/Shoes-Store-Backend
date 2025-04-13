using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Interfaces.Repository
{
    public interface IOTP
    {
        Task<bool> AddOTP(string email, string otp, DateTime ExpiryTime);
        Task<bool> VerifyOTP(string email, string otp);
    }
}
