using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Components;

namespace AntDesign.Internal
{
    public partial class DatePickerDatetimePanel<TValue> : DatePickerPanelBase<TValue>
    {
        [Parameter]
        public bool ShowToday { get; set; } = false;

        [Parameter]
        public bool IsShowTime { get; set; } = false;

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

        private DatePickerDisabledTime GetDisabledTime()
        {
            List<int> disabledHours = new List<int>();
            List<int> disabledMinutes = new List<int>();
            List<int> disabledSeconds = new List<int>();

            if (DisabledHours != null)
            {
                disabledHours.AddRange(DisabledHours(Value));
            }
            if (DisabledMinutes != null)
            {
                disabledMinutes.AddRange(DisabledMinutes(Value));
            }
            if (DisabledSeconds != null)
            {
                disabledSeconds.AddRange(DisabledSeconds(Value));
            }

            DatePickerDisabledTime userDisabledTime = DisabledTime?.Invoke(Value);

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
    }
}
