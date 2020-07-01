using System.Text.Json;

namespace AntDesign
{
    public class UploadFileItem
    {
        public string Id { get; set; }

        public string FileName { get; set; }

        public int Progress { get; set; }

        public string ObjectURL { get; set; }

        public string Url { get; set; }

        public string Response { get; set; }

        public UploadState State { get; set; }

        public long Size { get; set; }

        public string Ext { get; set; }

        public string Type { get; set; }

        public ResponseModel GetResponse<ResponseModel>(JsonSerializerOptions options = null) => JsonSerializer.Deserialize<ResponseModel>(this.Response, options);
    }
}
