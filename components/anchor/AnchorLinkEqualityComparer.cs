using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AntDesign
{
    public class AnchorLinkEqualityComparer : IEqualityComparer<AnchorLink>
    {
        public bool Equals(AnchorLink x, AnchorLink y)
        {
            return x.Href == y.Href;
        }

        public int GetHashCode(AnchorLink obj)
        {
            return obj.Href.GetHashCode();
        }
    }
}
