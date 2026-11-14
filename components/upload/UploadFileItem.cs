// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using System.Text.Json;
using AntDesign.Core.Helpers;

namespace AntDesign
{
    public class UploadFileItem
    {
        /// <summary>
        /// GUID to identify the file
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        /// The file name
        /// </summary>
        public string FileName { get; set; }

        /// <summary>
        /// Percentage uploaded
        /// </summary>
        public int Percent { get; set; }

        /// <summary>
        /// Object URL for the file
        /// </summary>
        public string ObjectURL { get; set; }

        /// <summary>
        /// URL for the actual file
        /// </summary>
        public string Url { get; set; }

        /// <summary>
        /// Response from the server
        /// </summary>
        public string Response { get; set; }

        /// <summary>
        /// State of the upload
        /// </summary>
        public UploadState State { get; set; }

        /// <summary>
        /// Size of the file
        /// </summary>
        public long Size { get; set; }

        /// <summary>
        /// Extension for the file
        /// </summary>
        public string Ext { get; set; }

        /// <summary>
        /// Type of file
        /// </summary>
        public string Type { get; set; }

        public TResponseModel GetResponse<TResponseModel>(JsonSerializerOptions options = null)
        {
            return JsonSerializer.Deserialize<TResponseModel>(this.Response, options: options ?? JsonSerializerHelper.DefaultOptions);
        }

        /// <summary>
        /// File extensions that are considered images
        /// </summary>
        public static HashSet<string> ImageExtensions { get; set; } = new(StringComparer.InvariantCultureIgnoreCase) { ".jpg", ".png", ".gif", ".ico", ".jfif", ".jpeg", ".bmp", ".tga", ".svg", ".tif", ".webp" };

        /// <summary>
        /// Check if the file is a picture. See <see cref="ImageExtensions"/> for the extensions which will cause this to return true.
        /// </summary>
        /// <returns>Boolean indicating if the file is a picture or not.</returns>
        public bool IsPicture()
        {
            if (string.IsNullOrEmpty(Ext))
            {
                var lastIndex = FileName.LastIndexOf('.');
                if (lastIndex < 0) return false;
                Ext = FileName[lastIndex..];
            }
            return ImageExtensions.Contains(Ext);
        }
    }
}
