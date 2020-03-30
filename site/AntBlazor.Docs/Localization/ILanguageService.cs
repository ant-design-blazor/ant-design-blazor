using System;
using System.Globalization;

namespace AntBlazor.Docs.Localization
{
    public interface ILanguageService
    {
        CultureInfo CurrentCulture { get; }

        event EventHandler<CultureInfo> LanguageChanged;

        void SetLanguage(CultureInfo culture);
    }
}