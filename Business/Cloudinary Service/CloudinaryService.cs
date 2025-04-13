using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using Interfaces.Services;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Cloudinary_Service
{
    public class CloudinaryService : IImageService
    {
        private readonly Cloudinary _cloudinary;
        public CloudinaryService()
        {
            _cloudinary = new Cloudinary(Credentials.GetAccount());
        }
        public async Task<string> UploadImageAsync(IFormFile file )
        {
            if (file == null || file.Length == 0)
                throw new ArgumentException("Image File is invalid!");

            using var stream = file.OpenReadStream();
            var uploadParams = new ImageUploadParams
            {
                File = new FileDescription(file.FileName, stream)
            };

            var uploadResult = await _cloudinary.UploadAsync(uploadParams);

            if (uploadResult.StatusCode == System.Net.HttpStatusCode.OK)
            {
                return uploadResult.SecureUrl.ToString();
            }
            else
            {
                throw new Exception("Failed to upload image to Cloudinary");
            }
        }

        public async Task<string> UpdateImageByUrlAsync(IFormFile file, string ImageURL)
        {
            string publicId = GetPublicId(ImageURL);
            return await UpdateImageAsync(file, publicId);
        }
        public async Task<string> UpdateImageAsync(IFormFile file, string publicId)
        {
            if (file == null || file.Length == 0)
                throw new ArgumentException("Image File is invalid!");

            using var stream = file.OpenReadStream();
            var uploadParams = new ImageUploadParams
            {
                File = new FileDescription(file.FileName, stream),
                PublicId = publicId,
                Overwrite = true,
                Invalidate = true
            };

            var uploadResult = await _cloudinary.UploadAsync(uploadParams);

            if (uploadResult.StatusCode == System.Net.HttpStatusCode.OK)
            {
                return uploadResult.SecureUrl.ToString();
            }
            else
            {
                throw new Exception("Failed to upload image to Cloudinary");
            }
        }
        public async Task<IEnumerable<string>> UploadRangeOfImagesAsync(IEnumerable<IFormFile> file)
        {
            List<string> paths = new List<string>();
            foreach (IFormFile item in file)
            {
                string path = await UploadImageAsync(item);
                if (path == null)
                {
                    // delete all image that inserted
                    return null;
                }
                paths.Add(path);
            }
            return paths;
        }
        private string GetPublicId(string imageUrl)
        {
            var fileName = Path.GetFileNameWithoutExtension(imageUrl);
            return fileName;
        }
    }
}
