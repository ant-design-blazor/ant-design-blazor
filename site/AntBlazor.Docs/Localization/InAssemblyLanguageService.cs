using System;
using System.Globalization;

namespace AntBlazor.Docs.Localization
{
    public class InAssemblyLanguageService : ILanguageService
    {
        public InAssemblyLanguageService(CultureInfo culture)
        {
            SetLanguage(culture, true);
        }

        public InAssemblyLanguageService()
        {
            SetLanguage(CultureInfo.CurrentCulture, true);
        }

        public CultureInfo CurrentCulture { get; private set; }

        public event EventHandler<CultureInfo> LanguageChanged;

        private void SetLanguage(CultureInfo culture, bool isDefault)
        {
            if (CurrentCulture == null || !CurrentCulture.Equals(culture))
            {
                CurrentCulture = culture;
                LanguageChanged?.Invoke(this, culture);
            }
        }

        public void SetLanguage(CultureInfo culture)
        {
            if (CurrentCulture == null || !CurrentCulture.Equals(culture))
            {
                CurrentCulture = culture;
                LanguageChanged?.Invoke(this, culture);
            }
        }
    }
}