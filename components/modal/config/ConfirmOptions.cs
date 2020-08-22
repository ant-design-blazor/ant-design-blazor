using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using OneOf;

namespace AntDesign
{
    public class ConfirmOptions
    {
        /// <summary>
        /// OK
        /// </summary>
        internal static string DefaultBtn1Text = "OK";
        /// <summary>
        /// Cancel
        /// </summary>
        internal static string DefaultBtn2Text = "Cancel";
        /// <summary>
        /// Ignore
        /// </summary>
        internal static string DefaultBtn3Text = "Ignore";


        public string ClassName { get; set; }

        public bool Visible { get; set; }

        public OneOf<string, RenderFragment>? Title { get; set; } = null;

        public OneOf<string, RenderFragment> Content { get; set; }

        public OneOf<string, double> Width { get; set; } = 520;

        public bool Centered { get; set; }

        public bool Mask { get; set; } = true;

        public bool MaskClosable { get; set; } = false;

        public string MaskStyle { get; set; }

        public RenderFragment? Icon { get; set; } = null;

        public int ZIndex { get; set; } = 1000;

        public string Style { get; set; }

        public bool Keyboard { get; set; } = true;

        public string TransitionName { get; set; }

        public string MaskTransitionName { get; set; }

        public ElementReference? GetContainer { get; set; } = null;


        public ConfirmAutoFocusButton AutoFocusButton { get; set; }

        /// <summary>
        /// set OK button type for the leftmost button: OK or Yes button
        /// </summary>
        public string OkType
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
        /// set OK button content for the leftmost button: OK or Yes button
        /// </summary>
        public OneOf<string, RenderFragment> OkText { get => Button1Props.ChildContent; set => Button1Props.ChildContent = value; }

        public OneOf<string, RenderFragment> CancelText { get => Button2Props.ChildContent; set => Button2Props.ChildContent = value; }

        /// <summary>
        /// the leftmost button in LTR layout
        /// </summary>
        internal OneOf<string, RenderFragment> Button1Text { get => Button1Props.ChildContent; set => Button1Props.ChildContent = value; }

        /// <summary>
        /// the secondary button in LTR layout
        /// </summary>
        internal OneOf<string, RenderFragment> Button2Text { get => Button2Props.ChildContent; set => Button2Props.ChildContent = value; }

        /// <summary>
        /// the rightmost button in LTR layout
        /// </summary>
        internal OneOf<string, RenderFragment> Button3Text { get => Button3Props.ChildContent; set => Button3Props.ChildContent = value; }

        #endregion

        #region button OnClick callback

        public Func<ModalClosingEventArgs, Task> OnCancel { get; set; }
        public Func<ModalClosingEventArgs, Task> OnOk { get; set; }

        #endregion

        #region button props

        public ButtonProps OkButtonProps { get => Button1Props; set => Button1Props = value; }
        public ButtonProps CancelButtonProps { get => Button1Props; set => Button2Props = value; }

        public ButtonProps Button1Props
        {
            get => _button1Props;
            set
            {
                _button1Props = value;
                if (_button1Props != null && _button1Props.ChildContent.IsT0 && string.IsNullOrWhiteSpace(_button1Props.ChildContent.AsT0))
                {
                    _button1Props.ChildContent = DefaultBtn1Text;
                }
            }
        }
        public ButtonProps Button2Props
        {
            get => _button2Props;
            set
            {
                _button2Props = value;
                if (_button2Props != null && _button2Props.ChildContent.IsT0 && string.IsNullOrWhiteSpace(_button2Props.ChildContent.AsT0))
                {
                    _button2Props.ChildContent = DefaultBtn2Text;
                }
            }
        }

        public ButtonProps Button3Props
        {
            get => _button3Props;
            set
            {
                _button3Props = value;
                if (_button3Props != null && _button3Props.ChildContent.IsT0 && string.IsNullOrWhiteSpace(_button3Props.ChildContent.AsT0))
                {
                    _button3Props.ChildContent = DefaultBtn3Text;
                }
            }
        }

        private ButtonProps _button1Props = new ButtonProps() { Type = ButtonType.Primary, ChildContent = DefaultBtn1Text };
        private ButtonProps _button2Props = new ButtonProps() { ChildContent = DefaultBtn2Text };
        private ButtonProps _button3Props = new ButtonProps() { ChildContent = DefaultBtn3Text };

        #endregion

        #region Confirm buttons config

        /// <summary>
        /// show Cancel button?
        /// </summary>
        public bool OkCancel
        {
            get => ConfirmButtons != ConfirmButtons.OK;
            set
            {
                if (!value)
                {
                    ConfirmButtons = ConfirmButtons.OK;
                }
                else
                {
                    ConfirmButtons = ConfirmButtons.OKCancel;
                }
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
                    ConfirmType = value.ToString().ToLower();
                }
            }
        }

        #endregion

        /// <summary>
        /// set default options for buttons
        /// </summary>
        internal void BuildButtonsDefaultOptions()
        {
            // config default button text
            switch (ConfirmButtons)
            {
                case ConfirmButtons.YesNo:
                case ConfirmButtons.YesNoCancel:
                    {
                        if (this.Button1Text.IsT0)
                        {
                            var text = this.Button1Text.AsT0;
                            if (text == ConfirmOptions.DefaultBtn1Text)
                            {
                                this.Button1Text = "Yes";
                            }
                        }
                        if (this.Button2Text.IsT0)
                        {
                            var text = this.Button2Text.AsT0;
                            if (text == ConfirmOptions.DefaultBtn2Text)
                            {
                                this.Button2Text = "No";
                            }
                        }
                        if (ConfirmButtons == ConfirmButtons.YesNoCancel)
                        {
                            if (this.Button3Text.IsT0)
                            {
                                var text = this.Button3Text.AsT0;
                                if (text == ConfirmOptions.DefaultBtn3Text)
                                {
                                    this.Button3Text = "Cancel";
                                }
                            }
                            // config button2 defult type
                            if (this.Button2Props.Danger == null)
                            {
                                this.Button2Props.Danger = true;
                            }
                        }
                        break;
                    }

                case ConfirmButtons.RetryCancel:
                    {
                        if (this.Button1Text.IsT0 && this.Button1Text.AsT0 == ConfirmOptions.DefaultBtn1Text)
                        {
                            this.Button1Text = "Retry";
                        }
                        break;
                    }
                case ConfirmButtons.AbortRetryIgnore:
                    {
                        if (this.Button1Text.IsT0 && this.Button1Text.AsT0 == ConfirmOptions.DefaultBtn1Text)
                        {
                            this.Button1Text = "Abort";
                        }
                        if (this.Button2Text.IsT0 && this.Button2Text.AsT0 == ConfirmOptions.DefaultBtn2Text)
                        {
                            this.Button2Text = "Retry";
                        }
                        // config button2 defult type
                        if (this.Button2Props.Danger == null)
                        {
                            this.Button2Props.Danger = true;
                        }
                        break;
                    }
                default:
                    break;
            }
        }
    }
}
