using Microsoft.Extensions.CommandLineUtils;

namespace AntDesign.Docs.Build.CLI
{
    public interface IAppCommand
    {
        string Name { get; }

        void Execute(CommandLineApplication command);
    }
}