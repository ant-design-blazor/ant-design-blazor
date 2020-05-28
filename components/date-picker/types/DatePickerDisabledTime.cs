namespace AntDesign
{
    public class DatePickerDisabledTime
    {
        internal int[] _disabledHours;
        internal int[] _disabledMinutes;
        internal int[] _disabledSeconds;

        public DatePickerDisabledTime(
            int[] disabledHours, int[] disabledMinutes, int[] disabledSeconds)
        {
            _disabledHours = disabledHours;
            _disabledMinutes = disabledMinutes;
            _disabledSeconds = disabledSeconds;
        }
    }
}
