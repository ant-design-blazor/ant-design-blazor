// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Concurrent;
#if NET8_0_OR_GREATER
using System.Collections.Frozen;
#endif
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
    public partial class LocaleProvider
    {
        public static string DefaultLanguage { get; set; } = "en-US";

        private static readonly Assembly _resourcesAssembly = typeof(LocaleProvider).Assembly;

#if NET7_0_OR_GREATER
        [GeneratedRegex(@"^.*locales\.(.+)\.json")]
        private static partial Regex LocaleJsonRegex();
#else
        private static readonly Regex _localeJsonRegex = new(@"^.*locales\.(.+)\.json");
#endif

        private static readonly JsonSerializerOptions _localeJsonOpt = InitLocaleJsonOpt();

        private static JsonSerializerOptions InitLocaleJsonOpt()
        {
            var opt = new JsonSerializerOptions()
            {
#if NET7_0_OR_GREATER
                TypeInfoResolver = LocaleSourceGenerationContext.Default,
#endif
                PropertyNameCaseInsensitive = true,
            };
            opt.Converters.Add(new JsonStringEnumConverter(JsonNamingPolicy.CamelCase));
            return opt;
        }


        private static readonly ConcurrentDictionary<string, Locale> _localeCache = new();

#if NET8_0_OR_GREATER
        private static readonly FrozenDictionary<string, string> _availableResources = GetAvailableResources().ToFrozenDictionary();
#else
        private static readonly Dictionary<string, string> _availableResources = GetAvailableResources();
#endif

        private static Dictionary<string, string> GetAvailableResources()
        {
            var regex =
#if NET7_0_OR_GREATER
                LocaleJsonRegex();
#else
                _localeJsonRegex;
#endif
            var availableResources = _resourcesAssembly
                .GetManifestResourceNames()
                .Select(x => regex.Match(x))
                .Where(x => x.Success)
                .ToDictionary(x => x.Groups[1].Value, x => x.Value);

            foreach (var resource in availableResources.ToArray())
            {
                var parentCultureName = GetParentCultureName(resource.Key);
                while (parentCultureName != string.Empty)
                {
                    availableResources.TryAdd(parentCultureName, resource.Value);
                    parentCultureName = GetParentCultureName(parentCultureName);
                }
            }

            return availableResources;
        }

        public static Locale CurrentLocale => GetCurrentLocale();

        public static Locale GetCurrentLocale()
        {
            return GetLocale(CultureInfo.CurrentUICulture);
        }

        public static Locale GetLocale(string cultureName)
        {
            if (TryGetSpecifiedLocale(cultureName, out Locale locale)) return locale;
            // fallback to parent CultureInfo
            var parentCultureName = GetParentCultureName(cultureName);
            if (parentCultureName != string.Empty)
            {
                if (TryGetSpecifiedLocale(parentCultureName, out locale))
                {
                    AddClonedLocale(cultureName, ref locale);
                    return locale;
                }
                if (cultureName.Count(c => c == '-') == 2)
                {
                    var lang = cultureName[..cultureName.IndexOf('-')];
                    var region = cultureName[cultureName.LastIndexOf('-')..];
                    if (TryGetSpecifiedLocale(lang + region, out locale))
                    {
                        AddClonedLocale(cultureName, ref locale);
                        return locale;
                    }
                }
                parentCultureName = GetParentCultureName(parentCultureName);
                if (parentCultureName != string.Empty && TryGetSpecifiedLocale(parentCultureName, out locale))
                {
                    AddClonedLocale(cultureName, ref locale);
                    return locale;
                }
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

        public static Locale GetLocale(CultureInfo cultureInfo)
        {
            return GetLocale(cultureInfo.Name);
        }

        public static void SetLocale(string cultureName, Locale locale = null)
        {
            var culture = CultureInfo.GetCultureInfo(cultureName);

            if (culture != null)
            {
                CultureInfo.DefaultThreadCurrentUICulture = culture;
                CultureInfo.CurrentCulture = culture;
                CultureInfo.CurrentUICulture = culture;
            }

            if (locale != null)
            {
                locale.SetCultureInfo(cultureName);
                _localeCache.AddOrUpdate(cultureName, locale, (name, original) => locale);
            }
        }

        private static bool TryGetSpecifiedLocale(string cultureName, out Locale locale)
        {
            if (!_availableResources.ContainsKey(cultureName)) return _localeCache.TryGetValue(cultureName, out locale);
            locale = _localeCache.GetOrAdd(cultureName, static key =>
            {
                var fileName = _availableResources[key];
                using var fileStream = _resourcesAssembly.GetManifestResourceStream(fileName);
                if (fileStream == null) return null;
                using var streamReader = new StreamReader(fileStream);
                var content = streamReader.ReadToEnd();

                var result = JsonSerializer.Deserialize<Locale>(content, _localeJsonOpt);
                result.SetCultureInfo(key);
                return result;
            });
            return true;
        }

        private static void AddClonedLocale(string cultureName, ref Locale locale)
        {
            locale = locale with { };
            locale.SetCultureInfo(cultureName);
            _localeCache.TryAdd(cultureName, locale);
        }

        private static string GetParentCultureName(string name)
        {
            if (string.IsNullOrWhiteSpace(name)) return string.Empty;
            try
            {
                return CultureInfo.GetCultureInfo(name).Parent.Name;
            }
            catch (Exception)
            {
                return name.Contains('-') ? name[0..name.LastIndexOf('-')] : string.Empty;
            }
        }
    }

#if NET7_0_OR_GREATER
    [JsonSerializable(typeof(Locale))]
    internal partial class LocaleSourceGenerationContext : JsonSerializerContext
    {
    }
#endif
}
