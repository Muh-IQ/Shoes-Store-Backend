using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Interfaces.DTOs
{
    public class BrandDTO
    {
        public int ID { get; set; }
        [Required(ErrorMessage = "Brand name is required.")]
        public string Name { get; set; }
        public BrandDTO(int id,string name)
        {
            this.ID = id;
            this.Name = name;
        }
    }
}
