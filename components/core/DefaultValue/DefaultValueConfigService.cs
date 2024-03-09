using Microsoft.Extensions.Options;

namespace AntDesign
{
    public class DefaultValueConfigService
    {
        public DefaultValueOptions Options { get; }
        public DefaultValueConfigService(IOptions<DefaultValueOptions> options)
        {
            Options = options.Value;
        }
    }
}
