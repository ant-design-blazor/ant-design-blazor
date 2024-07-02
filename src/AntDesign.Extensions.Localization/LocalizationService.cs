using System;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;
using Microsoft.Extensions.Localization;

namespace AntDesign.Extensions.Localization
{
    internal class LocalizationService : ILocalizationService
    {
        private CultureInfo? _currentCulture;

        public CultureInfo CurrentCulture => _currentCulture ?? CultureInfo.CurrentCulture;

        public event EventHandler<CultureInfo> LanguageChanged = default!;

        public void SetLanguage(CultureInfo culture)
        {
            if (!culture.Equals(CultureInfo.CurrentCulture))
            {
                CultureInfo.CurrentCulture = culture;
            }

            if (_currentCulture == null || !_currentCulture.Equals(culture))
            {
                _currentCulture = culture;
                CultureInfo.DefaultThreadCurrentCulture = culture;
                CultureInfo.DefaultThreadCurrentUICulture = culture;

                LanguageChanged?.Invoke(this, culture);
            }
        }
    }
}
