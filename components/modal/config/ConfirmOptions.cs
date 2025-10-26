// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Globalization;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using OneOf;

namespace AntDesign
{
    /// <summary>
    /// Confirm dialog options
    /// </summary>
    public class ConfirmOptions : DialogOptionsBase
    {
        public ConfirmOptions()
        {
            Width = 416;
            Mask = true;
            MaskClosable = false;
            Locale = LocaleProvider.CurrentLocale.Confirm;
            OkText = Locale.OkText;
            CancelText = Locale.CancelText;
        }

        #region internal

        /// <summary>
        /// OK
        /// </summary>
        internal string DefaultBtn1Text { get; } = "OK";
        /// <summary>
        /// Cancel
        /// </summary>
        internal string DefaultBtn2Text { get; } = "Cancel";
        /// <summary>
        /// Ignore
        /// </summary>
        internal string DefaultBtn3Text { get; } = "Ignore";

        #endregion

        /// <summary>
        /// Confirm Locale
        /// </summary>
        public ConfirmLocale Locale { get; set; } = LocaleProvider.CurrentLocale.Confirm;

        /// <summary>
        /// the class name of the element of ".ant-modal" 
        /// </summary>
        public string ClassName { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public bool Visible { get; set; }

        /// <summary>
        /// ChildContent
        /// </summary>
        public OneOf<string, RenderFragment> Content { get; set; }

        /// <summary>
        /// Confirm left top icon
        /// </summary>
        public RenderFragment? Icon { get; set; } = null;

        /// <summary>
        /// .ant-modal element's style
        /// </summary>
        public string Style { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public ConfirmAutoFocusButton AutoFocusButton { get; set; } = ConfirmAutoFocusButton.Ok;

        /// <summary>
        /// set OK button type for the leftmost button: OK or Yes button
        /// </summary>
        public new ButtonType OkType
        {
            get
            {
                Button1Props ??= new ButtonProps();
                return Button1Props.Type;
            }

            set
            {
                Button1Props ??= new ButtonProps();
                Button1Props.Type = value;
            }
        }

        #region button text

        /// <summary>
        /// set OK button content for the leftmost button: OK or Yes button, it will override the ConfirmLocale
        /// </summary>
        public new OneOf<string, RenderFragment>? OkText { get => Button1Text; set => Button1Text = value; }

        /// <summary>
        /// set Cancel button content for the second on the left button: Cancel or NO button, it will override the ConfirmLocale
        /// </summary>
        public new OneOf<string, RenderFragment>? CancelText { get => Button2Text; set => Button2Text = value; }

        /// <summary>
        /// the leftmost button in LTR layout, it will override the ConfirmLocale
        /// </summary>
        internal OneOf<string, RenderFragment>? Button1Text { get => Button1Props.ChildContent; set => Button1Props.ChildContent = value; }

        /// <summary>
        /// The second button on the left is in the LTR layout, it will override the ConfirmLocale
        /// </summary>
        internal OneOf<string, RenderFragment>? Button2Text { get => Button2Props.ChildContent; set => Button2Props.ChildContent = value; }

        /// <summary>
        /// the rightmost button in LTR layout, it will override the ConfirmLocale
        /// </summary>
        internal OneOf<string, RenderFragment>? Button3Text { get => Button3Props.ChildContent; set => Button3Props.ChildContent = value; }

        #endregion

        #region button OnClick callback

        /// <summary>
        /// for OK-Cancel Confirm dialog, cancel button clicked callback.
        /// It's only trigger in Confirm created by ModalService mode
        /// </summary>
        public Func<ModalClosingEventArgs, Task> OnCancel { get; set; }

        /// <summary>
        /// for OK-Cancel Confirm dialog, OK button clicked callback.
        /// It's only trigger in Confirm created by ModalService mode
        /// </summary>
        public Func<ModalClosingEventArgs, Task> OnOk { get; set; }

        #endregion

        #region button props

        /// <summary>
        ///  OK-Cancel Confirm dialog's OK button props. It is equivalent to Button1Props.
        /// </summary>
        public new ButtonProps OkButtonProps { get => Button1Props; set => Button1Props = value; }

        /// <summary>
        ///  OK-Cancel Confirm dialog's cancel button props. It is equivalent to Button2Props.
        /// </summary>
        public new ButtonProps CancelButtonProps { get => Button2Props; set => Button2Props = value; }

        /// <summary>
        /// the leftmost button in LTR layout 
        /// </summary>
        public ButtonProps Button1Props
        {
            get => _button1Props;
            set => _button1Props = SetButtonProps(value, _button1Props);
        }

        /// <summary>
        /// The second button on the left is in the LTR layout
        /// </summary>
        public ButtonProps Button2Props
        {
            get => _button2Props;
            set => _button2Props = SetButtonProps(value, _button2Props);
        }

        /// <summary>
        /// the rightmost button in LTR layout
        /// </summary>
        public ButtonProps Button3Props
        {
            get => _button3Props;
            set => _button3Props = SetButtonProps(value, _button3Props);
        }

        private ButtonProps _button1Props = new ButtonProps() { Type = ButtonType.Primary };
        private ButtonProps _button2Props = new ButtonProps();
        private ButtonProps _button3Props = new ButtonProps();

        #endregion

        #region Confirm buttons config

        /// <summary>
        /// show Cancel button for OK-Cancel Confirm dialog
        /// </summary>
        public bool OkCancel
        {
            get => ConfirmButtons != ConfirmButtons.OK;
            set
            {
                ConfirmButtons = !value ? ConfirmButtons.OK : ConfirmButtons.OKCancel;
            }
        }

        internal ConfirmButtons ConfirmButtons { get; set; } = ConfirmButtons.OKCancel;

        #endregion

        #region config confirm icon style

        internal string ConfirmType { get; set; } = "confirm";
        private ConfirmIcon _confirmIcon;
        internal ConfirmIcon ConfirmIcon
        {
            get => _confirmIcon;
            set
            {
                _confirmIcon = value;
                Icon = ConfirmIconRenderFragments.GetByConfirmIcon(value);
                if (value == ConfirmIcon.None)
                {
                    ConfirmType = "confirm";
                }
                else
                {
                    ConfirmType = value.ToString().ToLower(CultureInfo.CurrentUICulture);
                }
            }
        }

        #endregion

        private static ButtonProps SetButtonProps(ButtonProps newProps, ButtonProps oldProps)
        {
            if (newProps == null || newProps.ChildContent.HasValue)
            {
                return newProps;
            }

            if (oldProps != null)
            {
                newProps.ChildContent = oldProps.ChildContent;
            }

            return newProps;
        }

        /// <summary>
        /// set default options for buttons
        /// </summary>
        internal void BuildButtonsDefaultOptions()
        {
            // config default button text
            switch (ConfirmButtons)
            {
                case ConfirmButtons.OK:
                    {
                        this.Button1Text ??= Locale.OkText;
                        break;
                    }
                case ConfirmButtons.OKCancel:
                    {
                        this.Button1Text ??= Locale.OkText;
                        this.Button2Text ??= Locale.CancelText;
                        break;
                    }

                case ConfirmButtons.YesNo:
                case ConfirmButtons.YesNoCancel:
                    {
                        this.Button1Text ??= Locale.YesText;
                        this.Button2Text ??= Locale.NoText;
                        if (ConfirmButtons == ConfirmButtons.YesNoCancel)
                        {
                            this.Button3Text ??= Locale.CancelText;

                            // config button2 default type
                            this.Button2Props.Danger ??= true;
                        }
                        break;
                    }

                case ConfirmButtons.RetryCancel:
                    {
                        this.Button1Text ??= Locale.RetryText;
                        this.Button2Text ??= Locale.CancelText;
                        break;
                    }
                case ConfirmButtons.AbortRetryIgnore:
                    {
                        this.Button1Text ??= Locale.AbortText;
                        this.Button2Text ??= Locale.RetryText;
                        this.Button3Text ??= Locale.IgnoreText;

                        // config button2 default type
                        this.Button2Props.Danger ??= true;
                        break;
                    }
                default:
                    break;
            }
        }
    }

    public class ConfirmOptions<TResult> : ConfirmOptions
    {
        /// <summary>
        /// On dialog open
        /// </summary>
        public Func<Task>? OnOpen { get; set; }

        /// <summary>
        /// On dialog close
        /// </summary>
        public Func<Task>? OnClose { get; set; }

        /// <summary>
        /// On OK click (supports TResult)
        /// </summary>
        public new Func<TResult?, Task>? OnOk { get; set; }

        /// <summary>
        /// On Cancel click (supports TResult)
        /// </summary>
        public new Func<TResult?, Task>? OnCancel { get; set; }
    }
}
