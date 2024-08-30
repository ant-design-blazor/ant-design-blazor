// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

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
