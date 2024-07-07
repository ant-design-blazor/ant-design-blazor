// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using Microsoft.AspNetCore.Components;

namespace AntDesign.Select.Internal
{
    public partial class SelectOptionGroup<TItemValue, TItem>
    {
        private const string ClassNamePrefix = "ant-select-item-group";
        [CascadingParameter] internal SelectBase<TItemValue, TItem> SelectParent { get; set; }
        string _oldGroupName = string.Empty;

        protected override void OnInitialized()
        {
            SetClassMap();
            base.OnInitialized();
        }

        protected void SetClassMap()
        {
            ClassMapper.Clear()
                .Add("ant-select-item")
                .Add(ClassNamePrefix);
        }
    }
}
