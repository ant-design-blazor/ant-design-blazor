// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using System.Text.Json;
using System.Text.RegularExpressions;
using Microsoft.Extensions.Localization;

namespace AntDesign.Extensions.Localization
{
    public class SimpleStringLocalizerOptions : LocalizationOptions
    {
        private Dictionary<string, IDictionary<string, string>> _cache = new();
        private Assembly? _resourcesAssembly;

        public IDictionary<string, string>? GetResource(string cultureName)
        {
            return _cache.TryGetValue(cultureName, out var resource) ? resource : null;
        }

        public Assembly? ResourcesAssembly
        {
            get => _resourcesAssembly;
            set
            {
                _resourcesAssembly = value;
                if (value != null)
                {
                    LoadResources(value);
                }
            }
        }


        private void LoadResources(Assembly resourceAssembly)
        {
            var availableResources = resourceAssembly
               .GetManifestResourceNames()
               .Select(x => Regex.Match(x, $@"^.*{ResourcesPath}\.(.+)\.json"))
               .Where(x => x.Success)
               .ToDictionary(x => x.Groups[1].Value, x => x.Value);

            foreach (var resource in availableResources)
            {
                using var stream = resourceAssembly.GetManifestResourceStream(resource.Value);

                using var reader = new StreamReader(stream);

                var json = reader.ReadToEnd();

                var kv = JsonSerializer.Deserialize<IDictionary<string, string>>(json, new JsonSerializerOptions
                {
                    ReadCommentHandling = JsonCommentHandling.Skip,
                }) ?? throw new InvalidOperationException($"Failed to parse JSON: '{json}'");

                _cache.Add(resource.Key, kv);
            }
        }
    }
}
