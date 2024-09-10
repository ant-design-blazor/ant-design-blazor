using System.Globalization;
using System.Threading;
using System.Threading.Tasks;

namespace IntelliSenseLocalizer.ThirdParty;

public interface IContentTranslator
{

    Task<string> TranslateAsync(string content, CultureInfo from, CultureInfo to, CancellationToken cancellationToken = default);
}
