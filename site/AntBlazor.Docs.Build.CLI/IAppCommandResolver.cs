using Microsoft.Extensions.CommandLineUtils;

namespace AntBlazor.Docs.Build
{
    public interface IAppCommandResolver
    {
        void Resolve(CommandLineApplication application);
    }
}