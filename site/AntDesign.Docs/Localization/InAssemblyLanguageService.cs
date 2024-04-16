using System;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;

namespace AntDesign.Docs.Localization
{
    public class InAssemblyLanguageService : ILanguageService
    {
        private readonly Assembly _resourcesAssembly;

        public InAssemblyLanguageService(Assembly assembly, CultureInfo culture)
        {
            _resourcesAssembly = assembly;
        }

        public InAssemblyLanguageService(Assembly assembly)
        {
            _resourcesAssembly = assembly;
        }

        public CultureInfo CurrentCulture { get; private set; }

        public Resources Resources { get; private set; }

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
