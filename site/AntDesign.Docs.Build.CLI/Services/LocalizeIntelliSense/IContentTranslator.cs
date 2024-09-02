using System.Globalization;

namespace IntelliSenseLocalizer.ThirdParty;

public interface IContentTranslator
{
    #region Public 方法

    Task<string> TranslateAsync(string content, CultureInfo from, CultureInfo to, CancellationToken cancellationToken = default);

    #endregion Public 方法
}
