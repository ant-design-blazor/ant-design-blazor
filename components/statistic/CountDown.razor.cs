using System;
using System.Text.RegularExpressions;
using System.Threading;
using Microsoft.AspNetCore.Components;

namespace AntDesign
{
    public partial class CountDown : StatisticComponentBase<DateTime>
    {
        [Parameter] public string Format { get; set; } = "hh:mm:ss";

        [Parameter] public EventCallback OnFinish { get; set; }

        private Timer _timer;

        private const int REFRESH_INTERVAL = 1000 / 30;

        private TimeSpan _countDown = TimeSpan.Zero;
        private string _format = "";

        /// <summary>
        /// parse other characters in format string.
        /// </summary>
        /// <remarks>refer to https://docs.microsoft.com/en-us/dotnet/standard/base-types/custom-timespan-format-strings#other-characters</remarks>
        /// <param name="format"></param>
        /// <returns></returns>
        private static string ParseSpanTimeFormatString(string format)
        {
            if (string.IsNullOrWhiteSpace(format))
            {
                return format;
            }

            return Regex.Replace(format, "[^d|^h|^m|^s|^f|^F]+", "'$0'");
        }

        protected override void OnInitialized()
        {
            _format = ParseSpanTimeFormatString(Format);
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
                    OnFinish.InvokeAsync(o);
                }
            }

            InvokeStateHasChanged();
        }
    }
}
