using Interfaces.DTOs;
using Microsoft.AspNetCore.Mvc;

namespace ShoesStore_Project.RequestDTO
{
    public class ProductRequestDTO
    {
        public required string Title { get; set; }
        public required int CategoryID { get; set; }
        public required int BrandID { get; set; }
        public required decimal Price { get; set; }
        public required string SizeQuantities { get; set; }
        public List<IFormFile>? images { get; set; } // mutipale images

        
    }



}
