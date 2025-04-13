using Interfaces.DTOs.ProductDTOs;
using Interfaces.Utiltiy.IMisc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Interfaces.Repository
{
    public interface IFilter
    {
        Task<IEnumerable<ProductCardDTO>> GetPagedFilteredProducts(IDirection Direction, IProperty property , int pageNumber, int pageSize);
    }
}
