using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Rendering;
using System;
using System.Collections.Generic;
using System.Text;

namespace AntDesign
{
    public static class RenderFragmentHelper
    {
        public static RenderFragment ToRenderFragment(this string value) => builder => builder.AddContent(1, value);

        public static RenderFragment CreateChildContent<T>(IDictionary<string, object> extendedProperties = null) where T : IComponent
        {
            return Foo;
            void Foo(RenderTreeBuilder builder)
            {
                builder.OpenComponent<T>(0);
                if (extendedProperties != null)
                {
                    int num = 1;
                    foreach (KeyValuePair<string, object> extendedProperty in extendedProperties)
                    {
                        builder.AddAttribute(num++, extendedProperty.Key, extendedProperty.Value);
                    }
                }

                builder.CloseComponent();
            }
        }

    }
}
