using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Interfaces.Utiltiy.IMisc.IDirection;

namespace Interfaces.Utiltiy.IMisc
{
    public enum DirectionType
    {
        ASC = 1,
        DESC = 2
    }
    public interface IDirection
    {
        DirectionType Select {  get; set; }
        string Get();
        string Get(DirectionType Select);
    }
}
