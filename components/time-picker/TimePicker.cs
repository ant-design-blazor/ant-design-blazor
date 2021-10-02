
namespace AntDesign
{
    public class TimePicker<TValue> : DatePicker<TValue>
    {
        public TimePicker()
        {
            Picker = DatePickerType.Time;
        }
    }
}
