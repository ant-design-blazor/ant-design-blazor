using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AntBlazor
{
    public class CheckBoxOption
    {
        public string label { get; set; }

        public string value { get; set; }
        public bool @checked { get; set; }
        public bool disabled { get; set; }
    }
}