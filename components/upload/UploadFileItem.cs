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

        public bool IsPicture()
        {
            string[] imageTypes = new[] { ".jpg", ".png", ".gif", ".ico",".jfif",".jpeg",".bmp",".tga",".svg",".tif" };
            Ext = FileName.Substring(FileName.LastIndexOf('.'));
            return imageTypes.Any(imageType => imageType.Equals(Ext, StringComparison.InvariantCultureIgnoreCase));
        }
    }
}
