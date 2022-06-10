using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;

namespace AntDesign.Internal
{
    public partial class DatePickerDatetimePanel<TValue> : DatePickerPanelBase<TValue>, IAsyncDisposable
    {
        [Parameter]
        public bool ShowToday { get; set; } = false;

        [Parameter]
        public string ShowTimeFormat { get; set; }

        [Parameter]
        public string Format { get; set; }

        [Parameter]
        public Func<DateTime, int[]> DisabledHours { get; set; } = null;

        [Parameter]
        public Func<DateTime, int[]> DisabledMinutes { get; set; } = null;

        [Parameter]
        public Func<DateTime, int[]> DisabledSeconds { get; set; } = null;

        [Parameter]
        public Func<DateTime, DatePickerDisabledTime> DisabledTime { get; set; } = null;

        [Parameter]
        public EventCallback OnOkClick { get; set; }

        [Parameter]
        public EventCallback<bool> OnOpenChange { get; set; }

        private Dictionary<int, ElementReference> _hours = new();
        private Dictionary<int, ElementReference> _minutes = new();
        private Dictionary<int, ElementReference> _seconds = new();

        private ElementReference _hoursParent;
        private ElementReference _minutesParent;
        private ElementReference _secondsParent;

        private DatePickerDisabledTime GetDisabledTime()
        {
            List<int> disabledHours = new List<int>();
            List<int> disabledMinutes = new List<int>();
            List<int> disabledSeconds = new List<int>();
            DatePickerDisabledTime userDisabledTime = null;

            if (DisabledHours is not null)
            {
                disabledHours.AddRange(DisabledHours(Value ?? DateTime.Now));
            }
            if (DisabledMinutes is not null)
            {
                disabledMinutes.AddRange(DisabledMinutes(Value ?? DateTime.Now));
            }
            if (DisabledSeconds is not null)
            {
                disabledSeconds.AddRange(DisabledSeconds(Value ?? DateTime.Now));
            }

            userDisabledTime = DisabledTime?.Invoke(Value ?? DateTime.Now);

            if (userDisabledTime != null)
            {
                if (userDisabledTime._disabledHours != null)
                {
                    disabledHours.AddRange(userDisabledTime._disabledHours);
                }
                if (userDisabledTime._disabledMinutes != null)
                {
                    disabledMinutes.AddRange(userDisabledTime._disabledMinutes);
                }
                if (userDisabledTime._disabledSeconds != null)
                {
                    disabledSeconds.AddRange(userDisabledTime._disabledSeconds);
                }
            }

            return new DatePickerDisabledTime(disabledHours.ToArray(), disabledMinutes.ToArray(), disabledSeconds.ToArray());
        }

        protected override void OnInitialized()
        {
            base.OnInitialized();
            DatePicker.OverlayVisibleChanged += DatePicker_OverlayVisibleChanged;
        }

        private async Task ScrollToSelectedHourAsync() => await JsInvokeAsync(JSInteropConstants.ScrollTo,
                                                                              _hours[Value.Value.Hour],
                                                                              _hoursParent);
        private async Task ScrollToSelectedMinuteAsync() => await JsInvokeAsync(JSInteropConstants.ScrollTo,
                                                                                _minutes[Value.Value.Minute],
                                                                                _minutesParent);
        private async Task ScrollToSelectedSecondAsync() => await JsInvokeAsync(JSInteropConstants.ScrollTo,
                                                                                _seconds[Value.Value.Second],
                                                                                _secondsParent);

        private async Task ScrollToSelectedTimeAsync()
        {
            await ScrollToSelectedHourAsync();
            await ScrollToSelectedMinuteAsync();
            await ScrollToSelectedSecondAsync();
        }

        private void DatePicker_OverlayVisibleChanged(object sender, bool visible)
        {
            if (visible) _ = ScrollToSelectedTimeAsync();
        }

        protected override void OnSelectHour(DateTime date)
        {
            base.OnSelectHour(date);
            _ = ScrollToSelectedHourAsync();
        }

        protected override void OnSelectMinute(DateTime date)
        {
            base.OnSelectMinute(date);
            _ = ScrollToSelectedMinuteAsync();
        }

        protected override void OnSelectSecond(DateTime date)
        {
            base.OnSelectSecond(date);
            _ = ScrollToSelectedSecondAsync();
        }

        public async ValueTask DisposeAsync()
        {
            DatePicker.OverlayVisibleChanged -= DatePicker_OverlayVisibleChanged;
            await Task.Yield();
        }
    }
}
