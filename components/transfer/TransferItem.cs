using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AntBlazor
{
    public class TransferItem
    {
        public string Key { get; set; } = "";

        public string Title { get; set; } = "";

        public string Description { get; set; } = "";

        public bool Disabled { get; set; } = false;
    }
}
