// Licensed to the .NET Foundation under one or more agreements.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections;
using System.Collections.Generic;

namespace AntDesign
{
    /// <summary>Prevents the component props source generator from creating a props class for the annotated component.</summary>
    [AttributeUsage(AttributeTargets.Class)]
    public sealed class SkipComponentPropsGenerationAttribute : Attribute
    {
    }

    /// <summary>
    /// Base class for values supplied to a component through <see cref="Microsoft.AspNetCore.Components.CascadingValue{TValue}"/>.
    /// </summary>
    public abstract class ComponentProps
    {
        private readonly HashSet<string> _setProperties = new(StringComparer.Ordinal);

        /// <summary>Returns whether the property was explicitly assigned by the caller.</summary>
        public bool IsSet(string propertyName) => _setProperties.Contains(propertyName);

        /// <summary>Marks a property as explicitly assigned and stores its value.</summary>
        protected void SetProperty<T>(ref T field, T value, string propertyName)
        {
            field = value;
            _setProperties.Add(propertyName);
        }
    }

    /// <summary>
    /// A named set of component property overrides supplied through a cascading value.
    /// </summary>
    public sealed class ComponentPropsCollection : IEnumerable<ComponentProps>
    {
        private readonly Dictionary<string, ComponentProps> _values = new(StringComparer.Ordinal);
        private readonly Dictionary<Type, ComponentProps> _defaults = new();

        /// <summary>Creates a collection with defaults for one or more component props types.</summary>
        public ComponentPropsCollection(params ComponentProps[] props)
        {
            foreach (var value in props)
            {
                if (value is null)
                {
                    throw new ArgumentNullException(nameof(props));
                }

                _defaults[value.GetType()] = value;
            }
        }

        /// <summary>Registers the overrides for a component instance identified by <paramref name="id" />.</summary>
        public ComponentProps this[string id]
        {
            set
            {
                if (string.IsNullOrWhiteSpace(id))
                {
                    throw new ArgumentException("A component id is required.", nameof(id));
                }

                _values[id] = value ?? throw new ArgumentNullException(nameof(value));
            }
        }

        /// <summary>Adds the default overrides for every component using this props type.</summary>
        public void Add<TProps>(TProps props)
            where TProps : ComponentProps
        {
            _defaults[typeof(TProps)] = props ?? throw new ArgumentNullException(nameof(props));
        }

        /// <summary>Adds the overrides for the component instance identified by <paramref name="id" />.</summary>
        public void Add(string id, ComponentProps props)
        {
            this[id] = props;
        }

        /// <summary>Gets the default overrides for a component props type.</summary>
        public TProps? GetDefault<TProps>()
            where TProps : ComponentProps
            => _defaults.TryGetValue(typeof(TProps), out var value) ? value as TProps : null;

        /// <summary>Gets the overrides matching both the id and component props type.</summary>
        public TProps? Get<TProps>(string? id)
            where TProps : ComponentProps
            => id is not null && _values.TryGetValue(id, out var value) ? value as TProps : null;

        internal ComponentPropsCollection Clone()
        {
            var result = new ComponentPropsCollection();
            foreach (var value in _values)
            {
                result._values.Add(value.Key, value.Value);
            }

            foreach (var value in _defaults)
            {
                result._defaults.Add(value.Key, value.Value);
            }

            return result;
        }

        internal void AddRange(ComponentPropsCollection props)
        {
            foreach (var value in props._defaults)
            {
                _defaults[value.Key] = value.Value;
            }

            foreach (var value in props._values)
            {
                _values[value.Key] = value.Value;
            }
        }

        public IEnumerator<ComponentProps> GetEnumerator()
        {
            foreach (var value in _defaults.Values)
            {
                yield return value;
            }

            foreach (var value in _values.Values)
            {
                yield return value;
            }
        }

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }

    public interface IComponentPropsReceiver
    {
        void ApplyCascadingProps();
    }
}
