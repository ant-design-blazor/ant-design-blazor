using System;
using System.Diagnostics.CodeAnalysis;
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
#if NET5_0_OR_GREATER
        [return: DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.All)]
        [UnconditionalSuppressMessage("Trimming", "IL2026:Members annotated with 'RequiresUnreferencedCodeAttribute' require dynamic access otherwise can break functionality when trimming application code", Justification = "<Pending>")]
        public TResponseModel GetResponse<[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.All)] TResponseModel>(JsonSerializerOptions options = null)
#else
  public TResponseModel GetResponse<TResponseModel>(JsonSerializerOptions options = null)
#endif
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
