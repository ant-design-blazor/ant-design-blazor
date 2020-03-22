namespace AntBlazor
{
    public class StyleHelper
    {
        //fix the user set 100% or xxxVH etc..
        public static string ToCssPixel(string value) => int.TryParse(value, out var _) ? $"{value}px" : value;

        public static string ToCssPixel(int value) => $"{value}px";
    }
}