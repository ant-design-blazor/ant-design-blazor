using System;
using System.Collections.Generic;
using System.Linq;

namespace AntDesign
{
    public class ClassBuilder<T>
    {
        public ClassBuilder<T> If(string className, Func<T, bool> func)
        {
            _rules.Add(new ClassBuilderRuleIf<T>(className, func));
            return this;
        }

        public ClassBuilder<T> Get(Func<T, string> func)
        {
            _rules.Add(new ClassBuilderRuleGet<T>(func));
            return this;
        }

        public ClassBuilder<T> Class(string className)
        {
            _rules.Add(new ClassBuilderRuleClass<T>(className));
            return this;
        }

        public ClassBuilder<T> Dictionary<TK>(Func<T, TK> func, IDictionary<TK, string> dictionary)
        {
            _rules.Add(new ClassBuilderRuleDictionary<T, TK>(func, dictionary));
            return this;
        }

        private readonly List<ClassBuilderRule<T>> _rules = new List<ClassBuilderRule<T>>();

        public string GetClasses(T data)
        {
            return string.Join(" ", _rules.Select(i => i.GetClass(data)).Where(i => i != null));
        }
    }
}
