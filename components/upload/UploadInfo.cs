using System.Collections.Generic;

namespace AntDesign
{
    public class UploadInfo
    {
        /// <summary>
        /// The file for the current operation
        /// </summary>
        public UploadFileItem File { get; set; }

        /// <summary>
        /// The current list of files
        /// </summary>
        public List<UploadFileItem> FileList { get; set; }
    }
}
