namespace AntDesign
{
    public abstract class ClassBuilderRule<T>
    {
        public abstract string GetClass(T data);
    }
}