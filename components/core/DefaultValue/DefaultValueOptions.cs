namespace AntDesign
{
    public class DefaultValueOptions
    {
        public InputDefaultValueOptions InputDefaultValue => InputDefaultValueOptions.Instance;
        public DialogDefaultValueOptions DialogDefaultValue => DialogDefaultValueOptions.Instance;
        public ConfirmDefaultValueOptions ConfirmDefaultValue => ConfirmDefaultValueOptions.Instance;
        public ModalDefaultValueOptions ModalDefaultValue => ModalDefaultValueOptions.Instance;
    }
}
