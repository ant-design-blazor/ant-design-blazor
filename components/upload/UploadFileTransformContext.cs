using System;

namespace AntDesign
{
    /// <summary>Input provided to <see cref="Upload.TransformFile"/>.</summary>
    public sealed class UploadFileTransformContext
    {
        /// <summary>Metadata for the file being transformed.</summary>
        public UploadFileItem File { get; set; }

        /// <summary>File contents. Transform callbacks should keep this in memory only for small files.</summary>
        public byte[] Content { get; set; }

        /// <summary>The browser-reported media type.</summary>
        public string ContentType { get; set; }
    }

    /// <summary>The replacement bytes and optional metadata returned by <see cref="Upload.TransformFile"/>.</summary>
    public sealed class UploadFileTransformResult
    {
        /// <summary>Replacement file contents. Return null to upload the original file.</summary>
        public byte[] Content { get; set; }

        /// <summary>Optional replacement filename. Defaults to the original filename.</summary>
        public string FileName { get; set; }

        /// <summary>Optional replacement media type. Defaults to the original media type.</summary>
        public string ContentType { get; set; }
    }
}
