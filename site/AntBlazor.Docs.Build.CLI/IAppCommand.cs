using Microsoft.Extensions.CommandLineUtils;

namespace AntBlazor.Docs.Build
{
    public interface IAppCommand
    {
        string Name { get; }

        void Execute(CommandLineApplication command);
    }
}