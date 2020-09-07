using System.Collections.Concurrent;
using System.Globalization;
using System.IO;
using System.Reflection;
using System.Text.Json;
using AntDesign.Locales;

namespace AntDesign
{
    public class LocaleProvider
    {
        public static Locale CurrentLocale => GetCurrentLocale();

        private static readonly ConcurrentDictionary<string, Locale> _localeCache = new ConcurrentDictionary<string, Locale>();
        private static Assembly _resourcesAssembly = typeof(LocaleProvider).Assembly;

        public static Locale GetCurrentLocale()
        {
            var current = CultureInfo.DefaultThreadCurrentCulture;
            return _localeCache.GetOrAdd(current.Name, key =>
            {
                string fileName = $"{_resourcesAssembly.GetName().Name}.locales.{key}.json";
                using var fileStream = _resourcesAssembly.GetManifestResourceStream(fileName);
                if (fileStream == null) return null;
                using var streamReader = new StreamReader(fileStream);
                var content = streamReader.ReadToEnd();

                return JsonSerializer.Deserialize<Locale>(content, new JsonSerializerOptions()
                {
                    PropertyNameCaseInsensitive = true,
                });
            });
        }
    }
}
