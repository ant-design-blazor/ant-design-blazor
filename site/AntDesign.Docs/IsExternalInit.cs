namespace System.Runtime.CompilerServices
{
    /// <summary>
    /// 解决使用record类型时发生的下面错误
    /// 错误 CS0518  预定义类型“System.Runtime.CompilerServices.IsExternalInit”未定义或导入 AntDesign.Docs(netcoreapp3.1)
    /// </summary>
    internal sealed class IsExternalInit
    {
    }
}
