using Interfaces.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Interfaces.Repository
{
    public interface IStorage
    {
        Task<List<StorageDTO>> GetSizeQuantity(int ProductID);
    }
}
