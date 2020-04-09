using System.Diagnostics;

namespace AntBlazor.Docs.Build.CLI.Extensions
{
    public static class ProcessExtensions
    {
        public static void Exec(this Process process, string command)
        {
            if (!process.HasExited)
                process.StandardInput.WriteLine(command);
        }
    }
}