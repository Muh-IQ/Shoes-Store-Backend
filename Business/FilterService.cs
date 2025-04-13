using Interfaces.DTOs.ProductDTOs;
using Interfaces.Repository;
using Interfaces.Services;
using Interfaces.Utiltiy.IMisc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business
{
    public class FilterService(IFilter filter) : IFilterService
    {
       
        public async Task<IEnumerable<ProductCardDTO>> GetPagedFilteredProducts(IDirection Direction, IProperty property, int pageNumber, int pageSize)
        {
            return await filter.GetPagedFilteredProducts(Direction, property, pageNumber, pageSize);
        }
    }
}
