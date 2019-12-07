using System;

namespace AntBlazor
{
    public static class IdGeneratorHelper
    {
        public static string Generate(string prefix)
        {
            return prefix + Guid.NewGuid();
        }
    }
}