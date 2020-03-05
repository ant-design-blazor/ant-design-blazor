namespace AntBlazor
{
    public class StyleHelper
    {
        public static string ToCssPixel(string value)
        {
            if (value.EndsWith("px"))
            {
                return value;
            }

            return $"{value}px";
        }
    }
}