using System;
using System.Collections.Generic;
using System.Linq;

namespace AntDesign
{
    public class ClassMapper
    {
        private readonly Dictionary<Func<string>, Func<bool>> _map = new();

        public ClassMapper() : this(string.Empty)
        {
        }

        public ClassMapper(string originalClass)
        {
            OriginalClass = originalClass;

            _map.Add(() => OriginalClass, () => !string.IsNullOrEmpty(OriginalClass));
        }

        public string Class => ToString();

        public string OriginalClass { get; internal set; }

        [Obsolete("Use ToString() instead")]
        public string AsString() => ToString();

        public override string ToString() => string.Join(' ', _map.Where(i => i.Value()).Select(i => i.Key()));

        public ClassMapper Add(string name) => Get(() => name);

        public ClassMapper Get(Func<string> funcName)
        {
            _map.Add(funcName, () => !string.IsNullOrEmpty(funcName()));
            return this;
        }

        public ClassMapper GetIf(Func<string> funcName, Func<bool> func)
        {
            _map.Add(funcName, func);
            return this;
        }

        public ClassMapper If(string name, Func<bool> func) => GetIf(() => name, func);

        public ClassMapper Clear()
        {
            _map.Clear();

            _map.Add(() => OriginalClass, () => !string.IsNullOrEmpty(OriginalClass));

            return this;
        }
    }
}
