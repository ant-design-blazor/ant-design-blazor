// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.Json;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Logging;

namespace AntDesign.Docs.Localization.EmbeddedJson
{
    public class EmbeddedJsonStringLocalizer : IStringLocalizer
    {
        private readonly Lazy<IDictionary<string, string>> _resources;
        private readonly Lazy<IDictionary<string, string>> _fallbackResources;

        public EmbeddedJsonStringLocalizer(string resourceName, Assembly resourceAssembly, CultureInfo cultureInfo, ILogger<EmbeddedJsonStringLocalizer> logger)
        {
            _resources = new Lazy<IDictionary<string, string>>(
                () => ReadResources(resourceName, resourceAssembly, cultureInfo, logger, isFallback: false));
            _fallbackResources = new Lazy<IDictionary<string, string>>(
                () => ReadResources(resourceName, resourceAssembly, cultureInfo.Parent, logger, isFallback: true));
        }

        public LocalizedString this[string name]
        {
            get
            {
                if (name == null) throw new ArgumentNullException(nameof(name));
                if (TryGetResource(name, out var value))
                {
                    return new LocalizedString(name, value, resourceNotFound: false);
                }
                return new LocalizedString(name, name, resourceNotFound: true);
            }
        }

        public LocalizedString this[string name, params object[] arguments]
        {
            get
            {
                if (name == null) throw new ArgumentNullException(nameof(name));
                if (TryGetResource(name, out var value))
                {
                    return new LocalizedString(name, string.Format(value, arguments), resourceNotFound: false);
                }
                return new LocalizedString(name, string.Format(name, arguments), resourceNotFound: true);
            }
        }

        public IEnumerable<LocalizedString> GetAllStrings(bool includeParentCultures)
        {
            return _resources.Value.Select(r => new LocalizedString(r.Key, r.Value));
        }

        private bool TryGetResource(string name, out string value)
        {
            return _resources.Value.TryGetValue(name, out value) ||
                _fallbackResources.Value.TryGetValue(name, out value);
        }

        private static IDictionary<string, string> ReadResources(string resourcePathName, Assembly resourceAssembly, CultureInfo cultureInfo, ILogger<EmbeddedJsonStringLocalizer> logger, bool isFallback)
        {
            var availableResources = resourceAssembly
                .GetManifestResourceNames()
                .Select(x => Regex.Match(x, $@"^.*{resourcePathName}\.(.+)\.json"))
                .Where(x => x.Success)
                .Select(x => (CultureName: x.Groups[1].Value, ResourceName: x.Value))
                .ToList();

            var (_, resourceName) = availableResources.FirstOrDefault(x => x.CultureName.Equals(cultureInfo.Name, StringComparison.OrdinalIgnoreCase));

            using var stream = resourceAssembly.GetManifestResourceStream(resourceName);

            using var reader = new StreamReader(stream);

            string json = reader.ReadToEnd();

            return JsonSerializer.Deserialize<IDictionary<string, string>>(json, new JsonSerializerOptions
            {
                ReadCommentHandling = JsonCommentHandling.Skip,
            }) ?? throw new InvalidOperationException($"Failed to parse JSON: '{json}'");
        }
    }
}
