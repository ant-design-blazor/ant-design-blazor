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

namespace AntDesign.Docs.Localization.EmbeddedJson
{
    public class EmbeddedJsonStringLocalizerFactory : IStringLocalizerFactory
    {
        private readonly ILoggerFactory _loggerFactory;
        private readonly ConcurrentDictionary<string, IStringLocalizer> _cache = new ConcurrentDictionary<string, IStringLocalizer>();
        private readonly string _resourceBasePath;
        private readonly Assembly _resourceAssembly;

        public EmbeddedJsonStringLocalizerFactory(string resourceBasePath, Assembly resourceAssembly, ILoggerFactory loggerFactory)
        {
            _resourceBasePath = resourceBasePath;
            _loggerFactory = loggerFactory;
            _resourceAssembly = resourceAssembly;
        }

        public IStringLocalizer Create(Type resourceSource)
        {
            var resourceType = resourceSource.GetTypeInfo();
            var cultureInfo = CultureInfo.CurrentUICulture;
            var resourceName = $"{_resourceBasePath.Replace("/", ".")}.{cultureInfo.Name}.json";
            return GetCachedLocalizer(resourceName, resourceType.Assembly, cultureInfo);
        }

        public IStringLocalizer Create(string baseName, string location)
        {
            var cultureInfo = CultureInfo.CurrentUICulture;
            var resourceName = $"{baseName}.json";
            return GetCachedLocalizer(resourceName, _resourceAssembly, cultureInfo);
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
