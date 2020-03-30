using System;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;

namespace AntBlazor.Docs.Localization
{
    public class InAssemblyLanguageService : ILanguageService
    {
        private readonly Assembly _resourcesAssembly;

        public InAssemblyLanguageService(Assembly assembly, CultureInfo culture)
        {
            _resourcesAssembly = assembly;
            SetDefaultLanguage(culture);
        }

        public InAssemblyLanguageService(Assembly assembly)
        {
            _resourcesAssembly = assembly;
            SetDefaultLanguage(CultureInfo.CurrentCulture);
        }

        public CultureInfo CurrentCulture { get; private set; }

        public Resources Resources { get; private set; }

        public event EventHandler<CultureInfo> LanguageChanged;

        private void SetDefaultLanguage(CultureInfo culture)
        {
            CurrentCulture = culture;

            string[] languageFileNames = _resourcesAssembly.GetManifestResourceNames().Where(s => s.Contains("Resources") && s.Contains(".yml") && s.Contains("-")).ToArray();

            Resources = GetKeysFromCulture(culture.Name, languageFileNames.SingleOrDefault(n => n.Contains($"{culture.Name}.yml")));

            if (Resources == null)
            {
                string language = culture.Name.Split('-')[0];
                Resources = GetKeysFromCulture(culture.Name, languageFileNames.FirstOrDefault(n => n.Contains(language)));
            }

            if (Resources == null && culture.Name != "en-US")
                Resources = GetKeysFromCulture("en-US", languageFileNames.SingleOrDefault(n => n.Contains($"en-US.yml")));

            if (Resources == null)
                Resources = GetKeysFromCulture("en-US", languageFileNames.FirstOrDefault());

            if (Resources == null)
                throw new FileNotFoundException($"There is no language files existing in the Resource folder within '{_resourcesAssembly.GetName().Name}' assembly");
        }

        public void SetLanguage(CultureInfo culture)
        {
            if (CurrentCulture == null || !CurrentCulture.Equals(culture))
            {
                CurrentCulture = culture;
                string fileName = $"{_resourcesAssembly.GetName().Name}.Resources.{culture.Name}.yml";

                Resources = GetKeysFromCulture(culture.Name, fileName);

                if (Resources == null)
                    throw new FileNotFoundException($"There is no language files for '{culture.Name}' existing in the Resources folder within '{_resourcesAssembly.GetName().Name}' assembly");
                LanguageChanged?.Invoke(this, culture);
            }
        }

        private Resources GetKeysFromCulture(string culture, string fileName)
        {
            try
            {
                // Read the file
                using var fileStream = _resourcesAssembly.GetManifestResourceStream(fileName);
                if (fileStream == null) return null;
                using var streamReader = new StreamReader(fileStream);
                return new Resources(streamReader.ReadToEnd());
            }
            catch (System.Exception)
            {
                return null;
            }
        }
    }
}