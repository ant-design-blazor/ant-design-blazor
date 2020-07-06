using System;
using System.Collections.Generic;
using System.Text;

namespace AntDesign.TableModels
{
    public class SortType : EnumValue<SortType>
    {
        public static readonly SortType None = new SortType(null, 0);

        public static readonly SortType Ascending = new SortType("asc", 1);

        public static readonly SortType Descending = new SortType("desc", 2);

        public SortType(string name, int value) : base(name, value)
        {
        }

        public static SortType Parse(string typeName)
        {
            return (typeName) switch
            {
                "acs" => Ascending,
                "desc" => Descending,
                _ => None
            };
        }
    }
}
