// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Collections.Generic;

namespace AntDesign
{
    public class UploadInfo
    {
        public UploadFileItem File { get; set; }

        public List<UploadFileItem> FileList { get; set; }
    }
}
