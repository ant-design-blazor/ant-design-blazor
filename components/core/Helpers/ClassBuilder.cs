using System;
using System.Collections.Generic;
using System.Linq;

namespace AntBlazor
{
    public class ClassBuilder<T>
    {
        public static ClassBuilder<T> Create()
        {
            return new ClassBuilder<T>();
        }

        public ClassBuilder<T> If(string className, Func<T, bool> func)
        {
            Rules.Add(new ClassBuilderRuleIf<T>(className, func));
            return this;
        }

        public ClassBuilder<T> Get(Func<T, string> func)
        {
            Rules.Add(new ClassBuilderRuleGet<T>(func));
            return this;
        }

        public ClassBuilder<T> Class(string className)
        {
            Rules.Add(new ClassBuilderRuleClass<T>(className));
            return this;
        }

        public ClassBuilder<T> Dictionary<TK>(Func<T, TK> func, IDictionary<TK, string> dictionary)
        {
            Rules.Add(new ClassBuilderRuleDictionary<T, TK>(func, dictionary));
            return this;
        }


        private List<ClassBuilderRule<T>> Rules = new List<ClassBuilderRule<T>>();

        public string GetClasses(T data)
        {
            return string.Join(" ", Rules.Select(i => i.GetClass(data)).Where(i => i != null));
        }
    }
}