using System;
using System.Reflection;
using Microsoft.Extensions.CommandLineUtils;

namespace AntDesign.Docs.Build.CLI.Extensions
{
    internal static class CommandLineApplicationExtensions
    {
        public static CommandOption HelpOption(this CommandLineApplication app)
            => app.HelpOption("-?|-h|--help");

        public static CommandOption VerboseOption(this CommandLineApplication app)
            => app.Option("-v|--verbose", "Show verbose output", CommandOptionType.NoValue, inherited: true);

        public static void OnExecute(this CommandLineApplication app, Action action)
            => app.OnExecute(() =>
            {
                action();
                return 0;
            });

        public static void VersionOptionFromAssemblyAttributes(this CommandLineApplication app, Assembly assembly)
            => app.VersionOption("--version", GetInformationalVersion(assembly));

        private static string GetInformationalVersion(Assembly assembly)
        {
            var attribute = assembly.GetCustomAttribute<AssemblyInformationalVersionAttribute>();

            var versionAttribute = attribute == null
                ? assembly.GetName().Version.ToString()
                : attribute.InformationalVersion;

            return versionAttribute;
        }
    }
}