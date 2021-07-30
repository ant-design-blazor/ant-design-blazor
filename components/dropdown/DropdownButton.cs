﻿using System;
using System.Text.Json;
using System.Threading.Tasks;
using AntDesign.JsInterop;
using Microsoft.AspNetCore.Components;
using OneOf;

namespace AntDesign
{
    public class DropdownButton : Dropdown
    {
        /// <summary>
        /// Option to fit button width to its parent width
        /// </summary>
        [Parameter]
        public new bool Block
        {
            get => base.Block;
            set => base.Block = value;
        }

        /// <summary>
        /// Fully customizable button.
        /// </summary>
        [Parameter]
        public new Func<RenderFragment, RenderFragment, RenderFragment> ButtonsRender
        {
            get => base.ButtonsRender;
            set => base.ButtonsRender = value;
        }

        /// <summary>
        /// Allows to set each button's css class either to the same string
        /// or separately.
        /// </summary>
        [Parameter]
        public OneOf<string, (string LeftButton, string RightButton)> ButtonsClass
        {
            get =>_buttonsClass; 
            set
            {
                _buttonsClass = value;
                _buttonsClass.Switch(
                    single =>
                    {
                        ChangeButtonClass(single, single);
                    },
                    both =>
                    {
                        ChangeButtonClass(both.LeftButton, both.RightButton);
                    });
            }
        }

        /// <summary>
        /// Allows to set each button's style either to the same string
        /// or separately. 
        /// </summary>
        [Parameter]
        public OneOf<string, (string LeftButton, string RightButton)> ButtonsStyle
        {
            get => _buttonsStyle;
            set
            {
                _buttonsStyle = value;
                _buttonsStyle.Switch(
                    single =>
                    {
                        ChangeButtonStyles(single, single);
                    },
                    both =>
                    {
                        ChangeButtonStyles(both.LeftButton, both.RightButton);
                    });
            }
        }

        /// <summary>
        /// Set the danger status of button
        /// </summary>
        [Parameter]
        public bool Danger
        {
            get => _danger;
            set
            {
                _danger = value;
                ChangeButtonDanger(value);
            }
        }

        /// <summary>
        /// Used in situations with complex background, home pages usually.
        /// </summary>
        [Parameter]
        public bool Ghost
        {
            get => _ghost;
            set
            {
                _ghost = value;
                ChangeButtonGhost(value);
            }
        }

        private string _icon = "ellipsis";
        /// <summary>
        /// Icon that will be rendered in the right
        /// button.
        /// </summary>
        [Parameter]
        public string Icon
        {
            get => _icon;
            set
            {
                _icon = value;
                ChangeRightButtonIcon(value);
            }
        }

        /// <summary>
        /// Indicates if loading icon is going to be included. 
        /// If set to true, then dropdown will not be active.
        /// </summary>
        [Parameter]
        public bool Loading
        {
            get => _loading;
            set
            {
                _loading = value;
                ChangeButtonLoading(value);
            }
        }

        private string _size = AntSizeLDSType.Default;
        /// <summary>
        /// Button size.
        /// </summary>
        [Parameter]
        public string Size
        {
            get => _size;
            set
            {
                _size = value;
                ChangeButtonSize(value);
            }
        }

        /// <summary>
        /// Allows to set each button's type either to the same string
        /// or separately. Use AntDesign.ButtonType helper class.
        /// </summary>
        [Parameter]
        public OneOf<string, (string LeftButton, string RightButton)> Type

        {
            get => _buttonsType;
            set
            {
                _buttonsType = value;
                _buttonsType.Switch(
                    single =>
                    {
                        ChangeButtonType(single, single);
                    },
                    both =>
                    {
                        ChangeButtonType(both.LeftButton, both.RightButton);
                    });
            }
        }

        private bool _loading;
        private bool _danger;
        private bool _ghost = false;
        private OneOf<string, (string LeftButton, string RightButton)> _buttonsStyle;
        private OneOf<string, (string LeftButton, string RightButton)> _buttonsClass;
        private OneOf<string, (string LeftButton, string RightButton)> _buttonsType = ButtonType.Default;

        public DropdownButton() => IsButton = true;

        protected override void OnInitialized()
        {
            string prefixCls = "ant-btn";
            ClassMapper.Clear();
            Placement = PlacementType.BottomRight;
            base.OnInitialized();
            ClassMapper.If($"{prefixCls}-block", () => Block);

        }

        /// <summary>
        /// Force overlay trigger to be attached to wrapping element of 
        /// the right button. Right button has to be wrapped, 
        /// because overlay will be looking for first child
        /// element of the overlay trigger to calculate the overlay position.
        /// If the right button was the trigger, then its first child
        /// would be the icon/ellipsis and the overlay would have been 
        /// rendered too high.
        /// </summary>
        /// <param name="firstRender"></param>
        /// <returns></returns>
        protected override Task OnAfterRenderAsync(bool firstRender)
        {
            if (firstRender)
            {
                Ref = RefBack.Current;
                DomEventService.AddEventListener(Ref, "click", OnUnboundClick, true);
                DomEventService.AddEventListener(Ref, "mouseover", OnUnboundMouseEnter, true);
                DomEventService.AddEventListener(Ref, "mouseout", OnUnboundMouseLeave, true);
                DomEventService.AddEventListener(Ref, "focusin", OnUnboundFocusIn, true);
                DomEventService.AddEventListener(Ref, "focusout", OnUnboundFocusOut, true);
                DomEventService.AddEventListener(Ref, "contextmenu", OnContextMenu, true, true);
            }
            return base.OnAfterRenderAsync(firstRender);
        }

        protected override void Dispose(bool disposing)
        {
            DomEventService.RemoveEventListerner<JsonElement>(Ref, "click", OnUnboundClick);
            DomEventService.RemoveEventListerner<JsonElement>(Ref, "mouseover", OnUnboundMouseEnter);
            DomEventService.RemoveEventListerner<JsonElement>(Ref, "mouseout", OnUnboundMouseLeave);
            DomEventService.RemoveEventListerner<JsonElement>(Ref, "focusin", OnUnboundFocusIn);
            DomEventService.RemoveEventListerner<JsonElement>(Ref, "focusout", OnUnboundFocusOut);
            DomEventService.RemoveEventListerner<JsonElement>(Ref, "contextmenu", OnContextMenu);

            base.Dispose(disposing);
        }

        internal override async Task Show(int? overlayLeft = null, int? overlayTop = null)
        {
            if (!Loading)
            {
                await _overlay.Show(overlayLeft, overlayTop);
            }
        }
    }
}
