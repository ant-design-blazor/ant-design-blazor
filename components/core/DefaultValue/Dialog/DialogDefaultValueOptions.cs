using System;
using Microsoft.AspNetCore.Components;
using OneOf;

namespace AntDesign
{
    public class DialogDefaultValueOptions
    {
        public bool? Centered { get; set; }
        public bool? Keyboard { get; set; }
        public bool? Mask { get; set; }
        public int? ZIndex { get; set; }
        public bool? Rtl { get; set; }
        public OneOf<string, RenderFragment>? OkText { get; set; }
        public OneOf<string, RenderFragment>? CancelText { get; set; }

        private static Lazy<DialogDefaultValueOptions> _instance;
        public static DialogDefaultValueOptions Instance => _instance.Value;
        static DialogDefaultValueOptions()
        {
            _instance = new Lazy<DialogDefaultValueOptions>(new DialogDefaultValueOptions());
        }
        private DialogDefaultValueOptions() { }
    }
}
