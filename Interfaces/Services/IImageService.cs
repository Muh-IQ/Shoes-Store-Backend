using System;
using Microsoft.AspNetCore.Http;

namespace Interfaces.Services
{
    public interface IImageService
    {
        Task<string> UploadImageAsync(IFormFile file);
        Task<IEnumerable<string>> UploadRangeOfImagesAsync(IEnumerable<IFormFile> file);
        Task<string> UpdateImageAsync(IFormFile file, string publicId);
        Task<string> UpdateImageByUrlAsync(IFormFile file, string ImageURL);
    }
}
