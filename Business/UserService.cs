using Business.Cryptography_Service;
using Interfaces.DTOs;
using Interfaces.Repository;
using Interfaces.Services;
using Interfaces.Utiltiy;
using Microsoft.AspNetCore.Http;
using Microsoft.VisualBasic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business
{
    public class UserService(IUser repository, IImageService imageService, IAccessTokenService tokenService 
        , IRefreshTokenService refreshTokenService) : IUserService
    {
        public async Task<int> AddUser(UserDTO user, IFormFile file)
        {
            // check is it image
            CheckTypeOfImage(file);

            // check is exists email 
            bool res =  await IsExistsEmail(user.Email);
            if (res)
                throw new Exception($"This email is already exists {user.Email} , choose another email ");

            //upload image to thired parties
            string ImageURL = await imageService.UploadImageAsync(file);
            if (string.IsNullOrEmpty(ImageURL))
                throw new Exception("An error occurred while post image in third parties");


            // encryption password
            string EncryptedPassword = EncryptionService.HashPassword(user.Password);
            user.Password = EncryptedPassword;


            // set path after save it in external service
            user.ImagePath = ImageURL;
            return await repository.AddUser(user);
        }

        public async Task<string> GetAccessToken(string RefreshToken)
        {
            RefreshTokenDTO tokenDTO = await refreshTokenService.GetRefreshToken(RefreshToken);
            if (tokenDTO.IsRevoked || tokenDTO.ExpiryDate < DateTime.UtcNow)
                return null;

            return tokenService.GenerateAccessToken(tokenDTO.UserId);
        }

        public async Task<UserDTO> GetUser(int id)
        {
           return await  repository.GetUser(id);
        }

        public async Task<bool> IsExistsEmail(string email)
        {
            return await repository.IsExistsEmail(email);
        }

        public async Task<(string AccessToken, string RefreshToken)> Login(string email, string password)
        {
            password = EncryptionService.HashPassword(password);
            int ID = await repository.Login(email, password);
            if (ID > 0)
            {
                (bool IsAdded, string Token) = await refreshTokenService.AddNewRefreshToken(ID);
                if (IsAdded)
                {
                    string AccessToken = tokenService.GenerateAccessToken(ID);
                    return(AccessToken , Token) ;
                }
            }
            return (null, null);
        }
        
        private void CheckTypeOfImage(IFormFile image)
        {
            if (!ImageType.allowedTypes.Contains(image.ContentType))
            {
                throw new Exception("*enire only image");
            }

        }
    }
}
