using System;

namespace AntDesign
{
    public static class IdGeneratorHelper
    {
        public static string Generate(string prefix)
        {
            return prefix + Guid.NewGuid();
        }
    }
}