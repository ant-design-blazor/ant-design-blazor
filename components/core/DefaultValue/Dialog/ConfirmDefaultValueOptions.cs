using System;
using OneOf;

namespace AntDesign
{
    public class ConfirmDefaultValueOptions
    {
        public OneOf<string, double>? Width { get; set; }
        public bool? MaskClosable { get;set; }

        private static Lazy<ConfirmDefaultValueOptions> _instance;
        public static ConfirmDefaultValueOptions Instance => _instance.Value;
        static ConfirmDefaultValueOptions()
        {
            _instance = new Lazy<ConfirmDefaultValueOptions>(new ConfirmDefaultValueOptions());
        }
        private ConfirmDefaultValueOptions() { }
    }
}
