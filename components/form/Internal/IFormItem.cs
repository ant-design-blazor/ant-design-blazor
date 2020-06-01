namespace AntDesign.Internal
{
    public interface IFormItem
    {
        internal void AddControl<TValue>(AntInputComponentBase<TValue> control);

        void Reset();
    }
}
