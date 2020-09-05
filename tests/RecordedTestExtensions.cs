using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text;
using System.Text.RegularExpressions;
using Bunit;
using Microsoft.AspNetCore.Components;
using Xunit;
using Xunit.Sdk;

namespace AntDesign.Tests
{
    public static class RecordedTestExtensions
    {
        private const string Style =
            "<link href=\"../../components/wwwroot/css/ant-design-blazor.css\" rel=\"stylesheet\">\n";

        [MethodImpl(MethodImplOptions.NoInlining)]
        public static void RecordedMarkupMatches<T>(this IRenderedComponent<T> component) where T : notnull, IComponent
        {
            static string Cleanup(string value)
            {
                value = Regex.Replace(value, "id=\"ant-blazor-.+?\"", "id:ignore");
                value = Regex.Replace(value, "blazor:elementreference=\".+?\"", "blazor:elementreference:ignore");

                return value;
            }


            var caller = new StackTrace().GetFrame(1)?.GetMethod();
            RecordedMarkupMatches(
                component.Markup,
                caller,
                expected => component.MarkupMatches(expected),
                Cleanup
            );
        }

        public static void RecordedMarkupMatches(string markup)
        {
            var caller = new StackTrace().GetFrame(1)?.GetMethod();
            RecordedMarkupMatches(
                markup,
                caller,
                expected => Assert.Equal(markup, expected),
                m => m
            );
        }

        private static void RecordedMarkupMatches(string markup, MethodBase caller, Action<string> assert,
            Func<string, string> transform)
        {
            if (caller == null)
                throw new XunitException("Cannot find caller from StackTrace.");

            if (caller.ReflectedType == null)
                throw new XunitException("Cannot access ReflectedType for the method.");

            // Here we make an assumption that project is not following an unconventional directory structure.
            var expectedPath = $"{Path.DirectorySeparatorChar}bin{Path.DirectorySeparatorChar}";
            var parts = caller.ReflectedType.Assembly.Location.Split(expectedPath);
            if (parts.Length == 1)
                throw new XunitException($"Path does not include ${expectedPath}");

            var recordedTestsPath = Path.Combine(parts[0], "$Recorded");

            if (!Directory.Exists(recordedTestsPath))
                Directory.CreateDirectory(recordedTestsPath);

            var sanitisedFileName = Path.GetInvalidFileNameChars()
                .Aggregate(new StringBuilder($"{caller.ReflectedType}{caller.Name}.html"),
                    (builder, c) => builder.Replace(c, '_'))
                .ToString();

            var testFile = Path.Combine(recordedTestsPath, sanitisedFileName);

            if (File.Exists(testFile))
            {
                var expected = File.ReadAllText(testFile);
                assert(expected.Replace(Style, ""));
            }
            else
            {
                File.WriteAllText(testFile, Style + transform(markup));
                throw new XunitException(
                    "Test file for comparison was not found, so a new one was created. Please review the file before re-running the test.");
            }
        }
    }
}
