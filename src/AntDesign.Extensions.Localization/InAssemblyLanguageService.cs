using System;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;
using Microsoft.Extensions.Localization;

namespace AntDesign.Docs.Localization
{
    public class InAssemblyLanguageService : ILanguageService
    {
        public CultureInfo CurrentCulture { get; private set; } = CultureInfo.CurrentCulture;

        public event EventHandler<CultureInfo> LanguageChanged;

        public void SetLanguage(CultureInfo culture)
        {
            if (!culture.Equals(CultureInfo.CurrentCulture))
            {
                CultureInfo.CurrentCulture = culture;
            }

            if (CurrentCulture == null || !CurrentCulture.Equals(culture))
            {
                CurrentCulture = culture;
                CultureInfo.DefaultThreadCurrentCulture = culture;
                CultureInfo.DefaultThreadCurrentUICulture = culture;

                LanguageChanged?.Invoke(this, culture);
            }
        }
    }
}
