using System;
using System.Globalization;
using Microsoft.Extensions.Localization;

namespace AntDesign.Docs.Localization
{
    public interface ILanguageService
    {
        CultureInfo CurrentCulture { get; }

        event EventHandler<CultureInfo> LanguageChanged;

        void SetLanguage(CultureInfo culture);
    }
}
