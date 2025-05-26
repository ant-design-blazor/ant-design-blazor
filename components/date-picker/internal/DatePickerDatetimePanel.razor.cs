// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;

namespace AntDesign.Internal
{
    public partial class DatePickerDatetimePanel<TValue> : DatePickerPanelBase<TValue>, IAsyncDisposable
    {
        [Parameter]
        public bool ShowToday { get; set; } = true;

        [Parameter]
        public string ShowTimeFormat { get; set; }

        [Parameter]
        public Dictionary<string, DateTime?[]> Ranges { get; set; } = new Dictionary<string, DateTime?[]>();

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
        public EventCallback OnNowClick { get; set; }

        [Parameter]
        public EventCallback<DateTime?[]> OnRangeItemOver { get; set; }

        [Parameter]
        public EventCallback<DateTime?[]> OnRangeItemOut { get; set; }

        [Parameter]
        public EventCallback<DateTime?[]> OnRangeItemClicked { get; set; }

        [Parameter]
        public EventCallback<bool> OnOpenChange { get; set; }

        private bool ShowFooter => RenderExtraFooter != null || ShowRanges || ShowToday;
        private bool ShowNow => !IsRange && (Ranges is null || Ranges.Count == 0) || Picker == DatePickerType.Time;
        private bool ShowRanges => IsShowTime || Ranges != null;

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

        private void HandleRangeItemOver(DateTime?[] rangeitem)
        {
            if (OnRangeItemOver.HasDelegate)
            {
                OnRangeItemOver.InvokeAsync(rangeitem);
            }
        }

        private void HandleRangeItemOut(DateTime?[] rangeitem)
        {
            if (OnRangeItemOut.HasDelegate)
            {
                OnRangeItemOut.InvokeAsync(rangeitem);
            }
        }

        private void HandleRangeItemClicked(DateTime?[] rangeitem)
        {
            if (OnRangeItemClicked.HasDelegate)
            {
                OnRangeItemClicked.InvokeAsync(rangeitem);
            }
        }

        private DatePickerDisabledTime GetDisabledTime()
        {
            List<int> disabledHours = new List<int>();
            List<int> disabledMinutes = new List<int>();
            List<int> disabledSeconds = new List<int>();

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

            var userDisabledTime = DisabledTime?.Invoke(Value ?? DateTime.Now);

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

        private async Task ScrollToSelectedHourAsync(DateTime currentValue, int? duration = null)
        {
            _selectedHour = currentValue.Hour;

            var hoursKey = Use12Hours && _selectedHour == 0 ?
                            12 : Use12Hours && _selectedHour > 12 ?
                                _selectedHour.Value - 12 : _selectedHour.Value;

            if (_hours.TryGetValue(hoursKey, out var hoursVal))
            {
                await InvokeSmoothScrollAsync(hoursVal, _hoursParent, duration);
            }
        }

        private async Task ScrollToSelectedMinuteAsync(DateTime currentValue, int? duration = null)
        {
            _selectedMinute = currentValue.Minute;

            if (_minutes.TryGetValue(_selectedMinute.Value, out var minuteVal))
            {
                await InvokeSmoothScrollAsync(minuteVal, _minutesParent, duration);
            }
        }

        private async Task ScrollToSelectedSecondAsync(DateTime currentValue, int? duration = null)
        {
            _selectedSecond = currentValue.Second;

            if (_seconds.TryGetValue(_selectedSecond.Value, out var secVal))
            {
                await InvokeSmoothScrollAsync(secVal, _secondsParent, duration);
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
                await ScrollToSelectedHourAsync(currentValue);
            }
            if (currentValue.Minute != _selectedMinute)
            {
                await ScrollToSelectedMinuteAsync(currentValue);
            }
            if (currentValue.Second != _selectedSecond)
            {
                await ScrollToSelectedSecondAsync(currentValue);
            }
        }

        private void DatePicker_OverlayVisibleChanged(object sender, bool visible)
        {
            if (!visible || Value is null)
            {
                return;
            }

            var currentValue = Value.Value;

            _ = ScrollToSelectedHourAsync(currentValue, 0);
            _ = ScrollToSelectedMinuteAsync(currentValue, 0);
            _ = ScrollToSelectedSecondAsync(currentValue, 0);
        }

        private async Task InvokeSmoothScrollAsync(ElementReference element, ElementReference parent, int? duration)
                            => await JsInvokeAsync(JSInteropConstants.SmoothScrollTo, element, parent, duration ?? 120);

        public async ValueTask DisposeAsync()
        {
            DatePicker.OverlayVisibleChanged -= DatePicker_OverlayVisibleChanged;
            await Task.Yield();
        }
    }
}
