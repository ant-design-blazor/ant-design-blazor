// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Text.Json;
using System.Threading.Tasks;
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
            get => _buttonsClass;
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
        /// <default value="false" />
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
        /// <default value="false" />
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
        /// <default value="ellipsis" />
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
        /// <default value="false" />
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

        private ButtonSize _size = ButtonSize.Default;

        /// <summary>
        /// Button size.
        /// </summary>
        /// <default value="AntSizeLDSType.Default" />
        [Parameter]
        public ButtonSize Size
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
        /// <default value="ButtonType.Default" />
        [Parameter]
        public OneOf<ButtonType, (ButtonType LeftButton, ButtonType RightButton)> Type

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
        private OneOf<ButtonType, (ButtonType LeftButton, ButtonType RightButton)> _buttonsType = ButtonType.Default;

        public DropdownButton() => IsButton = true;

        protected override void OnInitialized()
        {
            string prefixCls = "ant-btn";
            ClassMapper.Clear();
            Placement = Placement.BottomRight;
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

                DomEventListener.AddExclusive<JsonElement>(Ref, "click", OnUnboundClick);
                DomEventListener.AddExclusive<JsonElement>(Ref, "mouseover", OnUnboundMouseEnter);
                DomEventListener.AddExclusive<JsonElement>(Ref, "mouseout", OnUnboundMouseLeave);
                DomEventListener.AddExclusive<JsonElement>(Ref, "focusin", OnUnboundFocusIn);
                DomEventListener.AddExclusive<JsonElement>(Ref, "focusout", OnUnboundFocusOut);
                DomEventListener.AddExclusive<JsonElement>(Ref, "contextmenu", OnContextMenu, true);
            }
            return base.OnAfterRenderAsync(firstRender);
        }

        protected override void Dispose(bool disposing)
        {
            DomEventListener.DisposeExclusive();
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
