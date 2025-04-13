using Interfaces.Repository;
using Interfaces.Services;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business
{
    public class OTPService(IOTP repository, IEmailService emailService, IConfiguration configuration) : IOTPService
    {
        public async Task<bool> AddOTP(string email)
        {
            // get otp from Email Service
            string otp = GenerateNumericOTP();
            DateTime expires = DateTime.Now.AddMinutes(int.Parse(configuration["SendGrid:OTPExpirationMinutes"]));

            bool res = await emailService.SendOtpAsync(email, otp);
            if (res)
            {
                return await repository.AddOTP(email, otp, expires);
            }
            return res;
        }

        public string GenerateNumericOTP(int length = 6)
        {

            Random random = new Random();
            return new string(Enumerable.Repeat("0123456789", length)
                .Select(s => s[random.Next(s.Length)]).ToArray());
        }

        public async Task<bool> VerifyOTP(string email, string otp)
        {
            return await repository.VerifyOTP(email, otp);
        }
    }
}
