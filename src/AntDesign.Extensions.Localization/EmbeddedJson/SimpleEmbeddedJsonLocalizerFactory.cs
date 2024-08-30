// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Collections.Concurrent;
using System.Globalization;
using System.Reflection;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace AntDesign.Extensions.Localization.EmbeddedJson
{
    internal sealed class SimpleEmbeddedJsonLocalizerFactory : IStringLocalizerFactory
    {
        private readonly ILoggerFactory _loggerFactory;
        private readonly ConcurrentDictionary<string, IStringLocalizer> _cache = new ConcurrentDictionary<string, IStringLocalizer>();
        private readonly string _resourcesRelativePath;
        private readonly IOptions<SimpleStringLocalizerOptions> _localizationOptions;

        public SimpleEmbeddedJsonLocalizerFactory(IOptions<SimpleStringLocalizerOptions> localizationOptions, ILoggerFactory loggerFactory)
        {
            _loggerFactory = loggerFactory;
            _resourcesRelativePath = localizationOptions.Value.ResourcesPath ?? string.Empty;
            _localizationOptions = localizationOptions;

            if (!string.IsNullOrEmpty(_resourcesRelativePath))
            {
                _resourcesRelativePath = _resourcesRelativePath.Replace(Path.AltDirectorySeparatorChar, '.').Replace(Path.DirectorySeparatorChar, '.');
            }
        }

        public IStringLocalizer Create(Type resourceSource)
        {
            var cultureInfo = CultureInfo.CurrentUICulture;
            var typeInfo = resourceSource.GetTypeInfo();
            var baseName = GetResourcePrefix(typeInfo);
            var assembly = typeInfo.Assembly;

            return GetCachedLocalizer(baseName, assembly, cultureInfo);
        }

        public IStringLocalizer Create(string baseName, string location)
        {
            var cultureInfo = CultureInfo.CurrentUICulture;
            var assembly = Assembly.Load(new AssemblyName(location)) ?? _localizationOptions.Value.ResourcesAssembly;

            return GetCachedLocalizer(_resourcesRelativePath, assembly, cultureInfo);
        }

        private RootNamespaceAttribute? GetRootNamespaceAttribute(Assembly assembly)
        {
            return assembly.GetCustomAttribute<RootNamespaceAttribute>();
        }


        private string GetResourcePrefix(TypeInfo typeInfo)
        {
            return GetResourcePrefix(typeInfo, GetRootNamespace(typeInfo.Assembly), GetResourcePath(typeInfo.Assembly));
        }

        private string GetResourcePrefix(TypeInfo typeInfo, string? baseNamespace, string? resourcesRelativePath)
        {
            if (string.IsNullOrEmpty(resourcesRelativePath))
            {
                return typeInfo.FullName;
            }
            else
            {
                // This expectation is defined by dotnet's automatic resource storage.
                // We have to conform to "{RootNamespace}.{ResourceLocation}.{FullTypeName - RootNamespace}".
                return baseNamespace + "." + resourcesRelativePath + TrimPrefix(typeInfo.FullName, baseNamespace + ".");
            }
        }

        private static string? TrimPrefix(string name, string prefix)
        {
            if (name.StartsWith(prefix, StringComparison.Ordinal))
            {
                return name.Substring(prefix.Length);
            }

            return name;
        }

        private string? GetRootNamespace(Assembly assembly)
        {
            var rootNamespaceAttribute = GetRootNamespaceAttribute(assembly);

            if (rootNamespaceAttribute != null)
            {
                return rootNamespaceAttribute.RootNamespace;
            }

            return assembly.GetName().Name;
        }

        private string GetResourcePath(Assembly assembly)
        {
            var resourceLocationAttribute = GetResourceLocationAttribute(assembly);

            // If we don't have an attribute assume all assemblies use the same resource location.
            var resourceLocation = resourceLocationAttribute == null
                ? _resourcesRelativePath + "."
                : resourceLocationAttribute.ResourceLocation + ".";
            resourceLocation = resourceLocation
                .Replace(Path.DirectorySeparatorChar, '.')
                .Replace(Path.AltDirectorySeparatorChar, '.');

            return resourceLocation;
        }

        private ResourceLocationAttribute? GetResourceLocationAttribute(Assembly assembly)
        {
            return assembly.GetCustomAttribute<ResourceLocationAttribute>();
        }


        private IStringLocalizer GetCachedLocalizer(string resourceName, Assembly assembly, CultureInfo cultureInfo)
        {
            var cacheKey = GetCacheKey(resourceName, assembly, cultureInfo);
            return _cache.GetOrAdd(cacheKey, new SimpleEmbeddedJsonStringLocalizer(resourceName, assembly, cultureInfo, _localizationOptions));
        }

        private string GetCacheKey(string resourceName, Assembly assembly, CultureInfo cultureInfo)
        {
            return resourceName + ';' + assembly.FullName + ';' + cultureInfo.Name;
        }
    }
}
