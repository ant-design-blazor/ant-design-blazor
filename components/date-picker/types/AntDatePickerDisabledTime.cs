namespace AntBlazor
{
    public class AntDatePickerDisabledTime
    {
        internal int[] _disabledHours;
        internal int[] _disabledMinutes;
        internal int[] _disabledSeconds;

        public AntDatePickerDisabledTime(
            int[] disabledHours, int[] disabledMinutes, int[] disabledSeconds)
        {
            _disabledHours = disabledHours;
            _disabledMinutes = disabledMinutes;
            _disabledSeconds = disabledSeconds;
        }
    }
}
