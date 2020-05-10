using Microsoft.Extensions.CommandLineUtils;

namespace AntBlazor.Docs.Build.CLI
{
    public interface IAppCommandResolver
    {
        void Resolve(CommandLineApplication application);
    }
}