using System;
using System.Collections.Generic;
using System.Text;

namespace AntDesign.Internal
{
    public partial class DatePickerDatetimePanel : DatePickerPanelBase
    {
        private DatePickerDisabledTime GetDisabledTime()
        {
            List<int> disabledHours = new List<int>();
            List<int> disabledMinutes = new List<int>();
            List<int> disabledSeconds = new List<int>();

            if (DatePicker.DisabledHours != null)
            {
                disabledHours.AddRange(DatePicker.DisabledHours(Value));
            }
            if (DatePicker.DisabledMinutes != null)
            {
                disabledMinutes.AddRange(DatePicker.DisabledMinutes(Value));
            }
            if (DatePicker.DisabledSeconds != null)
            {
                disabledSeconds.AddRange(DatePicker.DisabledSeconds(Value));
            }

            DatePickerDisabledTime userDisabledTime = DatePicker.DisabledTime?.Invoke(Value);

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
