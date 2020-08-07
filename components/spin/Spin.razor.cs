using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using System.Timers;
using Microsoft.AspNetCore.Components;

namespace AntDesign
{
    public partial class Spin : AntDomComponentBase
    {
        /// <summary>
        /// small | default | large
        /// </summary>
        [Parameter]
        public string Size { get; set; } = "default";

        [Parameter] public string Tip { get; set; } = null;

        [Parameter] public int Delay { get; set; } = 0;

        [Parameter] public bool Spinning { get; set; } = true;

        [Parameter] public string WrapperClassName { get; set; }

        [Parameter] public RenderFragment Indicator { get; set; }

        [Parameter] public RenderFragment ChildContent { get; set; }

        private ClassMapper WrapperClassMapper { get; set; } = new ClassMapper();

        private const string PrefixCls = "ant-spin";

        private bool _isLoading = true;

        private bool Simple => ChildContent == null;

        private string ContainerClass => _isLoading ? $"{PrefixCls}-blur" : "";

        private Timer _delayTimer;

        private void SetClass()
        {
            WrapperClassMapper
                .If(WrapperClassName, () => !string.IsNullOrWhiteSpace(WrapperClassName))
                .If($"{PrefixCls}-nested-loading", () => !Simple);

            ClassMapper.Add(PrefixCls)
                .If($"{PrefixCls}-spinning", () => _isLoading)
                .If($"{PrefixCls}-lg", () => Size == "large")
                .If($"{PrefixCls}-sm", () => Size == "small")
                .If($"{PrefixCls}-show-text", () => string.IsNullOrWhiteSpace(Tip));
        }

        protected override void OnInitialized()
        {
            SetClass();

            _isLoading = Spinning;

            if (Delay > 0)
            {
                _delayTimer = new Timer(Delay);
                _delayTimer.Elapsed += DelayElapsed;
            }

            base.OnInitialized();
        }

        protected override void OnParametersSet()
        {
            base.OnParametersSet();

            if (_delayTimer != null)
            {
                if (_isLoading != Spinning)
                {
                    _delayTimer.Stop();
                    _delayTimer.Start();
                }
                else
                {
                    _delayTimer.Stop();
                }
            }
            else
            {
                if (_isLoading != Spinning)
                {
                    _isLoading = Spinning;
                    InvokeAsync(StateHasChanged);
                }
            }
        }

        private void DelayElapsed(object sender, ElapsedEventArgs args)
        {
            _isLoading = Spinning;
            InvokeAsync(StateHasChanged);
        }
    }
}
