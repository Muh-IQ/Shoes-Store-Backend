using Interfaces.Services;
using Microsoft.Extensions.Configuration;
using SendGrid;
using SendGrid.Helpers.Mail;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace Business.EmailService
{
    public class EmailService : IEmailService
    {
        private readonly string _apiKey;
        private readonly string _senderEmail;

        public  EmailService(IConfiguration configuration)
        {
            _apiKey = configuration["SendGrid:ApiKey"];
            _senderEmail = configuration["SendGrid:SenderEmail"];
        }
        public async Task<bool> SendOtpAsync(string toEmail, string otpCode)
        {
            var client = new SendGridClient(_apiKey);
            var msg = new SendGridMessage()
            {
                From = new EmailAddress(_senderEmail, "Your App"),
                Subject = "Your OTP Code",
                PlainTextContent = $"Your OTP code is: {otpCode}",
                HtmlContent = $"<strong>Your OTP code is: {otpCode}</strong>"
            };
            msg.AddTo(new EmailAddress(toEmail));

            var response = await client.SendEmailAsync(msg);
            if (!response.IsSuccessStatusCode)
            {
                return false;
                throw new Exception("Failed to send OTP email.");
            }
            return true;
        }

        
    }
}
