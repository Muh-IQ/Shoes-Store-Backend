using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Interfaces.Utiltiy
{
    public static class ImageType
    {
        public static HashSet<string> allowedTypes = new HashSet<string>()
        {
             "image/jpeg", "image/png", "image/gif", "image/webp"
        };
    }
}
