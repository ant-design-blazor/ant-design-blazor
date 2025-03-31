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

        private readonly JsonSerializerOptions _serializerOptions;

        public TranslateCache(ITranslate innerService)
        {
            _innerService = innerService;

            var encoding = new TextEncoderSettings();
            encoding.AllowRange(UnicodeRanges.All);

            _serializerOptions = new JsonSerializerOptions
            {
                WriteIndented = true,
                Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping
            };

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

        public async Task BackupTranslations(bool onlyKeepUsed = true)
        {
            var translations = KnownChineseTranslations;

            if (onlyKeepUsed)
            {
                translations = translations
                    .Where(x => x.Value.Used)
                    .ToDictionary(x => x.Key, x => x.Value);
            }

            await File.WriteAllTextAsync(GetPathToKnownTranslationsFile(), SerializeTranslations(translations), Encoding.UTF8);
        }

        public async Task<string> TranslateText(string text, string to, string from = "auto")
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
                await BackupTranslations(false);

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

        private string SerializeTranslations(Dictionary<string, Translation> translations) =>
            JsonSerializer.Serialize(translations, _serializerOptions);
    }
}
