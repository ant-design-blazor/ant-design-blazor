using System;

namespace AntDesign
{
    internal class GuidComponentIdGenerator : IComponentIdGenerator
    {
        public string Generate(AntDomComponentBase component) => "ant-blazor-" + Guid.NewGuid();
    }
}
