using System;

namespace AntDesign
{
    public static class SelectModeExtensions
    {
        public const string Tags = "tags";
        public const string Multiple = "multiple";
        private const StringComparison Comparison = StringComparison.OrdinalIgnoreCase;

        public static SelectMode ToSelectMode(this string mode)
        {
            if (Tags.Equals(mode, Comparison))
            {
                return SelectMode.Tags;
            }

            return Multiple.Equals(mode, Comparison) ? SelectMode.Multiple : SelectMode.Default;
        }
    }
}
