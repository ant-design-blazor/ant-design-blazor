using System;

namespace AntBlazor
{
    public class ClassBuilderRuleIf<T> : ClassBuilderRule<T>
    {
        public string ClassName { get; set; }
        public Func<T, bool> Func { get; set; }

        public ClassBuilderRuleIf(string className, Func<T, bool> func)
        {
            ClassName = className;
            Func = func;
        }

        public override string GetClass(T data)
        {
            if (Func(data))
            {
                return ClassName;
            }

            return null;
        }
    }
}