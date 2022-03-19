// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using Microsoft.AspNetCore.Components;
using System.Collections.Generic;
using AntDesign.Core.Reflection;

namespace AntDesign.DataAnnotations
{
    public class DynamicTableViewAttribute : DynamicTableEditAttribute
    {
        public TextAlignment TextAlignment { get; set; }
        public int Height { get; set; }
        /// <summary>
        /// Format and ConverterType, are exclusive, ConverterType has a higher priority.
        /// </summary>
        private string Format { get; set; }
        /// <summary>
        /// a class implemented IValueConverter
        /// </summary>
        public Type ConverterType { get; set; }
        private static System.Collections.Concurrent.ConcurrentDictionary<Type, IValueConverter> _converters = new System.Collections.Concurrent.ConcurrentDictionary<Type, IValueConverter>();
        internal static IValueConverter GetConverter(Type t)
        {
            if (t == null || !t.IsSubTypeOf(typeof(IValueConverter)))
            {
                return null;
            }
            if (_converters.ContainsKey(t))
            {
                return _converters[t];
            }
            else
            {
                var c = (IValueConverter)Activator.CreateInstance(t);
                _converters.TryAdd(t, c);
                return c;
            }
        }
        public IValueConverter GetConverter()
        {
            return GetConverter(this.ConverterType);
        }

        public DynamicTableViewAttribute(Type uicontroltype, string bindproperty, string[] extraProperties = null, object[] extraPropertyValues = null)
            : base(uicontroltype, bindproperty, extraProperties, extraPropertyValues)
        {
            TextAlignment = TextAlignment.Left;
            Height = 25;
        }

        public DynamicTableViewAttribute()
            : this(typeof(AntDesign.Input<string>), "Value", null)
        {
            TextAlignment = TextAlignment.Left;
        }
    }

    /// <summary>
    /// A converter for display purpose, will applied in DiffForm/DynamicTable
    /// </summary>
    public class DisplayConverterAttribute : Attribute
    {
        public object ConvertorParameter { get; set; }

        internal Type _converter = null;
        public DisplayConverterAttribute(Type valueconvertertype)
        {
            if (valueconvertertype == null)
                throw new NullReferenceException("DisplayConverterAttribute(Type valueconvertertype), valueconvertertype can not be null.");
            if (valueconvertertype.GetInterface(typeof(IValueConverter).FullName, false) == null)
                throw new Exception("DisplayConverterAttribute(Type valueconvertertype), valueconvertertype must be a IValueConverter2.");
            _converter = valueconvertertype;
        }

        public IValueConverter GetConverter()
        {
            return DynamicTableViewAttribute.GetConverter(this._converter);
        }
    }

    public class UIControlAttribute : Attribute
    {
        internal protected Type _uicontroltype = null;
        protected internal string BindPropertyName { get; private set; }
        public IDictionary<string, object> ExtraProperties { get; set; }

        public UIControlAttribute(Type uicontroltype, string bindproperty, string[] extraProperties = null, object[] extraPropertyValues = null)
        {
            if (uicontroltype == null)
                throw new NullReferenceException("UIControlAttribute(Type uicontroltype,string bindproperty), uicontroltype can not be null.");
            if (!ReflectUtility.IsSubTypeOf(uicontroltype, typeof(ComponentBase)))
                throw new Exception("UIControlAttribute(Type uicontroltype,string bindproperty), uicontroltype must be a subtype of ComponentBase .");
            if (string.IsNullOrEmpty(bindproperty))
                throw new NullReferenceException("UIControlAttribute(Type uicontroltype,string bindproperty), binddependencyproperty can not be empty.");
            //if (uicontroltype.IsGenericType)
            //    _uicontroltype = FatLynx.Utility.BuildTypeFromGenericType(uicontroltype);
            //else
            _uicontroltype = uicontroltype;

            BindPropertyName = bindproperty;
            if (extraProperties != null && extraPropertyValues != null)
            {
                if (extraProperties.Length != extraPropertyValues.Length)
                {
                    throw new NullReferenceException("UIControlAttribute(Type uicontroltype,string bindproperty,string[] extraProperties, object[] extraPropertyValues), extraProperties.Length!=extraPropertyValues.Length.");
                }

                ExtraProperties = new Dictionary<string, object>();
                for (int i = 0; i < extraProperties.Length; i++)
                {
                    ExtraProperties[extraProperties[i]] = extraPropertyValues[i];
                }
            }
        }

        internal UIControlAttribute() { }
    }

    public class DynamicTableEditAttribute : UIControlAttribute
    {
        public DynamicTableEditAttribute(Type uicontroltype, string bindproperty, string[] extraProperties = null, object[] extraPropertyValues = null) : base(uicontroltype, bindproperty, extraProperties, extraPropertyValues) { }
        public DynamicTableEditAttribute() : this(typeof(AntDesign.Input<string>), "Value") { }
    }
    public class DynamicTableColumnSetupAttribute : Attribute
    {
        //public bool CanReorder { get; set; }
        public bool CanSort { get; set; }
        //public bool CanResize { get; set; }
        public bool Frozen { get; set; }
        //public bool AllowFrozen { get; set; }
        public bool IsReadOnly { get; set; }
        public TextAlignment HeaderHorizontalAlignment { get; set; }
        public int Width { get; set; }
        public string HeaderFontStyle { get; set; }
        public DynamicTableColumnSetupAttribute()
        {
            //CanReorder = true;
            CanSort = true;
            //CanResize = true;
            Frozen = false;
            IsReadOnly = false;
            //AllowFrozen = true;
            HeaderHorizontalAlignment = TextAlignment.Left;
            Width = 0;
        }
    }

    public class DynamicTableDetailCollectionTagAttribute : Attribute
    {
        //TODO
    }

    public enum TextAlignment
    {
        Left, Center, Right
    }

}
