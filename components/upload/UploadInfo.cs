// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

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
