using System.Collections.Generic;

namespace AntDesign
{
    public interface IAnchor
    {
        //List<AnchorLink> Links { get; }
        void Add(AnchorLink anchorLink);
        void Remove(AnchorLink anchorLink);

        void Clear();

        List<AnchorLink> FlatChildren();
    }
}
