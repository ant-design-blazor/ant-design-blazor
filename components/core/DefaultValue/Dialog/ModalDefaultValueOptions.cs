using System;
using Microsoft.AspNetCore.Components;
using OneOf;

namespace AntDesign
{
    public class ModalDefaultValueOptions
    {
        public OneOf<string, double>? Width { get; set; }
        public bool? MaskClosable { get; set; }
        public bool? DestroyOnClose { get; set; }
        public OneOf<string, RenderFragment>? Footer { get; set; }

        private static Lazy<ModalDefaultValueOptions> _instance;
        public static ModalDefaultValueOptions Instance => _instance.Value;
        static ModalDefaultValueOptions()
        {
            _instance = new Lazy<ModalDefaultValueOptions>(new ModalDefaultValueOptions());
        }
        private ModalDefaultValueOptions() { }
    }
}
