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

        private static readonly IDictionary<string, string> _availableResources = _resourcesAssembly
               .GetManifestResourceNames()
            .Select(x => Regex.Match(x, @"^.*locales\.(.+)\.json"))
            .Where(x => x.Success)
            .ToDictionary(x => x.Groups[1].Value, x => x.Value);

        public static Locale GetCurrentLocale()
        {
            var currentCulture = CultureInfo.DefaultThreadCurrentUICulture?.Name;
            if (string.IsNullOrWhiteSpace(currentCulture) || !_availableResources.ContainsKey(currentCulture))
            {
                currentCulture = DefaultLanguage;
            }

            return GetLocale(currentCulture);
        }

        public static Locale GetLocale(string cultureName)
        {
            return _localeCache.GetOrAdd(cultureName, key =>
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
                return JsonSerializer.Deserialize<Locale>(content, serializerOptions);
            });
        }

        private static void SetLocale(string cultureName, Locale locale = null)
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
