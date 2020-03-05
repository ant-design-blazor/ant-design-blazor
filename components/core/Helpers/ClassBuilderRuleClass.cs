namespace AntBlazor
{
    public class ClassBuilderRuleClass<T> : ClassBuilderRule<T>
    {
        public string ClassName { get; set; }


        public ClassBuilderRuleClass(string className)
        {
            ClassName = className;
        }

        public override string GetClass(T data)
        {
            return ClassName;
        }
    }
}