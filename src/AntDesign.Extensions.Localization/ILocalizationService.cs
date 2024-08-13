using System;
using System.Globalization;

namespace AntDesign.Extensions.Localization
{
    /// <summary>
    /// Interactive localization service, can use in all interactive apps.
    /// <list type="bullet">
    /// <item>Call <see cref="SetLanguage(CultureInfo)"/> to change the language.</item>
    /// <item>The <see cref="CurrentCulture"/> is the current culture.</item>
    /// <item>The <see cref="LanguageChanged"/> is the event that fires when the language has changed.</item>
    /// </list>
    /// </summary>
    public interface ILocalizationService
    {
        /// <summary>
        /// Get the current culture
        /// </summary>d
        CultureInfo CurrentCulture { get; }

        /// <summary>
        /// An event that fires when the language has changed.
        /// </summary>
        event EventHandler<CultureInfo> LanguageChanged;

        /// <summary>
        /// Invoke the <see cref="InteractiveStringLocalizer"/> to change the culture.
        /// </summary>
        /// <param name="culture"></param>
        void SetLanguage(CultureInfo culture);
    }
}
