using Microsoft.Extensions.CommandLineUtils;

namespace AntBlazor.Docs.Build.CLI
{
    public interface IAppCommand
    {
        string Name { get; }

        void Execute(CommandLineApplication command);
    }
}