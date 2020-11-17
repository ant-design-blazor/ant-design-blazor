using System;

#pragma warning disable 1591 // Disable missing XML comment

namespace AntDesign.Select.Internal
{
    internal static class SelectModeExtensions
    {
        internal const string Tags = "tags";
        internal const string Multiple = "multiple";
        private const StringComparison Comparison = StringComparison.OrdinalIgnoreCase;

        internal static SelectMode ToSelectMode(this string mode)
        {
            if (Tags.Equals(mode, Comparison))
            {
                return SelectMode.Tags;
            }

            return Multiple.Equals(mode, Comparison) ? SelectMode.Multiple : SelectMode.Default;
        }
    }
}
