// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Reflection;
using System.Text;
using AntDesign.Core.Reflection;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;

namespace AntDesign.Form.Dynamic
{
    public class FormField<TEntity> where TEntity : class, new()
    {
        protected readonly Form<TEntity> Form;

        private PropertyReflector _propertyReflector;

        public PropertyInfo PropertyInfo { get; }

        internal Type PropertyType => PropertyInfo.PropertyType;

        internal event EventHandler? ValueChanged;

        internal string EditorId => Form.Id + '_' + PropertyInfo.Name;

        internal TEntity Owner => Form.Model;

        internal string Label => _propertyReflector.DisplayName ?? string.Empty;

        internal object? Value
        {
            get => PropertyInfo.GetValue(Owner);
            set
            {
                if (PropertyInfo.SetMethod is not null && !Equals(Value, value))
                {
                    PropertyInfo.SetValue(Owner, value);
                    ValueChanged?.Invoke(this, EventArgs.Empty);
                }
            }
        }

        public IDictionary<string, object> InputComponentAttributes
        {
            get
            {
                var expressionContainer = InputComponentExpressionContainer.Create(this);

                var attributes = new Dictionary<string, object?>
                {
                    { "id", EditorId },
                    { "Value", Value },
                    { "autofocus", true },
                    { "required", _propertyReflector.RequiredAttribute is not null },
                    //{ "class", PropertyInfo.InputClass() ?? Form.FieldCssClassProvider?.Input(this) },
                    { "ValueExpression", expressionContainer.ValueExpression }
                };

                if (PropertyInfo.IsEditable())
                {
                    attributes["ValueChanged"] = expressionContainer.ValueChanged;
                }
                else
                {
                    attributes["disabled"] = "true";
                    attributes["readonly"] = "true";
                }

                //if (!string.IsNullOrEmpty(PropertyInfo.DataListName()))
                //    attributes["list"] = PropertyInfo.DataListName();

                //if (!string.IsNullOrEmpty(PropertyInfo.Placeholder()))
                //    attributes["placeholder"] = PropertyInfo.Placeholder();

                if (PropertyInfo.RadioAttribute() is not null)
                {
                    // TODO implement this
                    // attributes["Field"] = this;
                    // attributes["FieldCssClassProvider"] = Form.FieldCssClassProvider;
                    // attributes["ValidationCssClassProvider"] = Form.ValidationCssClassProvider;

                    throw new InvalidOperationException("RadioAttribute is not implemented yet.");
                }

                if (PropertyInfo.IsCheckbox())
                {
                    attributes["role"] = "switch";
                }
                else if (PropertyInfo.RangeAttribute() is { Minimum: var min, Maximum: var max })
                {
                    attributes["min"] = min;
                    attributes["max"] = max;
                }

                if (PropertyInfo.GetHtmlInputType() is { } htmlInputType)
                    attributes["type"] = htmlInputType;

                return attributes!;
            }
        }

        internal RenderFragment InputComponentTemplate
        {
            get
            {
                return builder =>
                {
                    var inputComponentType = PropertyInfo.GetInputComponentType();

                    builder.OpenComponent(0, inputComponentType);
                    builder.AddMultipleAttributes(1, InputComponentAttributes);
                    builder.CloseComponent();
                };
            }
        }

        internal RenderFragment ValidationMessage
        {
            get
            {
                return builder =>
                {
                    //var expressionContainer = ValidationMessageExpressionContainer.Create(this);

                    builder.OpenComponent(0, typeof(ValidationMessage<>).MakeGenericType(PropertyType));
                    //builder.AddAttribute(1, "For", expressionContainer.Lambda);
                    builder.CloseComponent();
                };
            }
        }

        internal static IEnumerable<FormField<TEntity>> FromForm(Form<TEntity> form)
        {
            return typeof(TEntity)
                .GetProperties(BindingFlags.Public | BindingFlags.Instance | BindingFlags.FlattenHierarchy)
                .Where(prop => prop.GetCustomAttribute<NotMappedAttribute>() is null)
                .Select(prop => new FormField<TEntity>(form, prop));
        }

        protected FormField(Form<TEntity> form, PropertyInfo propertyInfo)
        {
            Form = form;
            PropertyInfo = propertyInfo;
            _propertyReflector = new PropertyReflector(propertyInfo);
        }
    }
}
