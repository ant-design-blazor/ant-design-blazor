// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using AntDesign.DataAnnotations;
using AntDesign.Core.Reflection;

namespace AntDesign
{
    internal class EntityClassAccessor
    {
        private object _currentItemValue;
        private Type _itemType;
        private List<PropertyInfo> _itemTypePropertiesField = new List<PropertyInfo>();
        private Dictionary<string, PropertyValueAccessor> _itemValuesField = new Dictionary<string, PropertyValueAccessor>();
        private static System.Collections.Concurrent.ConcurrentDictionary<PropertyInfo, Attribute[]> _attributeCache = new System.Collections.Concurrent.ConcurrentDictionary<PropertyInfo, Attribute[]>();
        public object CurrentItem
        {
            get => _currentItemValue;
            set
            {
                _currentItemValue = value;
                UpdateFields();
            }
        }

        public EntityClassAccessor GetAccessor(object item)
        {
            EntityClassAccessor accessor = new EntityClassAccessor() { _itemType = this._itemType, _itemTypePropertiesField = this._itemTypePropertiesField, _itemValuesField = this._itemValuesField, _currentItemValue = item };
            return accessor;
        }

        public Dictionary<string, PropertyValueAccessor> Fields { get => this._itemValuesField; }

        public void UpdateFields()
        {
            if (this.CurrentItem == null || this.CurrentItem.GetType() == _itemType)
            {
                return;
            }

            _itemType = this.CurrentItem.GetType();
            _itemTypePropertiesField.Clear();
            if (_itemType.IsSubTypeOf(typeof(QueryConditionEntityViewBase)))
            {
                _itemTypePropertiesField.AddRange(_itemType.GetProperties().Where(f => f.PropertyType.IsSubTypeOf(typeof(IQueryConditionItem))).OrderBy(f => GetDisplaySequence(f)));
            }
            else
            {
                _itemTypePropertiesField.AddRange(_itemType.GetProperties().OrderBy(f => GetDisplaySequence(f)));
            }
            _itemValuesField.Clear();
            foreach (var item in _itemTypePropertiesField)
            {
                _itemValuesField.Add(item.Name, new PropertyValueAccessor(item, this.CurrentItem));
            }

            return;
        }

        internal static Attribute[] GetAttributes(PropertyInfo propertyInfo)
        {
            if (_attributeCache.ContainsKey(propertyInfo))
            {
                return _attributeCache[propertyInfo];
            }
            else
            {
                var attr = propertyInfo.GetCustomAttributes().ToArray();
                _attributeCache.TryAdd(propertyInfo, attr);
                return attr;
            }
        }

        internal static Attribute GetAttribute(PropertyInfo propertyInfo, Type attribute)
        {
            return GetAttributes(propertyInfo).FirstOrDefault(f => f?.GetType() == attribute);
        }

        internal int GetDisplaySequence(PropertyInfo propertyInfo)
        {
            if (propertyInfo == null)
            {
                return int.MaxValue;
            }
            var disa = GetAttribute(propertyInfo, typeof(DisplayAttribute)) as DisplayAttribute;
            if (disa == null)
            {
                return int.MaxValue - 1;
            }
            var seq = disa.GetOrder();
            if (seq == null)
            {
                return int.MaxValue - 1;
            }
            else
            {
                var frz = GetAttribute(propertyInfo, typeof(DynamicTableColumnSetupAttribute)) as DynamicTableColumnSetupAttribute;
                if (frz != null && frz.Frozen)
                {
                    return seq.Value;
                }
                else
                {
                    return seq.Value + 100;
                }
            }
        }

        internal string GetDisplayName(PropertyInfo propertyInfo)
        {
            if (propertyInfo == null)
            {
                return string.Empty;
            }
            var disa = GetAttribute(propertyInfo, typeof(DisplayAttribute)) as DisplayAttribute;
            if (disa == null)
            {
                return propertyInfo.Name;
            }
            var name = disa.GetName();
            if (name == null)
            {
                return propertyInfo.Name;
            }
            else
            {
                return name;
            }
        }
        internal string GetDisplayPrompt(PropertyInfo propertyInfo)
        {
            if (propertyInfo == null)
            {
                return string.Empty;
            }
            var disa = GetAttribute(propertyInfo, typeof(DisplayAttribute)) as DisplayAttribute;
            if (disa == null)
            {
                return string.Empty;
            }
            var name = disa.GetPrompt();
            if (name == null)
            {
                return string.Empty;
            }
            else
            {
                return name;
            }
        }
    }

    internal class PropertyValueAccessor
    {
        public PropertyInfo PropertyInfo { get; private set; }
        public object CurrentItem { get; set; }
        public object Value { get => this[PropertyInfo]; set => this[PropertyInfo] = value; }
        public string StringValue { get => (string)this[PropertyInfo]; set => this[PropertyInfo] = value; }
        public int IntValue { get => (int)this[PropertyInfo]; set => this[PropertyInfo] = value; }
        public int? NullableIntValue { get => (int?)this[PropertyInfo]; set => this[PropertyInfo] = value; }
        public double DoubleValue { get => (double)this[PropertyInfo]; set => this[PropertyInfo] = value; }
        public double? NullableDoubleValue { get => (double?)this[PropertyInfo]; set => this[PropertyInfo] = value; }
        public decimal DecimalValue { get => (decimal)this[PropertyInfo]; set => this[PropertyInfo] = value; }
        public decimal? NullableDecimalValue { get => (decimal?)this[PropertyInfo]; set => this[PropertyInfo] = value; }
        public long Int64Value { get => (long)this[PropertyInfo]; set => this[PropertyInfo] = value; }
        public long? NullableInt64Value { get => (long?)this[PropertyInfo]; set => this[PropertyInfo] = value; }
        public DateTime DateTimeValue { get => (DateTime)this[PropertyInfo]; set => this[PropertyInfo] = value; }
        public DateTime? NullableDateTimeValue { get => (DateTime?)this[PropertyInfo]; set => this[PropertyInfo] = value; }
        public Guid GuidValue { get => (Guid)this[PropertyInfo]; set => this[PropertyInfo] = value; }
        public bool BooleanValue { get => (Boolean)this[PropertyInfo]; set => this[PropertyInfo] = value; }
        public bool? NullableBooleanValue { get => (Boolean?)this[PropertyInfo]; set => this[PropertyInfo] = value; }
        public Guid? NullableGuidValue { get => (Guid?)this[PropertyInfo]; set => this[PropertyInfo] = value; }
        public QueryConditionStringItem QueryConditionStringItem { get => (QueryConditionStringItem)this[PropertyInfo]; }
        public QueryConditionMultiStringItem QueryConditionMulitStringItem { get => (QueryConditionMultiStringItem)this[PropertyInfo]; }
        public QueryConditionInt64RangeItem QueryConditionInt64RangeItem { get => (QueryConditionInt64RangeItem)this[PropertyInfo]; }
        public QueryConditionInt64Item QueryConditionInt64Item { get => (QueryConditionInt64Item)this[PropertyInfo]; }
        public QueryConditionInt32Item QueryConditionInt32Item { get => (QueryConditionInt32Item)this[PropertyInfo]; }
        public QueryConditionInt32RangeItem QueryConditionInt32RangeItem { get => (QueryConditionInt32RangeItem)this[PropertyInfo]; }
        public QueryConditionGuidItem QueryConditionGuidItem { get => (QueryConditionGuidItem)this[PropertyInfo]; }
        public QueryConditionExpressionItem QueryConditionExpressionItem { get => (QueryConditionExpressionItem)this[PropertyInfo]; }
        public QueryConditionDoubleRangeItem QueryConditionDoubleRangeItem { get => (QueryConditionDoubleRangeItem)this[PropertyInfo]; }
        public QueryConditionDoubleItem QueryConditionDoubleItem { get => (QueryConditionDoubleItem)this[PropertyInfo]; }
        public QueryConditionDecimalRangeItem QueryConditionDecimalRangeItem { get => (QueryConditionDecimalRangeItem)this[PropertyInfo]; }
        public QueryConditionDecimalItem QueryConditionDecimalItem { get => (QueryConditionDecimalItem)this[PropertyInfo]; }
        public QueryConditionDateTimeRangeItem QueryConditionDateTimeRangeItem { get => (QueryConditionDateTimeRangeItem)this[PropertyInfo]; }
        public QueryConditionDateTimeItem QueryConditionDateTimeItem { get => (QueryConditionDateTimeItem)this[PropertyInfo]; }
        public IQueryConditionItem QueryConditionItem { get => (IQueryConditionItem)this[PropertyInfo]; }
        public QueryConditionMultiItem<int> QueryConditionMulitIntItem { get => (QueryConditionMultiItem<int>)this[PropertyInfo]; }
        public QueryConditionMultiItem<Guid> QueryConditionMulitGuidItem { get => (QueryConditionMultiItem<Guid>)this[PropertyInfo]; }
        public QueryConditionItem<Boolean> QueryConditionBooleanItem { get => (QueryConditionItem<Boolean>)this[PropertyInfo]; }
        public PropertyValueAccessor(PropertyInfo propertyInfo, object entity)
        {
            CurrentItem = entity;
            PropertyInfo = propertyInfo;
            DisplayAttribute = EntityClassAccessor.GetAttribute(propertyInfo, typeof(DisplayAttribute)) as DisplayAttribute;
            StringLengthAttribute = EntityClassAccessor.GetAttribute(propertyInfo, typeof(StringLengthAttribute)) as StringLengthAttribute;
            RegularExpressionAttribute = EntityClassAccessor.GetAttribute(propertyInfo, typeof(RegularExpressionAttribute)) as RegularExpressionAttribute;
            DisplayFormatAttribute = EntityClassAccessor.GetAttribute(propertyInfo, typeof(DisplayFormatAttribute)) as DisplayFormatAttribute;
            MinLengthAttribute = EntityClassAccessor.GetAttribute(propertyInfo, typeof(MinLengthAttribute)) as MinLengthAttribute;
            RangeAttribute = EntityClassAccessor.GetAttribute(propertyInfo, typeof(RangeAttribute)) as RangeAttribute;
            SizeInDataFormAttribute = EntityClassAccessor.GetAttribute(propertyInfo, typeof(SizeInDataFormAttribute)) as SizeInDataFormAttribute;
            QueryConditionOperatorAttribute = EntityClassAccessor.GetAttribute(propertyInfo, typeof(QueryConditionOperatorAttribute)) as QueryConditionOperatorAttribute;
            ItemsControlAttribute = EntityClassAccessor.GetAttribute(propertyInfo, typeof(DataSourceBindAttribute)) as DataSourceBindAttribute;
            RequiredAttribute = EntityClassAccessor.GetAttribute(propertyInfo, typeof(RequiredAttribute)) as RequiredAttribute;
            DataGridColumnSetupAttribute = EntityClassAccessor.GetAttribute(propertyInfo, typeof(DynamicTableColumnSetupAttribute)) as DynamicTableColumnSetupAttribute;
            AutoGenerateBehaviorAttribute = EntityClassAccessor.GetAttribute(propertyInfo, typeof(AutoGenerateBehaviorAttribute)) as AutoGenerateBehaviorAttribute;
            if (AutoGenerateBehaviorAttribute == null)
            {
                AutoGenerateBehaviorAttribute = new AutoGenerateBehaviorAttribute() { DataFormEnabled = true, DataFormVisibility = true, DynamicTableVisibility = true, DiffFormVisibility = true/*, ExportVisibility = true */};
            }
            UIControlAttribute = EntityClassAccessor.GetAttribute(propertyInfo, typeof(UIControlAttribute)) as UIControlAttribute;
            DataGridEditAttribute = EntityClassAccessor.GetAttribute(propertyInfo, typeof(DynamicTableEditAttribute)) as DynamicTableEditAttribute;
            DataGridDetailCollectionTagAttribute = EntityClassAccessor.GetAttribute(propertyInfo, typeof(DynamicTableDetailCollectionTagAttribute)) as DynamicTableDetailCollectionTagAttribute;
            DataGridViewAttribute = EntityClassAccessor.GetAttribute(propertyInfo, typeof(DynamicTableViewAttribute)) as DynamicTableViewAttribute;
            ValidationAttributes = EntityClassAccessor.GetAttributes(propertyInfo).Where(f => f?.GetType().IsSubTypeOf(typeof(ValidationAttribute)) == true).Select(f => f as ValidationAttribute).ToArray();
            DisplayConverterAttribute = EntityClassAccessor.GetAttribute(propertyInfo, typeof(DisplayConverterAttribute)) as DisplayConverterAttribute;
        }

        public bool IsValid(object data, out string[] results)
        {
            string propertyName = this.PropertyInfo.Name;
            List<string> l = new List<string>();
            if (ValidationAttributes != null)
            {
                ValidationContext c = new ValidationContext(data);
                c.MemberName = propertyName;
                c.DisplayName = this.DisplayAttribute?.GetName() ?? propertyName;
                foreach (var v in ValidationAttributes)
                {
                    var errs = v.GetValidationResult(this.PropertyInfo.GetValue(data), c);
                    if (errs != null)
                    {
                        l.Add(errs.ErrorMessage);
                    }
                }
            }

            results = l.ToArray();
            if (results.Length == 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        private TValue GetValue<TValue>(string propertyName)
        {
            return (TValue)this[PropertyInfo];
        }

        private void SetValue<TValue>(string propertyName, TValue value)
        {
            this[PropertyInfo] = value;
        }

        private object this[PropertyInfo propertyInfo]
        {
            get
            {
                if (propertyInfo != null && this.CurrentItem != null)
                {
                    return propertyInfo.GetValue(this.CurrentItem);
                }

                return null;
            }
            set
            {
                if (propertyInfo != null && propertyInfo.CanWrite && this.CurrentItem != null)
                {
                    propertyInfo.SetValue(this.CurrentItem, value);
                }
            }
        }

        public DisplayAttribute DisplayAttribute { get; private set; }
        public StringLengthAttribute StringLengthAttribute { get; private set; }
        public RegularExpressionAttribute RegularExpressionAttribute { get; private set; }
        public DisplayFormatAttribute DisplayFormatAttribute { get; private set; }
        public MinLengthAttribute MinLengthAttribute { get; private set; }
        public RangeAttribute RangeAttribute { get; private set; }
        public SizeInDataFormAttribute SizeInDataFormAttribute { get; private set; }
        public QueryConditionOperatorAttribute QueryConditionOperatorAttribute { get; private set; }
        public DataSourceBindAttribute ItemsControlAttribute { get; private set; }
        public RequiredAttribute RequiredAttribute { get; private set; }
        public DynamicTableColumnSetupAttribute DataGridColumnSetupAttribute { get; private set; }
        public AutoGenerateBehaviorAttribute AutoGenerateBehaviorAttribute { get; private set; }
        public UIControlAttribute UIControlAttribute { get; private set; }
        public DynamicTableEditAttribute DataGridEditAttribute { get; private set; }
        public DynamicTableDetailCollectionTagAttribute DataGridDetailCollectionTagAttribute { get; private set; }
        public DynamicTableViewAttribute DataGridViewAttribute { get; private set; }
        public ValidationAttribute[] ValidationAttributes { get; private set; }
        public DisplayConverterAttribute DisplayConverterAttribute { get; private set; }
        public SizeInDataForm SizeInDataForm
        {
            get
            {
                if (SizeInDataFormAttribute == null)
                {
                    return SizeInDataForm.Normal;
                }

                return SizeInDataFormAttribute.Size;
            }
        }
        public int MinLength
        {
            get
            {
                int m1 = 0, m2 = 0;
                if (MinLengthAttribute != null)
                {
                    m1 = MinLengthAttribute.Length;
                }
                if (StringLengthAttribute != null)
                {
                    m2 = StringLengthAttribute.MinimumLength;
                }

                return Math.Max(m1, m2);
            }
        }
        public int MaxLength
        {
            get
            {
                if (StringLengthAttribute == null)
                {
                    return int.MaxValue;
                }

                return StringLengthAttribute.MaximumLength;
            }
        }
        public string DataFormat
        {
            get
            {
                if (DisplayFormatAttribute != null)
                {
                    return DisplayFormatAttribute.DataFormatString;
                }

                return string.Empty;
            }
        }

        public int IntRangeMin
        {
            get
            {
                if (RangeAttribute?.Minimum != null)
                {
                    return ReflectUtility.Convert<int>(RangeAttribute.Minimum.ToString());
                }
                else
                {
                    return int.MinValue;
                }
            }
        }
        public int IntRangeMax
        {
            get
            {
                if (RangeAttribute?.Maximum != null)
                {
                    return ReflectUtility.Convert<int>(RangeAttribute.Maximum.ToString());
                }
                else
                {
                    return int.MaxValue;
                }
            }
        }
        public long Int64RangeMin
        {
            get
            {
                if (RangeAttribute?.Minimum != null)
                {
                    return ReflectUtility.Convert<long>(RangeAttribute.Minimum.ToString());
                }
                else
                {
                    return long.MinValue;
                }
            }
        }
        public long Int64RangeMax
        {
            get
            {
                if (RangeAttribute?.Maximum != null)
                {
                    return ReflectUtility.Convert<long>(RangeAttribute.Maximum.ToString());
                }
                else
                {
                    return long.MaxValue;
                }
            }
        }
        public DateTime DateTimeRangeMin
        {
            get
            {
                if (RangeAttribute?.Minimum != null)
                {
                    return ReflectUtility.Convert<DateTime>(RangeAttribute.Minimum.ToString());
                }
                else
                {
                    return DateTime.MinValue;
                }
            }
        }
        public DateTime DateTimeRangeMax
        {
            get
            {
                if (RangeAttribute?.Maximum != null)
                {
                    return ReflectUtility.Convert<DateTime>(RangeAttribute.Maximum.ToString());
                }
                else
                {
                    return DateTime.MaxValue;
                }
            }
        }
        public double DoubleRangeMin
        {
            get
            {
                if (RangeAttribute?.Minimum != null)
                {
                    return ReflectUtility.Convert<double>(RangeAttribute.Minimum.ToString());
                }
                else
                {
                    return double.MinValue;
                }
            }
        }
        public double DoubleRangeMax
        {
            get
            {
                if (RangeAttribute?.Maximum != null)
                {
                    return ReflectUtility.Convert<double>(RangeAttribute.Maximum.ToString());
                }
                else
                {
                    return double.MaxValue;
                }
            }
        }
        public decimal DecimalRangeMin
        {
            get
            {
                if (RangeAttribute?.Minimum != null)
                {
                    return ReflectUtility.Convert<decimal>(RangeAttribute.Minimum.ToString());
                }
                else
                {
                    return 0;
                }
            }
        }
        public decimal DecimalRangeMax
        {
            get
            {
                if (RangeAttribute?.Maximum != null)
                {
                    return ReflectUtility.Convert<decimal>(RangeAttribute.Maximum.ToString());
                }
                else
                {
                    return decimal.MaxValue;
                }
            }
        }

        public string GetDataGridHeaderStyle
        {
            get
            {
                if (DataGridColumnSetupAttribute == null)
                {
                    return "";
                }

                return $"text-align:{DataGridColumnSetupAttribute.HeaderHorizontalAlignment.ToString().ToLower()};{DataGridColumnSetupAttribute.HeaderFontStyle}";
            }
        }

        public QueryConditionOperator QueryConditionOperator
        {
            get
            {
                if (QueryConditionOperatorAttribute == null)
                {
                    if (ReflectUtility.IsSubTypeOf(this.PropertyInfo.PropertyType, typeof(IQueryConditionRangeItem<>)))
                        return QueryConditionOperator.Between;
                    else if (ReflectUtility.IsSubTypeOf(this.PropertyInfo.PropertyType, typeof(QueryConditionMultiItem<>)))
                        return QueryConditionOperator.In;
                    else
                        return QueryConditionOperator.Equal;
                }

                return QueryConditionOperatorAttribute.Operator;
            }
        }

        internal string GetQueryConditionOperatorString()
        {
            switch (this.QueryConditionOperator)
            {
                case QueryConditionOperator.BiggerOrEqual:
                    return ">=";
                case QueryConditionOperator.BiggerThan:
                    return ">";
                case QueryConditionOperator.LessOrEqual:
                    return "<";
                case QueryConditionOperator.LessThan:
                    return "<=";
                default:
                    return this.QueryConditionOperator.ToString();
            }
        }


        public System.Collections.IEnumerable GetItemsControlSource()
        {
            if (this.ItemsControlAttribute == null)
                return null;
            if (string.IsNullOrEmpty(this.ItemsControlAttribute.DataSourcePath))
            {
                return (System.Collections.IEnumerable)Activator.CreateInstance(this.ItemsControlAttribute.BindType);
            }
            else
            {
                var pi = this.ItemsControlAttribute.BindType?.GetProperty(this.ItemsControlAttribute.DataSourcePath, BindingFlags.Public | BindingFlags.Static);
                if (pi == null)
                {
                    pi = this.ItemsControlAttribute.BindType?.GetProperty(this.ItemsControlAttribute.DataSourcePath);
                    return (System.Collections.IEnumerable)pi?.GetValue(Activator.CreateInstance(this.ItemsControlAttribute.BindType));
                }
                else
                {
                    return (System.Collections.IEnumerable)pi?.GetValue(null);
                }
            }
        }

        public Type QueryConditionGenericType
        {
            get
            {
                if (this.PropertyInfo.PropertyType.IsSubTypeOf(typeof(QueryConditionItem<>)))
                {
                    return this.PropertyInfo.PropertyType.GetGenericArguments()[0];
                }
                else
                {
                    return null;
                }
            }
        }

        public Expression<Func<T>> GetPropertyExpression<T>(object data)
        {
            var obj = Expression.Constant(data);
            var property = Expression.Property(obj, this.PropertyInfo);

            if (this.PropertyInfo.PropertyType.IsSubTypeOf(typeof(IQueryConditionValueItem<>)))
            {
                property = Expression.Property(property, this.Value.GetType().GetProperty("Value"));
            }
            var lambda = Expression.Lambda<Func<T>>(property);

            return lambda;
        }

        public object GetPropertyExpressionBoxing(object data)
        {
            Type t = this.PropertyInfo.PropertyType;
            if (this.PropertyInfo.PropertyType.IsSubTypeOf(typeof(IQueryConditionValueItem<>)))
            {
                t = t.GetProperty("Value").PropertyType;
            }
            return ReflectUtility.Invoke(this, "GetPropertyExpression", null, new Type[] { t }, data);
        }

        public EventCallback<T> GetPropertyChangedCallback<T>(ComponentBase page, object data, bool stateHasChanged)
        {
            var obj = Expression.Constant(data);
            var property = Expression.Property(obj, this.PropertyInfo);
            Type eType = this.PropertyInfo.PropertyType;
            if (this.PropertyInfo.PropertyType.IsSubTypeOf(typeof(IQueryConditionValueItem<>)))
            {
                var pi = this.Value.GetType().GetProperty("Value");
                property = Expression.Property(property, pi);
                eType = pi.PropertyType;
            }
            var e = Expression.Parameter(eType, "e");
            var lambda = Expression.Assign(property, e);
            if (stateHasChanged)
            {
                var pageobj = Expression.Constant(page);
                var haschanged = Expression.Call(pageobj, typeof(ComponentBase).GetMethod("StateHasChanged", BindingFlags.NonPublic | BindingFlags.Instance));
                return EventCallback.Factory.Create(data, Expression.Lambda<Action<T>>(Expression.Block(lambda, haschanged), e).Compile());
            }
            else
            {
                return EventCallback.Factory.Create(data, Expression.Lambda<Action<T>>(lambda, e).Compile());
            }
        }

        public object GetPropertyChangedCallbackBoxing(ComponentBase page, object data, bool stateHasChanged)
        {
            Type t = this.PropertyInfo.PropertyType;
            if (this.PropertyInfo.PropertyType.IsSubTypeOf(typeof(IQueryConditionValueItem<>)))
            {
                t = t.GetProperty("Value").PropertyType;
            }
            return ReflectUtility.Invoke(this, "GetPropertyChangedCallback", null, new Type[] { t }, page, data, stateHasChanged);
        }

        public RenderFragment GetUIControl(Type control, string bindProperty, ComponentBase page, object data, bool stateHasChanged, IDictionary<string, object> extendProperties = null)
        {
            void Foo(RenderTreeBuilder builder)
            {
                int idx = 0;
                builder.OpenComponent(idx++, control);
                if (!string.IsNullOrEmpty(bindProperty))
                {
                    if (control.GetProperty($"{bindProperty}Expression") != null)
                    {
                        builder.AddAttribute(idx++, $"{bindProperty}Expression", GetPropertyExpressionBoxing(data));
                    }
                    if (control.GetProperty($"{bindProperty}Changed") != null)
                    {
                        builder.AddAttribute(idx++, $"{bindProperty}Changed", GetPropertyChangedCallbackBoxing(page, data, stateHasChanged));
                    }
                    if (control.GetProperty(bindProperty) != null)
                    {
                        if (this.PropertyInfo.PropertyType.IsSubTypeOf(typeof(IQueryConditionValueItem<>)))
                        {
                            var item = this.PropertyInfo.GetValue(data);
                            builder.AddAttribute(idx++, $"{bindProperty}", ReflectUtility.GetProperty(item, "Value"));
                        }
                        else
                        {
                            builder.AddAttribute(idx++, $"{bindProperty}", this.PropertyInfo.GetValue(data));
                        }
                    }
                }
                if (extendProperties == null)
                {
                    extendProperties = new Dictionary<string, object>();
                }

                if (control.IsSubTypeOf(typeof(AntDesign.Input<>)))
                {
                    //disable DebounceMilliseconds by default
                    if (!extendProperties.ContainsKey("DebounceMilliseconds"))
                    {
                        extendProperties.Add("DebounceMilliseconds", -1);
                    }
                }
                if (control.IsSubTypeOf(typeof(AntDesign.SelectBase<,>)) && this.ItemsControlAttribute != null)
                {
                    if (!extendProperties.ContainsKey("ValueName"))
                    {
                        extendProperties.Add("ValueName", this.ItemsControlAttribute.ValuePath);
                    }
                    if (!extendProperties.ContainsKey("LabelName"))
                    {
                        extendProperties.Add("LabelName", this.ItemsControlAttribute.LabelPath);
                    }
                    if (!extendProperties.ContainsKey("DataSource"))
                    {
                        extendProperties.Add("DataSource", this.GetItemsControlSource());
                    }
                }
                if (control == typeof(AntDesign.Cascader) && this.ItemsControlAttribute != null)
                {
                    if (!extendProperties.ContainsKey("Options"))
                    {
                        extendProperties.Add("Options", this.GetItemsControlSource());
                    }
                }
                if (extendProperties != null)
                {
                    foreach (var d in extendProperties)
                    {
                        var pi = control.GetProperty(d.Key);
                        if (pi != null && pi.CanWrite)
                        {
                            if (pi.PropertyType.IsSubTypeOf(typeof(OneOf.IOneOf)) && pi.PropertyType.IsGenericType && d.Value != null)
                            {
                                int piIndex = Array.IndexOf(pi.PropertyType.GetGenericArguments(), d.Value.GetType());
                                if (piIndex != -1)
                                {
                                    var oneOf = ReflectUtility.Invoke(pi.PropertyType, $"FromT{piIndex}", d.Value);
                                    builder.AddAttribute(idx++, d.Key, oneOf);
                                }
                            }
                            else
                            {
                                builder.AddAttribute(idx++, d.Key, d.Value);
                            }
                        }
                    }
                }

                builder.CloseComponent();
            }

            return Foo;

        }
    }
}
