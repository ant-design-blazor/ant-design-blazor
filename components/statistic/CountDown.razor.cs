using System;
using System.Threading;
using Microsoft.AspNetCore.Components;

namespace AntDesign
{
    public partial class CountDown : StatisticComponentBase<DateTime>
    {
        [Parameter] public string Format { get; set; } = @"hh\:mm\:ss";

        [Parameter] public EventCallback OnFinish { get; set; }

        private Timer _timer;

        private const int REFRESH_INTERVAL = 1000 / 30;

        private TimeSpan _countDown = TimeSpan.Zero;
        private string _format = "";

        protected override void OnInitialized()
        {
            _format = Format;
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
                OnFinish.InvokeAsync(o);
            }

            InvokeStateHasChanged();
        }
    }
}
