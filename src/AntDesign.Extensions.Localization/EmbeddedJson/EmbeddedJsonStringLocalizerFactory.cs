// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace AntDesign.Extensions.Localization.EmbeddedJson
{
    public class EmbeddedJsonStringLocalizerFactory : IStringLocalizerFactory
    {
        private readonly ILoggerFactory _loggerFactory;
        private readonly ConcurrentDictionary<string, IStringLocalizer> _cache = new ConcurrentDictionary<string, IStringLocalizer>();
        private readonly string _resourcesRelativePath;

        public EmbeddedJsonStringLocalizerFactory(IOptions<BlazorLocalizationOptions> localizationOptions, ILoggerFactory loggerFactory)
        {
            _loggerFactory = loggerFactory;
            _resourcesRelativePath = localizationOptions.Value.ResourcesPath ?? string.Empty;

            if (!string.IsNullOrEmpty(_resourcesRelativePath))
            {
                _resourcesRelativePath = _resourcesRelativePath.Replace(Path.AltDirectorySeparatorChar, '.').Replace(Path.DirectorySeparatorChar, '.');
            }
        }

        public IStringLocalizer Create(Type resourceSource)
        {
            var cultureInfo = CultureInfo.CurrentUICulture;
            var resourceType = resourceSource.GetTypeInfo();

            return GetCachedLocalizer(_resourcesRelativePath, resourceType.Assembly, cultureInfo);
        }

        public IStringLocalizer Create(string baseName, string location)
        {
            var cultureInfo = CultureInfo.CurrentUICulture;
            var assembly = Assembly.Load(new AssemblyName(location));

            return GetCachedLocalizer(_resourcesRelativePath, assembly, cultureInfo);
        }

        private IStringLocalizer GetCachedLocalizer(string resourceName, Assembly assembly, CultureInfo cultureInfo)
        {
            var cacheKey = GetCacheKey(resourceName, assembly, cultureInfo);
            return _cache.GetOrAdd(cacheKey, new EmbeddedJsonStringLocalizer(resourceName, assembly, cultureInfo, _loggerFactory.CreateLogger<EmbeddedJsonStringLocalizer>()));
        }

        private string GetCacheKey(string resourceName, Assembly assembly, CultureInfo cultureInfo)
        {
            return resourceName + ';' + assembly.FullName + ';' + cultureInfo.Name;
        }
    }
}
