
namespace AntDesign
{
    public partial class TimePicker<TValue> : DatePicker<TValue>
    {
        public TimePicker()
        {
            Picker = DatePickerType.Time;
        }
    }
}
