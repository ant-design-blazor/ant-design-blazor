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
                var parentCulture = CultureInfo.GetCultureInfo(resource.Key).Parent;
                var parentCultureName = parentCulture.Name;

                while (parentCultureName != string.Empty)
                {
                    if (!availableResources.ContainsKey(parentCultureName))
                    {
                        availableResources.Add(parentCultureName, resource.Value);
                    }
                    parentCulture = parentCulture.Parent;
                    parentCultureName = parentCulture.Name;
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
            // fallback to parent CultureInfo
            var parentCulture = cultureInfo.Parent;
            if (parentCulture.Name != string.Empty)
            {
                locale = GetLocale(parentCulture);
                AddClonedLocale(cultureName, ref locale);
                return locale;
            }
            // fallback to default language
            if (TryGetSpecifiedLocale(DefaultLanguage, out locale))
            {
                AddClonedLocale(cultureName, ref locale);
                return locale;
            }
            // fallback to 'en-US'
            TryGetSpecifiedLocale("en-US", out locale);
            AddClonedLocale(cultureName, ref locale);
            return locale;
        }

        private static void AddClonedLocale(string cultureName, ref Locale locale)
        {
            locale = JsonSerializer.Deserialize<Locale>(
                JsonSerializer.Serialize(locale, new JsonSerializerOptions { IgnoreReadOnlyProperties = true }));
            locale.LocaleName = cultureName;
            _localeCache.TryAdd(cultureName, locale);
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
                result.LocaleName = key;
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
