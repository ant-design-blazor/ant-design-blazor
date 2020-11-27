using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AntDesign
{
    public enum AntDirectionVHIType
    {
        Vertical,
        Horizontal,
        Inline
    }

    public class AntDirectionVHType
    {
        public const string Horizontal = "horizontal";
        public const string Vertical = "vertical";
    }

    public class AntFourDirectionType
    {
        public const string Top = "top";
        public const string Bottom = "bottom";
        public const string Left = "left";
        public const string Right = "right";
    }
}
