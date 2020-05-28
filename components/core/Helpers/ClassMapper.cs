using System;
using System.Collections.Generic;
using System.Linq;

namespace AntDesign
{
    public class ClassMapper
    {
        public string Class => AsString();

        internal string OriginalClass { get; set; }

        public string AsString()
        {
            return string.Join(" ", _map.Where(i => i.Value()).Select(i => i.Key()));
        }

        public override string ToString()
        {
            return AsString();
        }

        private readonly Dictionary<Func<string>, Func<bool>> _map = new Dictionary<Func<string>, Func<bool>>();

        public ClassMapper Add(string name)
        {
            _map.Add(() => name, () => true);
            return this;
        }

        public ClassMapper Get(Func<string> funcName)
        {
            _map.Add(funcName, () => true);
            return this;
        }

        public ClassMapper GetIf(Func<string> funcName, Func<bool> func)
        {
            _map.Add(funcName, func);
            return this;
        }

        public ClassMapper If(string name, Func<bool> func)
        {
            _map.Add(() => name, func);
            return this;
        }

        public ClassMapper Clear()
        {
            _map.Clear();

            _map.Add(() => OriginalClass, () => !string.IsNullOrEmpty(OriginalClass));

            return this;
        }
    }
}
