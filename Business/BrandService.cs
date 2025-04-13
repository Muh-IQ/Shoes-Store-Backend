using Interfaces.DTOs;
using Interfaces.Repository;
using Interfaces.Services;

namespace Business
{
    public class BrandService(IBrand service) : IBrandService
    {
        public async Task<List<BrandDTO>> GetBrands()
        {
            return await service.GetBrands();
        }
        public async Task<int> AddNewBrand(string brandName)
        {
            return await service.AddNewBrand(brandName);
        }
    }
}
