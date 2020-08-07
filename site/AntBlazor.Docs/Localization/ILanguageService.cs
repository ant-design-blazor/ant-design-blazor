using System;
using System.Globalization;

namespace AntDesign.Docs.Localization
{
    public interface ILanguageService
    {
        CultureInfo CurrentCulture { get; }

        Resources Resources { get; }

        event EventHandler<CultureInfo> LanguageChanged;

        void SetLanguage(CultureInfo culture);
    }
}
