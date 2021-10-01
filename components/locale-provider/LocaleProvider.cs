using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Text.RegularExpressions;
using AntDesign.Locales;

namespace AntDesign
{
    public class LocaleProvider
    {
        public static Locale CurrentLocale => GetCurrentLocale();

        public static string DefaultLanguage = "en-US";

        private static readonly ConcurrentDictionary<string, Locale> _localeCache = new ConcurrentDictionary<string, Locale>();
        private static Assembly _resourcesAssembly = typeof(LocaleProvider).Assembly;

        private static readonly IDictionary<string, string> _availableResources = GetAvailableResources();

        private static IDictionary<string, string> GetAvailableResources()
        {
            var availableResources = _resourcesAssembly
                .GetManifestResourceNames()
                .Select(x => Regex.Match(x, @"^.*locales\.(.+)\.json"))
                .Where(x => x.Success)
                .ToDictionary(x => x.Groups[1].Value, x => x.Value);
            foreach (var resource in availableResources.ToArray())
            {
                var cultureInfo = CultureInfo.GetCultureInfo(resource.Key);
                var parentCultureName = cultureInfo.Parent?.Name;
                if (parentCultureName != null && !availableResources.ContainsKey(parentCultureName))
                {
                    availableResources.Add(parentCultureName, resource.Value);
                }
            }
            return availableResources;
        }

        public static Locale GetCurrentLocale()
        {
            return GetLocale(CultureInfo.CurrentUICulture);
        }

        public static Locale GetLocale(string cultureName)
        {
            return GetLocale(CultureInfo.GetCultureInfo(cultureName));
        }

        public static Locale GetLocale(CultureInfo cultureInfo)
        {
            var cultureName = cultureInfo.Name;
            if (TryGetSpecifiedLocale(cultureName, out Locale locale)) return locale;
            //fallback to parent CultureInfo if not found
            cultureName = cultureInfo.Parent?.Name;
            if (cultureName != null && TryGetSpecifiedLocale(cultureName, out locale)) return locale;
            //fallback to default language if not found
            if (TryGetSpecifiedLocale(DefaultLanguage, out locale)) return locale;
            //fallback to 'en-US' if not found
            TryGetSpecifiedLocale("en-US", out locale);
            return locale;
        }

        public static bool TryGetSpecifiedLocale(string cultureName, out Locale locale)
        {
            if (!_availableResources.ContainsKey(cultureName)) return _localeCache.TryGetValue(cultureName, out locale);
            locale = _localeCache.GetOrAdd(cultureName, key =>
            {
                string fileName = _availableResources[key];
                using var fileStream = _resourcesAssembly.GetManifestResourceStream(fileName);
                if (fileStream == null) return null;
                using var streamReader = new StreamReader(fileStream);
                var content = streamReader.ReadToEnd();

                var serializerOptions = new JsonSerializerOptions()
                {
                    PropertyNameCaseInsensitive = true,
                };
                serializerOptions.Converters.Add(new JsonStringEnumConverter(JsonNamingPolicy.CamelCase));
                var result = JsonSerializer.Deserialize<Locale>(content, serializerOptions);
                result.LocaleName = cultureName;
                return result;
            });
            return true;
        }

        public static void SetLocale(string cultureName, Locale locale = null)
        {
            var culture = CultureInfo.GetCultureInfo(cultureName);

            if (culture != null)
            {
                CultureInfo.DefaultThreadCurrentUICulture = culture;
            }

            if (locale != null)
            {
                _localeCache.AddOrUpdate(cultureName, locale, (name, original) => locale);
            }
        }
    }
}
