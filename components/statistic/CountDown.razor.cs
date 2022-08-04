using System;
using System.Text.RegularExpressions;
using System.Threading;
using Microsoft.AspNetCore.Components;

namespace AntDesign
{
    public partial class CountDown : StatisticComponentBase<DateTime>
    {
        [Parameter]
        public string Format { get; set; } = "hh:mm:ss";


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

        [Parameter]
        public EventCallback OnFinish { get; set; }

        private Timer _timer;

        private const int REFRESH_INTERVAL = 1000 / 30;

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
            _timer.Change(0, REFRESH_INTERVAL);
        }

        private void StartCountDownForTimeSpan(object o)
        {
            _countDown = _countDown.Add(TimeSpan.FromMilliseconds(-REFRESH_INTERVAL));
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
            if(_timer!=null)
            {
                _timer.Dispose();
                SetTimer();
            }
        }
        protected override void Dispose(bool disposing)
        {
            _timer.Dispose();
            base.Dispose(disposing);
        }
    }
}
