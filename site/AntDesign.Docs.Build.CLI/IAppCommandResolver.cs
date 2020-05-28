using Microsoft.Extensions.CommandLineUtils;

namespace AntDesign.Docs.Build.CLI
{
    public interface IAppCommandResolver
    {
        void Resolve(CommandLineApplication application);
    }
}