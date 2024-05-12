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
        public Dictionary<string, IDictionary<string, string>> Resources { get; set; } = new();
        public Assembly? ResourcesAssembly { get; set; }

        public IDictionary<string, string>? GetResource(string cultureName)
        {
            return Resources.TryGetValue(cultureName, out var resource) ? resource : null;
        }

        public static Dictionary<string, IDictionary<string, string>> BuildResources(string resourcesPath, Assembly? resourceAssembly)
        {
            Dictionary<string, IDictionary<string, string>> cache = new();
            resourceAssembly ??= Assembly.GetCallingAssembly();
            var availableResources = resourceAssembly
               .GetManifestResourceNames()
               .Select(x => Regex.Match(x, $@"^.*{resourcesPath}\.(.+)\.json"))
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

                cache.Add(resource.Key, kv);
            }

            return cache;
        }
    }
}
