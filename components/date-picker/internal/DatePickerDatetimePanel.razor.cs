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

        private int? _selectedSecond;
        private int? _selectedMinute;
        private int? _selectedHour;

        private bool _isOkDisabled;

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
            _isOkDisabled = Value is null;
        }

        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            await base.OnAfterRenderAsync(firstRender);

            if (!firstRender)
            {
                _isOkDisabled = Value is null;
                await ScrollToSelectedTimeIfChangedAsync();
            }
        }

        private async Task ScrollToSelectedHourAsync(int? duration = null)
        {
            _selectedHour = Value.Value.Hour;

            var hoursKey = Use12Hours && _selectedHour == 0 ?
                            12 : Use12Hours && _selectedHour > 12 ?
                                _selectedHour.Value - 12 : _selectedHour.Value;

            if (_hours.ContainsKey(hoursKey))
            {
                await InvokeSmoothScrollAsync(_hours[hoursKey], _hoursParent, duration);
            }
        }

        private async Task ScrollToSelectedMinuteAsync(int? duration = null)
        {
            _selectedMinute = Value.Value.Minute;

            if (_minutes.ContainsKey(_selectedMinute.Value))
            {
                await InvokeSmoothScrollAsync(_minutes[_selectedMinute.Value], _minutesParent, duration);
            }
        }

        private async Task ScrollToSelectedSecondAsync(int? duration = null)
        {
            _selectedSecond = Value.Value.Second;

            if (_seconds.ContainsKey(_selectedSecond.Value))
            {
                await InvokeSmoothScrollAsync(_seconds[_selectedSecond.Value], _secondsParent, duration);
            }
        }

        private async Task ScrollToSelectedTimeIfChangedAsync()
        {
            if (Value is null)
            {
                return;
            }

            var currentValue = Value.Value;

            if (currentValue.Hour != _selectedHour)
            {
                await ScrollToSelectedHourAsync();
            }
            if (currentValue.Minute != _selectedMinute)
            {
                await ScrollToSelectedMinuteAsync();
            }
            if (currentValue.Second != _selectedSecond)
            {
                await ScrollToSelectedSecondAsync();
            }
        }

        private void DatePicker_OverlayVisibleChanged(object sender, bool visible)
        {
            if (visible)
            {
                _ = ScrollToSelectedHourAsync(0);
                _ = ScrollToSelectedMinuteAsync(0);
                _ = ScrollToSelectedSecondAsync(0);
            }
        }

        private async Task InvokeSmoothScrollAsync(ElementReference element, ElementReference parent, int? duration)
                            => await JsInvokeAsync(JSInteropConstants.SmoothScrollTo, element, parent, duration ?? 100);

        public async ValueTask DisposeAsync()
        {
            DatePicker.OverlayVisibleChanged -= DatePicker_OverlayVisibleChanged;
            await Task.Yield();
        }
    }
}
