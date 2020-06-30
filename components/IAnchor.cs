using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AntDesign
{
    public interface IAnchor
    {
        //List<AnchorLink> Links { get; }
        void Add(AnchorLink anchorLink);

        List<AnchorLink> FlatChildren();

        IAnchor GetParent();
    }
}
