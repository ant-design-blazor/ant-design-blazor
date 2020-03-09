using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;

namespace AntBlazor
{
    public class AntTitleBase : AntDomComponentBase
    {
        [Parameter]
        public int level { get; set; } = 1;
        [Parameter]
        public RenderFragment ChildContent { get; set; }

        protected override void OnInitialized()
        {
            base.OnInitialized();
            SetClassMap();
        }

        protected void SetClassMap()
        {
            ClassMapper.Clear()
                .Add("ant-typography");
            
        }
    }
}
