using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

namespace AntDesign.Docs.Build.CLI.Utils
{
    /// <summary>
    /// Creates convenience static classes that can be used as enums
    /// Example: Icon component needs a string as its Type.
    /// This creates a static class that can be used as `IconType.Fill.AccountBook`
    /// Wich produces the string "account-book"
    /// </summary>
    internal static class StaticClassGenerator
    {
        private const string OpenBracket = "{\n";
        private const string CloseBracket = "}\n";
        private const string Warning = "//This is a generated file. Any changes to it will be discarded in the next run\n";
        private const string NameSpace = "namespace AntDesign";

        //IconType specific
        private const string IconTypeFileName = "IconType.cs";

        private const string IconTypeClassName = "IconType";

        //Generates the static class IconType.cs
        internal static void GenerateIconTypeFile(string path)
        {
            var fileContent = GetIconTypeStaticClassString();
            GenerateFile(fileContent, path, IconTypeFileName);
        }

        //Gets the text of the static class IconType
        //This class has 3 classes, one for each theme Fill, Outline, Twotone
        internal static string GetIconTypeStaticClassString()
        {
            //creates one string representating a c# class by icon theme
            var iconsByTheme = new string[0]; //IconStore.GetAllIconNames()   .Select(item => GetClassString(item.Key, item.Value));

            //all icons classes joined
            var iconsByThemeAggregated = iconsByTheme.Aggregate((a, b) => a + b);

            var result = NameSpace
                + OpenBracket
                    + $"public static class {IconTypeClassName}\n"
                     + OpenBracket
                        + $"{iconsByThemeAggregated}"
                     + CloseBracket
                + CloseBracket;

            //Add warning on top
            return Warning + result;
        }

        //Gets the aggregated text for the given properties collection
        //produces `public static string PropertyName => "property-name";` for each property
        //and then concatenate in a string
        internal static string GetPropertiesString(IEnumerable<string> values)
        {
            var properties = values
                .Select(v => GetLine(2, $"public static string {ToCamelCase(v)} => \"{v}\";"))
                .Aggregate((a, b) => a + b);

            return properties;
        }

        //Gets the content for a static class with the given collection of static string properties
        internal static string GetClassString(string className, IEnumerable<string> properties)
        {
            var classString = GetLine(1, $"public static class {ToCamelCase(className)}")
                + GetLine(1, OpenBracket)
                + GetPropertiesString(properties)
                + GetLine(1, CloseBracket);

            return classString;
        }

        //produces a line of text with identation and new line at the end
        private static string GetLine(int indentation, string text)
        {
            return $"{Indentation(indentation)}{text}\n";
        }

        //converts a string to CamelCase
        //ex.: home-page =>HomePage
        private static string ToCamelCase(string word)
        {
            if (string.IsNullOrEmpty(word)) return word;

            var result = Regex.Replace(word.ToLowerInvariant()
                , @"\b[a-z]"
                , m => m.Value.ToUpperInvariant())
                .Replace("-", "");

            return result;
        }

        //Creates `n` indent characters
        private static string Indentation(int level = 1, int indentCharsPerLevel = 4, string indentChar = " ")
        {
            var indentation = "";
            for (var i = 0; i < level * indentCharsPerLevel; i++)
            {
                indentation += indentChar;
            }
            return indentation;
        }

        //Creates a file with the given text, path and fileName
        internal static void GenerateFile(string text, string path, string fileName)
        {
            using var outputFile = new StreamWriter(Path.Combine(path, fileName));
            outputFile.WriteLine(text);
        }
    }
}
