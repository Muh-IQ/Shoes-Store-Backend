using Interfaces.Utiltiy.IMisc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Interfaces.Utiltiy
{
    public class Direction : IDirection
    {
        public DirectionType Select {  get; set; }

        public string Get()
        {
            switch (Select)
            {
                case DirectionType.ASC:
                    return "ASC";
                case DirectionType.DESC:
                    return "DESC";
                default:
                    return "None";
            }
            
        }

        public string Get(DirectionType Select)
        {
            switch (Select)
            {
                case DirectionType.ASC:
                    return "ASC";
                case DirectionType.DESC:
                    return "DESC";
                default:
                    return "None";
            }
        }
    }
}
