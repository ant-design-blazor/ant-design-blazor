﻿using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using OneOf;

namespace AntBlazor
{
    public class ConfirmOptions
    {
        public string ClassName { get; set; }

        public bool Visible { get; set; }

        public OneOf<string, RenderFragment>? Title { get; set; } = null;

        public OneOf<string, RenderFragment> Content { get; set; }

        public EventCallback<MouseEventArgs>? OnCancel { get; set; }

        public EventCallback<MouseEventArgs>? OnOk { get; set; }

        public ButtonProps OkButtonProps { get; set; } = new ButtonProps(){ Type = AntButtonType.Primary};

        public ButtonProps CancelButtonProps { get; set; } = new ButtonProps();

        public OneOf<string, double> Width { get; set; } = 520;

        public bool Centered { get; set; }

        public bool Mask { get; set; } = true;
        public bool MaskClosable { get; set; } = true;

        public string MaskStyle { get; set; }

        public OneOf<string, RenderFragment> OkText { get; set; } = "确定";

        public string OkType { get; set; } = AntButtonType.Primary;

        public OneOf<string, RenderFragment> CancelText { get; set; } = "取消";

        public RenderFragment? Icon { get; set; } = null;

        public int ZIndex { get; set; } = 1000;

        /// <summary>
        /// 显示Cancel按钮
        /// </summary>
        public bool OkCancel { get; set; } = true;

        public string Style { get; set; }

        public bool Keyboard { get; set; }

        public ConfirmAutoFocusButton AutoFocusButton { get; set; }

        public string TransitionName { get; set; }

        public string MaskTransitionName { get; set; }

        public ElementReference? GetContainer { get; set; } = null;

        internal string ConfirmType { get; set; } = "confirm";
    }
}
