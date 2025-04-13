using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Interfaces.DTOs
{
    public class CategoryDTO
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public CategoryDTO(int id,string name)
        {
            this.ID = id;
            this.Name = name;       
        }
    }
}
