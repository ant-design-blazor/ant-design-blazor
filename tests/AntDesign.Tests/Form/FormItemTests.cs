// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Collections.Generic;
using AntDesign.JsInterop;
using Bunit;
using Microsoft.AspNetCore.Components;
using Xunit;

namespace AntDesign.Tests.Form.Validation
{
    public class FormItemTests : AntDesignTestBase
    {
        public FormItemTests() : base()
        {
        }

        [Theory]
        [MemberData(nameof(RequiredMarkTestData))]
        public void ItShouldRenderProperRequiredMark(bool isRequired, FormRequiredMark requiredMark, string expectedMarkup)
        {
            JSInterop.Setup<Window>("AntDesign.interop.domInfoHelper.getWindow", _ => true).SetResult(new Window
            {
                InnerHeight = 1000m,
                InnerWidth = 1000m
            });

            RenderFragment<object> fragment = model => builder =>
            {
                builder.OpenComponent<FormItem>(0);

                builder.AddAttribute(1, "Label", "Test Label");

                var validationRules = new FormValidationRule[]
                {
                    new()
                    {
                        Required = isRequired
                    }
                };

                builder.AddAttribute(2, "Rules", validationRules);

                builder.CloseComponent();
            };

            var wrappedSystemUnderTest = RenderComponent<Form<object>>(parameters => parameters
                .Add(x => x.ChildContent, fragment)
                .Add(x => x.RequiredMark, requiredMark)
                .Add(x => x.ValidateMode, FormValidateMode.Rules)
                .Add(x => x.Model, new { }));

            wrappedSystemUnderTest.WaitForAssertion(
                () => wrappedSystemUnderTest
                .FindComponent<FormItem>()
                .Find("label")
                .MarkupMatches(expectedMarkup));
        }

        public static IEnumerable<object[]> RequiredMarkTestData()
        {
            yield return new object[]{
                true,
                FormRequiredMark.Optional,
                "<label class=\"\">Test Label</label>"
            };

            yield return new object[]{
                true,
                FormRequiredMark.Required,
                "<label class=\"ant-form-item-required\">Test Label</label>"
            };

            yield return new object[]{
                true,
                FormRequiredMark.None,
                "<label class=\"\">Test Label</label>"
            };

            yield return new object[]{
                false,
                FormRequiredMark.Optional,
                "<label class=\"\">Test Label<span class=\"ant-form-item-optional\">(optional)</span></label>"
            };

            yield return new object[]{
                false,
                FormRequiredMark.Required,
                "<label class=\"\">Test Label</label>"
            };

            yield return new object[]{
                false,
                FormRequiredMark.None,
                "<label class=\"\">Test Label</label>"
            };
        }
    }
}
