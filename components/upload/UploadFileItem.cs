// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Linq;
using System.Text.Json;

namespace AntDesign
{
    public class UploadFileItem
    {
        public string Id { get; set; }

        public string FileName { get; set; }

        public int Percent { get; set; }

        public string ObjectURL { get; set; }

        public string Url { get; set; }

        public string Response { get; set; }

        public UploadState State { get; set; }

        public long Size { get; set; }

        public string Ext { get; set; }

        public string Type { get; set; }

        public TResponseModel GetResponse<TResponseModel>(JsonSerializerOptions options = null)
        {
            if (options == null)
            {
                //Provide default configuration: ignore case
                options = new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true,
                };
            }
            return JsonSerializer.Deserialize<TResponseModel>(this.Response, options);
        }

        public static string[] ImageExtensions { get; set; } = new[] { ".jpg", ".png", ".gif", ".ico", ".jfif", ".jpeg", ".bmp", ".tga", ".svg", ".tif", ".webp" };

        public bool IsPicture()
        {
            if (string.IsNullOrEmpty(Ext))
            {
                var lastIndex = FileName.LastIndexOf('.');
                if (lastIndex < 0) return false;
                Ext = FileName[lastIndex..];
            }
            return ImageExtensions.Any(imageExt => imageExt.Equals(Ext, StringComparison.InvariantCultureIgnoreCase));
        }
    }
}
