﻿// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

#nullable enable
using System;
using System.ComponentModel;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Rendering;
using AntDesign.Internal;

namespace AntDesign;

public partial class GenerateFormItem<TModel> : ComponentBase
{
    private PropertyInfo[] _propertyInfos = { };


    [CascadingParameter(Name = "Form")]
    private IForm Form { get; set; }


    /// <summary>
    /// If true then the rules will be required by CheckTypeRequire.
    /// Recommended only for forms without complex validation, where only simple validation for empty values is required.
    /// </summary>
    [Parameter]
    public bool RulesRequireByType { get; set; }

    [Parameter] public Func<PropertyInfo, TModel, RenderFragment> Definitions { get; set; }

    [Parameter] public Func<PropertyInfo, TModel, bool> NotGenerate { get; set; }

    protected override void OnInitialized()
    {
        _propertyInfos = typeof(TModel).GetProperties();

        base.OnInitialized();
    }

    protected override void BuildRenderTree(RenderTreeBuilder builder)
    {
        foreach (var property in _propertyInfos)
        {
            // Not Generate
            if (NotGenerate != null && NotGenerate.Invoke(property, (TModel)Form.Model))
            {
                continue;
            }

            var displayName = property.GetCustomAttribute<DisplayNameAttribute>()?.DisplayName ?? property.Name;
            var isRequired = RulesRequireByType is true && CheckTypeRequire(property);
            var underlyingType = THelper.GetUnderlyingType(property.PropertyType);
            RenderFragment? childContent = null;
            if (Definitions != null)
            {
                childContent = Definitions.Invoke(property, (TModel)Form.Model);
            }

            childContent ??= GenerateByType(underlyingType, property, displayName, isRequired);

            // Make FormItem 
            if (isRequired)
            {
                var formValidation = GetFormValidationRuleByType(underlyingType);
                builder.OpenComponent(0, typeof(FormItem));
                builder.AddAttribute(1, "Label", displayName);
                builder.AddAttribute(2, "Rules", formValidation);
                builder.AddAttribute(3, "ChildContent", childContent);
                builder.CloseComponent();
            }
            else
            {
                builder.OpenComponent(0, typeof(FormItem));
                builder.AddAttribute(1, "Label", displayName);
                builder.AddAttribute(2, "ChildContent", childContent);
                builder.CloseComponent();
            }
        }
    }

    private bool CheckTypeRequire(PropertyInfo property)
    {
        if (property.PropertyType.IsValueType)
        {
            return !THelper.IsTypeNullable(property.PropertyType);
        }

        return !(property.GetMethod?.CustomAttributes.Where(x =>
            x.AttributeType.Name == "NullableContextAttribute") ?? Array.Empty<CustomAttributeData>()).Any();
    }

    private RenderFragment? GenerateByType(Type underlyingType, PropertyInfo property, string displayName,
        bool isRequired)
    {
        if (underlyingType == typeof(string))
        {
            return MakeInputString(property, displayName);
        }

        if (THelper.IsNumericType(underlyingType) && !underlyingType.IsEnum)
        {
            return MakeInputNumeric(property, displayName);
        }

        if (underlyingType == typeof(DateTime))
        {
            return MakeDatePicker(property, isRequired);
        }

        if (underlyingType.IsEnum)
        {
            return MakeEnumSelect(property);
        }

        return null;
    }

    private FormValidationRule[] GetFormValidationRuleByType(Type underlyingType)
    {
        if (underlyingType == typeof(string))
        {
            return [new() { Required = true, Type = FormFieldType.String }];
        }

        if (THelper.IsNumericType(underlyingType) && !underlyingType.IsEnum)
        {
            return [new() { Required = true, Type = FormFieldType.Number }];
        }

        if (underlyingType == typeof(DateTime))
        {
            return [new() { Required = true, Type = FormFieldType.Date }];
        }

        if (underlyingType.IsEnum)
        {
            return [new() { Required = true, Transform = v => Enum.IsDefined(underlyingType, v) ? "y" : null }];
        }

        return [new() { Required = true }];
    }

    private object? CreateEventCallback(Type callbackType, object receiver, Action<object> callback)
    {
        var createMethod = typeof(EventCallbackFactory)
            .GetMethod(nameof(EventCallbackFactory.Create), new Type[] { typeof(object), typeof(Action<object>) });

        if (createMethod == null)
        {
            throw new InvalidOperationException("Unable to find EventCallbackFactory.Create method.");
        }

        var eventCallbackType = typeof(EventCallback<>).MakeGenericType(callbackType);

        var eventCallback = Activator.CreateInstance(eventCallbackType, receiver, callback);

        return eventCallback;
    }

    private RenderFragment MakeInputString(PropertyInfo property, string? displayName = null)
    {
        return MakeFormItem(property, typeof(Input<>));
    }

    private RenderFragment MakeInputNumeric(PropertyInfo property, string? displayName = null)
    {
        return MakeFormItem(property, typeof(InputNumber<>));
    }

    private RenderFragment MakeDatePicker(PropertyInfo property, bool isRequired)
    {
        if (isRequired)
        {
            var dateTimeValue = (DateTime)(property.GetValue(Form.Model) ?? DateTime.Now);
            if (dateTimeValue == DateTime.MinValue)
            {
                dateTimeValue = DateTime.Now;
                property.SetValue(Form.Model, dateTimeValue);
            }
        }
        return MakeFormItem(property, typeof(DatePicker<>));
    }

    private RenderFragment MakeEnumSelect(PropertyInfo property)
    {
        return MakeFormItem(property, typeof(EnumSelect<>));
    }

    private RenderFragment MakeFormItem(PropertyInfo property, Type formItemChildContent)
    {
        var constant = Expression.Constant(Form.Model, typeof(TModel));
        var exp = Expression.Property(constant, property.Name);
        var genericType = formItemChildContent;
        var constructedType = genericType.MakeGenericType(property.PropertyType);

        var funcType = typeof(Func<>);
        var constructedFuncType = funcType.MakeGenericType(property.PropertyType);

        return builder =>
        {
            builder.OpenComponent(0, constructedType);

            builder.AddAttribute(1, "Value", property.GetValue(Form.Model));
            var eventCallback = CreateEventCallback(property.PropertyType, this,
                new Action<object>(o => property.SetValue(Form.Model, o)));
            builder.AddAttribute(2, "ValueChanged", eventCallback);
            builder.AddAttribute(3, "ValueExpression", Expression.Lambda(constructedFuncType, exp));
            builder.CloseComponent();
        };
    }
}
