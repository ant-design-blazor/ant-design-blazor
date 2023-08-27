using System;

namespace AntDesign
{
    public class InputDefaultValueOptions
    {
        public int? MaxLength { get; set; }
        private static Lazy<InputDefaultValueOptions> _instance;
        public static InputDefaultValueOptions Instance => _instance.Value;
        static InputDefaultValueOptions()
        {
            _instance = new Lazy<InputDefaultValueOptions>(new InputDefaultValueOptions());
        }
        private InputDefaultValueOptions() { }
    }
}
