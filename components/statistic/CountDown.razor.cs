// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;

namespace AntDesign
{
    public partial class CountDown : StatisticComponentBase<DateTime>
    {
        /// <summary>
        /// Format of the time
        /// </summary>
        /// <default value="hh:mm:ss"/>
        [Parameter]
        public string Format { get; set; } = "hh:mm:ss";

        /// <summary>
        /// The value of the countdown
        /// </summary>
        public override DateTime Value
        {
            get
            {
                return base.Value;
            }
            set
            {
                if (base.Value != value)
                {
                    base.Value = value;
                    Reset();
                }
            }
        }

        /// <summary>
        /// Callback executed when the countdown runs out
        /// </summary>
        [Parameter]
        public EventCallback OnFinish { get; set; }

        /// <summary>
        /// Interval, in milliseconds, to update the UI on
        /// </summary>
        /// <default value="100ms" />
        [Parameter]
        public int RefreshInterval { get; set; } = REFRESH_INTERVAL;

        private const int REFRESH_INTERVAL = 1000 / 10;

        private TimeSpan _countDown = TimeSpan.Zero;
        private CancellationTokenSource _cts = new();
        private bool _firstAfterRender;

        protected override async Task OnInitializedAsync()
        {
            _countDown = Value - DateTime.Now;
            await base.OnInitializedAsync();
        }

        protected override void OnAfterRender(bool firstRender)
        {
            if (firstRender)
            {
                _firstAfterRender = true;
                _ = StartCountDownForTimeSpan();
            }
            base.OnAfterRender(firstRender);
        }

        private async Task StartCountDownForTimeSpan()
        {
            if (!_firstAfterRender)
            {
                return;
            }
            if (_cts.Token.IsCancellationRequested)
            {
                _cts = new();
            }
            while (!_cts.Token.IsCancellationRequested)
            {
                _countDown = Value - DateTime.Now;

                if (_countDown.Ticks <= 0)
                {
                    _countDown = TimeSpan.Zero;
                    if (OnFinish.HasDelegate)
                    {
                        await OnFinish.InvokeAsync(this);
                    }
                    _cts.Cancel();
                    break;
                }

                InvokeStateHasChanged();
                await Task.Delay(RefreshInterval, _cts.Token);
            }
        }

        /// <summary>
        /// Reset the countdown
        /// </summary>
        public void Reset()
        {
            _ = StartCountDownForTimeSpan();
        }

        protected override void Dispose(bool disposing)
        {
            _cts?.Cancel();
            base.Dispose(disposing);
        }
    }
}
