using System;
using System.Collections.Generic;
using System.Text;

namespace AntDesign.Docs
{
    public class DocComponent
    {
        public string Title { get; set; }

        public string SubTitle { get; set; }

        public string Type { get; set; }

        public string Desc { get; set; }
        public List<DocItem> DemoList { get; set; }
    }

    public class DocItem
    {
        public decimal Order { get; set; }

        public string Name { get; set; }

        public string Title { get; set; }

        public string Description { get; set; }

        public string Code { get; set; }

        public string Type { get; set; }

        public string Style { get; set; }

        public int? Iframe { get; set; }

        public bool Debug { get; set; }
    }
}
