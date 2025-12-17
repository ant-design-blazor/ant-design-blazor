// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Components;

namespace AntDesign
{
    /// <summary>
    /// Marks a property to use a relation data component for rendering.
    /// </summary>
    /// <remarks>
    /// <para>
    /// This attribute is used with PropertyColumn for declarative configuration of relation data rendering.
    /// </para>
    /// <para>
    /// <b>Use Case</b>: When you want to configure relation data rendering using attributes instead of manually writing ChildContent.
    /// </para>
    /// <para>
    /// <b>How it works</b>: PropertyColumn checks for this attribute during initialization. If found and ChildContent is null,
    /// it automatically creates the specified relation component.
    /// </para>
    /// </remarks>
    /// <example>
    /// <para><b>Example 1: Basic Usage</b></para>
    /// <code>
    /// public class Order
    /// {
    ///     public int Id { get; set; }
    ///     
    ///     [RelationColumn(typeof(UserNameRelation))]
    ///     public int UserId { get; set; }
    ///     
    ///     [RelationColumn(typeof(ProductNameRelation))]
    ///     public int ProductId { get; set; }
    /// }
    /// 
    /// // Usage:
    /// &lt;Table DataSource="orders"&gt;
    ///     &lt;PropertyColumn Property="o => o.UserId" /&gt;
    ///     &lt;PropertyColumn Property="o => o.ProductId" /&gt;
    /// &lt;/Table&gt;
    /// </code>
    /// 
    /// <para><b>Example 2: Component with Parameters</b></para>
    /// <code>
    /// public class Order
    /// {
    ///     [RelationColumn(typeof(UserAvatarRelation))]
    ///     public int UserId { get; set; }
    /// }
    /// 
    /// // In PropertyColumn usage:
    /// &lt;PropertyColumn Property="o => o.UserId"&gt;
    ///     &lt;UserAvatarRelation Size="50" ShowName="true" /&gt;
    /// &lt;/PropertyColumn&gt;
    /// </code>
    /// </example>
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false, Inherited = true)]
    public sealed class RelationColumnAttribute : Attribute
    {
        // Cache for RenderFragment by component type and parameters hash
        private static readonly ConcurrentDictionary<string, RenderFragment> _renderFragmentCache = new();

        // Cache for property types
        private static readonly ConcurrentDictionary<string, Type> _propertyTypeCache = new();

        /// <summary>
        /// Gets the type of the relation data rendering component.
        /// </summary>
        /// <remarks>
        /// This type must implement <see cref="IRelationComponent"/> interface,
        /// typically by inheriting from <see cref="RelationComponentBase{TItem, TData}"/>.
        /// </remarks>
        public Type ComponentType { get; }

        /// <summary>
        /// Gets or sets the parameters to pass to the component.
        /// </summary>
        /// <remarks>
        /// <para>
        /// Dictionary where keys are parameter names and values are parameter values.
        /// </para>
        /// <para>
        /// Example: new Dictionary&lt;string, object&gt; { ["Size"] = 50, ["ShowName"] = true }
        /// </para>
        /// <para>
        /// Values will be automatically converted to the target parameter types.
        /// </para>
        /// </remarks>
        public Dictionary<string, object> Parameters { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="RelationColumnAttribute"/> class.
        /// </summary>
        /// <param name="componentType">The type of the relation data rendering component</param>
        /// <exception cref="ArgumentNullException">Thrown when componentType is null</exception>
        /// <exception cref="ArgumentException">Thrown when componentType does not implement IRelationComponent interface</exception>
        public RelationColumnAttribute(Type componentType)
        {
            if (componentType == null)
            {
                throw new ArgumentNullException(nameof(componentType));
            }

            // Verify that the type implements IRelationComponent interface
            if (!typeof(IRelationComponent).IsAssignableFrom(componentType))
            {
                throw new ArgumentException(
                    $"Component type '{componentType.FullName}' must implement IRelationComponent interface.",
                    nameof(componentType));
            }

            ComponentType = componentType;
        }

        /// <summary>
        /// Creates a RenderFragment for the relation component based on this attribute.
        /// </summary>
        /// <returns>RenderFragment that renders the relation component</returns>
        public RenderFragment CreateRelationComponentContent()
        {
            // Generate cache key based on component type and parameters
            var cacheKey = GenerateCacheKey();

            return _renderFragmentCache.GetOrAdd(cacheKey, _ => CreateRenderFragmentInternal());
        }

        /// <summary>
        /// Generates a unique cache key based on component type and parameters.
        /// </summary>
        private string GenerateCacheKey()
        {
            if (Parameters == null || Parameters.Count == 0)
            {
                return ComponentType.FullName;
            }

            // Include parameters in cache key
            return $"{ComponentType.FullName}:{string.Join(",", Parameters.Select(kvp => $"{kvp.Key}={kvp.Value}"))}";
        }

        /// <summary>
        /// Internal method to create the actual RenderFragment.
        /// </summary>
        private RenderFragment CreateRenderFragmentInternal()
        {
            return builder =>
            {
                builder.OpenComponent(0, ComponentType);

                // Add parameters if any
                if (Parameters != null && Parameters.Count > 0)
                {
                    try
                    {
                        int sequence = 1;

                        foreach (var kvp in Parameters)
                        {
                            var propCacheKey = $"{ComponentType.FullName}.{kvp.Key}";

                            // Get or create property type from cache
                            var propertyType = _propertyTypeCache.GetOrAdd(propCacheKey, _ =>
                            {
                                var propInfo = ComponentType.GetProperty(kvp.Key);
                                return propInfo?.CanWrite == true ? propInfo.PropertyType : null;
                            });

                            if (propertyType != null)
                            {
                                // Convert parameter value to target type
                                object convertedValue = ConvertParameterValue(kvp.Value, propertyType);
                                builder.AddAttribute(sequence++, kvp.Key, convertedValue);
                            }
                        }
                    }
                    catch
                    {
                        // Ignore parameter conversion failures, use default values
                    }
                }

                builder.CloseComponent();
            };
        }

        /// <summary>
        /// Converts parameter value to the target type.
        /// </summary>
        private static object ConvertParameterValue(object value, Type targetType)
        {
            if (value == null)
                return null;

            var valueStr = value.ToString();

            // Handle nullable types
            var underlyingType = Nullable.GetUnderlyingType(targetType) ?? targetType;

            // Special handling for boolean type
            if (underlyingType == typeof(bool))
            {
                if (bool.TryParse(valueStr, out var boolValue))
                    return boolValue;
            }
            // Integer type
            else if (underlyingType == typeof(int))
            {
                if (int.TryParse(valueStr, out var intValue))
                    return intValue;
            }
            // Long integer type
            else if (underlyingType == typeof(long))
            {
                if (long.TryParse(valueStr, out var longValue))
                    return longValue;
            }
            // Double precision type
            else if (underlyingType == typeof(double))
            {
                if (double.TryParse(valueStr, out var doubleValue))
                    return doubleValue;
            }
            // String type
            else if (underlyingType == typeof(string))
            {
                return valueStr;
            }

            // Try general conversion
            try
            {
                return Convert.ChangeType(valueStr, underlyingType);
            }
            catch
            {
                return value;
            }
        }
    }
}
