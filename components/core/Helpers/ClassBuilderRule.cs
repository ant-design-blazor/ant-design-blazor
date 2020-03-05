namespace AntBlazor
{
    public abstract class ClassBuilderRule<T>
    {
        public abstract string GetClass(T data);
    }
}