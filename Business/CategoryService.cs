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
    public class CategoryService(ICategory service) : ICategoryService
    {
        public Task<List<CategoryDTO>> GetCategories()
        {
            return service.GetCategories();
        }
    }
}
