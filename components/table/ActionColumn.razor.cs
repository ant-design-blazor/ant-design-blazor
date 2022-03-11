﻿using AntDesign.TableModels;
using Microsoft.AspNetCore.Components;

namespace AntDesign
{
    public partial class ActionColumn : ColumnBase
    {
        [CascadingParameter(Name = "AntDesign.Column.Blocked")]
        public bool Blocked { get; set; }

        [Parameter]
        public virtual RenderFragment<CellData> CellRender { get; set; }

        protected override void OnInitialized()
        {
            base.OnInitialized();
            if (IsHeader)
            {
                Context.HeaderColumnInitialed(this);
            }
        }

        protected override bool ShouldRender()
        {
            if (Blocked) return false;
            return true;
        }
    }
}
