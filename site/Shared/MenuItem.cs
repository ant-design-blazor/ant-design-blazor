using System;
using System.Collections.Generic;
using System.Text;

namespace AntBlazor.Docs.Services
{
    public class MenuItem
    {
        public string Title { get; set; }

        public string Type { get; set; }

        public string Url { get; set; }

        public string Prefix { get; set; }

        public bool Open { get; set; }

        public string Icon { get; set; }

        public bool Default { get; set; }

        public MenuItem[] Children { get; set; }
    }
}