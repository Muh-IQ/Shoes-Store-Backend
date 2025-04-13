using Interfaces.Utiltiy.IMisc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Interfaces.Utiltiy
{
    public class Property : IProperty
    {
        public PropertyType Select { get ; set ; }

        public string Get()
        {
            switch (Select)
            {
                case PropertyType.Rate:
                    return "Rate";
                case PropertyType.Price:
                    return "Price";
                default:
                    return "None";
            }
        }

        public string Get(PropertyType Selected)
        {
            switch (Selected)
            {
                case PropertyType.Rate:
                    return "Rate";
                case PropertyType.Price:
                    return "Price";
                default:
                    return "None";
            }
        }
    }
}
