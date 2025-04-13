using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Interfaces.Utiltiy.IMisc
{
    public enum PropertyType
    {
        Rate = 1,
        Price = 2
    }
    public interface IProperty
    {
        PropertyType Select { get; set; }
        string Get();
        string Get(PropertyType Selected);
    }
}
