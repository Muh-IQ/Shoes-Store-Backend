using Interfaces.DTOs;
using Interfaces.Repository;
using Interfaces.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business
{
    public class StorageService(IStorage repository) : IStorageService
    {
        public async Task<List<StorageDTO>> GetSizeQuantity(int ProductID)
        {
            return await repository.GetSizeQuantity(ProductID);
        }
    }
}
