using System;
using System.Threading;
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

        private Timer _timer;

        private const int REFRESH_INTERVAL = 100;

        private TimeSpan _countDown = TimeSpan.Zero;

        protected override void OnInitialized()
        {
            base.OnInitialized();
            SetTimer();
        }

        private void SetTimer()
        {
            _countDown = Value - DateTime.Now;
            _timer = new Timer(StartCountDownForTimeSpan);
            _timer.Change(0, RefreshInterval);
        }

        private void StartCountDownForTimeSpan(object o)
        {
            _countDown = _countDown.Add(TimeSpan.FromMilliseconds(-RefreshInterval));
            if (_countDown.Ticks <= 0)
            {
                _countDown = TimeSpan.Zero;
                _timer.Dispose();
                if (OnFinish.HasDelegate)
                {
                    InvokeAsync(() => OnFinish.InvokeAsync(o));
                }
            }
            InvokeStateHasChanged();
        }

        public void Reset()
        {
            //避免初始化时调用Reset
            if (_timer != null)
            {
                _timer.Dispose();
                SetTimer();
            }
        }

        protected override void Dispose(bool disposing)
        {
            _timer?.Dispose();
            base.Dispose(disposing);
        }
    }
}
