using System;
using System.Collections.Generic;
using System.Text;

namespace AntDesign
{
    public class UploadFileItem
    {
        public string Id { get; set; }

        public string FileName { get; set; }

        public int Progress { get; set; }

        public string ObjectURL { get; set; }

        public string ResponseText { get; set; }

        public UploadState State { get; set; }

        public long Size { get; set; }

        public string Ext { get; set; }

    }
}
