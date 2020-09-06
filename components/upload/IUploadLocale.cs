using System;
using System.Collections.Generic;
using System.Text;

namespace AntDesign
{
    public interface IUploadLocale
    {
        public string Uploading { get; }

        public string RemoveFile { get; }

        public string UploadError { get; }

        public string PreviewFile { get; }

        public string DownloadFile { get; }
    }
}
