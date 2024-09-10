// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Encodings.Web;
using System.Text.Json;
using System.Text.Unicode;
using System.Threading.Tasks;

namespace AntDesign.Docs.Build.CLI.Services.Translation
{
    public class TranslateCache : ITranslate
    {
        private readonly ITranslate _innerService;

        private readonly static JsonSerializerOptions _serializerOptions = new JsonSerializerOptions
        {
            WriteIndented = true,
            Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping
        };

        public TranslateCache(ITranslate innerService)
        {
            _innerService = innerService;

            var encoding = new TextEncoderSettings();
            encoding.AllowRange(UnicodeRanges.All);

            var filePath = GetPathToKnownTranslationsFile();

            if (File.Exists(filePath))
            {
                var file = File.ReadAllText(filePath, Encoding.UTF8);
                KnownChineseTranslations = JsonSerializer.Deserialize<Dictionary<string, Translation>>(file, _serializerOptions);
            }
            else
            {
                KnownChineseTranslations = new();
            }
        }

        private Dictionary<string, Translation> KnownChineseTranslations { get; set; }

        private Dictionary<string, Dictionary<string, Translation>> ComponentKnownTranslations { get; set; }

        public async Task BackupTranslations(string lang, bool onlyKeepUsed = true)
        {
            var translations = KnownChineseTranslations;

            if (onlyKeepUsed)
            {
                translations = translations
                    .Where(x => x.Value.Used)
                    .ToDictionary(x => x.Key, x => x.Value);
            }

            await File.WriteAllTextAsync(GetPathToKnownTranslationsFile(), SerializeTranslations(translations), Encoding.UTF8);

            await File.WriteAllTextAsync(GetPathToKnownTranslationsFile(lang), JsonSerializer.Serialize(ComponentKnownTranslations, _serializerOptions), Encoding.UTF8);
        }

        public async Task<string> TranslateText(string component, string text, string to, string from = "auto")
        {
            ComponentKnownTranslations ??= GetComponentKnownTranslations(to);

            if (ComponentKnownTranslations.TryGetValue(component, out var componentTranslations) && componentTranslations.TryGetValue(text, out var translation))
            {
                translation.Used = true;
                return translation.Translated;
            }

            var newTranslation = await TranslateText(text, to, from);

            if (!ComponentKnownTranslations.ContainsKey(component))
            {
                ComponentKnownTranslations[component] = new();
            }

            if (!ComponentKnownTranslations[component].ContainsKey(text))
            {
                ComponentKnownTranslations[component].Add(text, new Translation
                {
                    Translated = newTranslation,
                    Used = true
                });
            }

            return newTranslation;
        }

        private async Task<string> TranslateText(string text, string to, string from = "auto")
        {

            if (KnownChineseTranslations.TryGetValue(text, out var translation))
            {
                translation.Used = true;
                return translation.Translated;
            }

            try
            {
                var newTranslation = await _innerService.TranslateText(text, to, from);
                if (newTranslation is null)
                {
                    return text;
                }

                KnownChineseTranslations.Add(text, new Translation
                {
                    Translated = newTranslation,
                    Used = true
                });


                return newTranslation;
            }
            catch (Exception)
            {
                // Back up all translations on error, even the "unused" ones
                // because something went wrong and we probably still need them.
                await BackupTranslations(to, false);

                return text;
            }
        }

        private static string GetPathToKnownTranslationsFile()
        {
            var executingPath = AppDomain.CurrentDomain.BaseDirectory.ToString();
            var combinedPath = Path.Combine(executingPath, "../../../KnownChineseTranslations.json");
            var absolutePath = Path.GetFullPath(combinedPath);

            return absolutePath;
        }

        private static string GetPathToKnownTranslationsFile(string language)
        {
            var executingPath = AppDomain.CurrentDomain.BaseDirectory.ToString();
            var combinedPath = Path.Combine(executingPath, $"../../../KnownTranslations.{language}.json");
            var absolutePath = Path.GetFullPath(combinedPath);

            return absolutePath;
        }

        private static Dictionary<string, Dictionary<string, Translation>> GetComponentKnownTranslations(string lang)
        {
            var path = GetPathToKnownTranslationsFile(lang);
            if (!File.Exists(path))
            {
                return new();
            }

            return JsonSerializer.Deserialize<Dictionary<string, Dictionary<string, Translation>>>(File.ReadAllText(path), _serializerOptions)!;
        }

        private string SerializeTranslations(Dictionary<string, Translation> translations) =>
            JsonSerializer.Serialize(translations, _serializerOptions);
    }
}
