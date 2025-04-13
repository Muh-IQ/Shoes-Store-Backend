using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Interfaces.IDTOs
{
    public interface IProductDTO
    {
        int ID { get; set; }
        string Title { get; set; }
        decimal Price { get; set; }
    }

}
