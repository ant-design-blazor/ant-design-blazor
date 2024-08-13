// Licensed to the .NET Foundation under one or more agreements.
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

    [CascadingParameter(Name = "Form")] private IForm Form { get; set; }

    [Parameter] public Func<string, FormValidationRule[]?>? ValidateRules { get; set; }

    [Parameter] public Func<string, RenderFragment?>? Definitions { get; set; }

    [Parameter] public Func<string, bool>? NotGenerate { get; set; }

    [Parameter] public string SubformStyle { get; set; } = "Collapse";

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
            if (NotGenerate != null && NotGenerate.Invoke(property.Name))
            {
                continue;
            }

            GenerateByType(property, builder, Form.Model, property.Name);
        }
    }

    private void MakeFormItemBlock(PropertyInfo property, FormValidationRule[]? validateRule,
        RenderFragment? formItemFragment,
        RenderTreeBuilder builder, int sequenceIndex)
    {
        var displayName = property.GetCustomAttribute<DisplayNameAttribute>()?.DisplayName ?? property.Name;
        if (validateRule != null)
        {
            builder.OpenComponent(sequenceIndex++, typeof(FormItem));
            builder.AddAttribute(sequenceIndex++, "Label", displayName);
            builder.AddAttribute(sequenceIndex++, "Rules", validateRule);
            builder.AddAttribute(sequenceIndex++, "ChildContent", formItemFragment);
            builder.CloseComponent();
        }
        else
        {
            builder.OpenComponent(sequenceIndex++, typeof(FormItem));
            builder.AddAttribute(sequenceIndex++, "Label", displayName);
            builder.AddAttribute(sequenceIndex++, "ChildContent", formItemFragment);
            builder.CloseComponent();
        }
    }

    private void GenerateByType(PropertyInfo property, RenderTreeBuilder builder, object model, string structurePath)
    {
        var sequenceIndex = 0;

        FormValidationRule[]? validateRule = null;
        if (ValidateRules != null)
        {
            validateRule = ValidateRules.Invoke(structurePath);
        }

        var definitionsFormItem = Definitions?.Invoke(structurePath);
        if (definitionsFormItem != null)
        {
            MakeFormItemBlock(property, validateRule, definitionsFormItem, builder, sequenceIndex);
            return;
        }

        var underlyingType = THelper.GetUnderlyingType(property.PropertyType);
        if (underlyingType == typeof(string))
        {
            var formItemFragment = MakeInputString(property, model);
            MakeFormItemBlock(property, validateRule, formItemFragment, builder, sequenceIndex);
        }

        if (THelper.IsNumericType(underlyingType) && !underlyingType.IsEnum)
        {
            var formItemFragment = MakeInputNumeric(property, model);
            MakeFormItemBlock(property, validateRule, formItemFragment, builder, sequenceIndex);
        }

        if (underlyingType == typeof(DateTime))
        {
            var formItemFragment = MakeDatePicker(property, model);
            MakeFormItemBlock(property, validateRule, formItemFragment, builder, sequenceIndex);
        }

        if (underlyingType.IsEnum)
        {
            var formItemFragment = MakeEnumSelect(property, model);
            MakeFormItemBlock(property, validateRule, formItemFragment, builder, sequenceIndex);
        }

        if (underlyingType.IsUserDefinedClass() && !underlyingType.IsArrayOrList())
        {
            if (SubformStyle == "Block")
            {
                MakeSubformWithBlockStyle(property, builder, model, structurePath, underlyingType, sequenceIndex);
            }
            else if (SubformStyle == "Collapse")
            {
                MakeSubformWithCollapseStyle(property, builder, model, structurePath, underlyingType, sequenceIndex);
            }
        }
    }

    private RenderFragment GenerateByTypeResult(PropertyInfo property, RenderTreeBuilder builder, object model,
        string structurePath)
    {
        return builder =>
        {
            var sequenceIndex = 0;

            FormValidationRule[]? validateRule = null;
            if (ValidateRules != null)
            {
                validateRule = ValidateRules.Invoke(structurePath);
            }

            var definitionsFormItem = Definitions?.Invoke(structurePath);
            if (definitionsFormItem != null)
            {
                MakeFormItemBlock(property, validateRule, definitionsFormItem, builder, sequenceIndex);
                return;
            }

            var underlyingType = THelper.GetUnderlyingType(property.PropertyType);
            if (underlyingType == typeof(string))
            {
                var formItemFragment = MakeInputString(property, model);
                MakeFormItemBlock(property, validateRule, formItemFragment, builder, sequenceIndex);
            }

            if (THelper.IsNumericType(underlyingType) && !underlyingType.IsEnum)
            {
                var formItemFragment = MakeInputNumeric(property, model);
                MakeFormItemBlock(property, validateRule, formItemFragment, builder, sequenceIndex);
            }

            if (underlyingType == typeof(DateTime))
            {
                var formItemFragment = MakeDatePicker(property, model);
                MakeFormItemBlock(property, validateRule, formItemFragment, builder, sequenceIndex);
            }

            if (underlyingType.IsEnum)
            {
                var formItemFragment = MakeEnumSelect(property, model);
                MakeFormItemBlock(property, validateRule, formItemFragment, builder, sequenceIndex);
            }

            if (underlyingType.IsUserDefinedClass() && !underlyingType.IsArrayOrList())
            {
                if (SubformStyle == "Block")
                {
                    MakeSubformWithBlockStyle(property, builder, model, structurePath, underlyingType, sequenceIndex);
                }
                else if (SubformStyle == "Collapse")
                {
                    MakeSubformWithCollapseStyle(property, builder, model, structurePath, underlyingType,
                        sequenceIndex);
                }
            }
        };
    }

    private void MakeSubformWithBlockStyle(PropertyInfo property, RenderTreeBuilder builder, object model,
        string structurePath, Type underlyingType, int sequenceIndex)
    {
        var childModel = property.GetValue(model);
        if (childModel == null) return;
        var propertyInfos = underlyingType.GetProperties();
        var displayName = property.GetCustomAttribute<DisplayNameAttribute>()?.DisplayName ?? property.Name;
        builder.OpenElement(sequenceIndex++, "div");
        builder.AddAttribute(sequenceIndex++, "style",
            "border: 1px solid #9e9e9e;padding: 16px;position: relative;border-radius: 2px;");
        builder.OpenElement(sequenceIndex++, "div");
        builder.AddAttribute(sequenceIndex++, "style", "position: absolute;top: -12px;background: #fff;");
        builder.AddContent(sequenceIndex++, displayName);
        builder.CloseElement();
        foreach (var propertyInfo in propertyInfos)
        {
            // Not Generate
            var itemStructurePath = $"{structurePath}.{propertyInfo.Name}";
            if (NotGenerate != null && NotGenerate.Invoke(itemStructurePath))
            {
                continue;
            }

            GenerateByType(propertyInfo, builder, childModel, itemStructurePath);
        }

        builder.CloseElement();
    }

    private void MakeSubformWithCollapseStyle(PropertyInfo property, RenderTreeBuilder builder, object model,
        string structurePath, Type underlyingType, int sequenceIndex)
    {
        var childModel = property.GetValue(model);
        if (childModel == null) return;
        var propertyInfos = underlyingType.GetProperties();
        var displayName = property.GetCustomAttribute<DisplayNameAttribute>()?.DisplayName ?? property.Name;
        builder.OpenComponent(sequenceIndex++, typeof(Collapse));
        RenderFragment panel = panelBuilder =>
        {
            var panelIndex = 0;
            panelBuilder.OpenComponent(panelIndex++, typeof(Panel));
            panelBuilder.AddAttribute(panelIndex++, "header", displayName);
            RenderFragment subform = subformBuilder =>
            {
                foreach (var propertyInfo in propertyInfos)
                {
                    // Not Generate
                    var itemStructurePath = $"{structurePath}.{propertyInfo.Name}";
                    if (NotGenerate != null && NotGenerate.Invoke(itemStructurePath))
                    {
                        continue;
                    }

                    GenerateByType(propertyInfo, subformBuilder, childModel, itemStructurePath);
                }
            };
            panelBuilder.AddAttribute(panelIndex++, "ChildContent", subform);
            panelBuilder.CloseComponent();
        };
        builder.AddAttribute(sequenceIndex++, "ChildContent", panel);

        builder.CloseComponent();
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

    private RenderFragment MakeInputString(PropertyInfo property, object model)
    {
        return MakeFormItem(property, typeof(Input<>), model);
    }

    private RenderFragment MakeInputNumeric(PropertyInfo property, object model)
    {
        return MakeFormItem(property, typeof(InputNumber<>), model);
    }

    private RenderFragment MakeDatePicker(PropertyInfo property, object model)
    {
        return MakeFormItem(property, typeof(DatePicker<>), model);
    }

    private RenderFragment MakeEnumSelect(PropertyInfo property, object model)
    {
        return MakeFormItem(property, typeof(EnumSelect<>), model);
    }

    private RenderFragment MakeFormItem(PropertyInfo property, Type formItemChildContent, object model)
    {
        var constant = Expression.Constant(model, model.GetType());
        var exp = Expression.Property(constant, property.Name);
        var genericType = formItemChildContent;
        var constructedType = genericType.MakeGenericType(property.PropertyType);

        var funcType = typeof(Func<>);
        var constructedFuncType = funcType.MakeGenericType(property.PropertyType);

        return builder =>
        {
            builder.OpenComponent(0, constructedType);
            builder.AddAttribute(1, "Value", property.GetValue(model));
            var eventCallback = CreateEventCallback(property.PropertyType, this,
                new Action<object>(o => property.SetValue(model, o)));
            builder.AddAttribute(2, "ValueChanged", eventCallback);
            builder.AddAttribute(3, "ValueExpression", Expression.Lambda(constructedFuncType, exp));
            builder.CloseComponent();
        };
    }
}
