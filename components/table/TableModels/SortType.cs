using System;
using System.Collections.Generic;
using System.Text;

namespace AntDesign.TableModels
{
    public class SortType : EnumValue<SortType>
    {
        public static readonly SortType None = new SortType(null, 0);

        public static readonly SortType Ascending = new SortType("ascend", 1);

        public static readonly SortType Descending = new SortType("descend", 2);

        public SortType(string name, int value) : base(name, value)
        {
        }

        public static SortType Parse(string typeName)
        {
            return (typeName) switch
            {
                "ascend" => Ascending,
                "descend" => Descending,
                _ => None
            };
        }
    }
}
