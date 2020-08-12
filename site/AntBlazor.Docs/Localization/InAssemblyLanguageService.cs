using System;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text.Json;
using System.Text.RegularExpressions;

namespace AntDesign.Docs.Localization
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
            var availableResources = _resourcesAssembly
                .GetManifestResourceNames()
                .Select(x => Regex.Match(x, @"^.*Resources\.(.+)\.json"))
                .Where(x => x.Success)
                .Select(x => (CultureName: x.Groups[1].Value, ResourceName: x.Value))
                .ToList();

            var (_, resourceName) = availableResources.FirstOrDefault(x => x.CultureName.Equals(culture.Name, StringComparison.OrdinalIgnoreCase));

            if (resourceName == null)
            {
                (_, resourceName) = availableResources.FirstOrDefault(x => x.CultureName.Equals("en-US", StringComparison.OrdinalIgnoreCase));
                culture = CultureInfo.GetCultureInfo("en-US");
            }

            CultureInfo.CurrentCulture = culture;
            CurrentCulture = culture;
            Resources = GetKeysFromCulture(culture.Name, resourceName);

            if (Resources == null)
                throw new FileNotFoundException($"There is no language files existing in the Resource folder within '{_resourcesAssembly.GetName().Name}' assembly");
        }

        public void SetLanguage(CultureInfo culture)
        {
            if (!culture.Equals(CultureInfo.CurrentCulture))
            {
                CultureInfo.CurrentCulture = culture;
            }

            if (CurrentCulture == null || !CurrentCulture.Equals(culture))
            {
                CurrentCulture = culture;
                string fileName = $"{_resourcesAssembly.GetName().Name}.Resources.{culture.Name}.json";

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
                var content = streamReader.ReadToEnd();
                return new Resources(content);
            }
            catch (System.Exception e)
            {
                return null;
            }
        }
    }
}
