// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

namespace AntDesign
{
    public enum FormValidateMode
    {
        /// <summary>
        /// Blazor's validate mode. Add validateAttributes on model field
        /// Blazor的验证模式，在model字段上附加验证特性
        /// </summary>
        Default = 0,
        /// <summary>
        /// Set rules on FormItem
        /// 在FormItem上设置Rules参数
        /// </summary>
        Rules = 1,
        /// <summary>
        /// Use both Default mode and Rules mode
        /// 同时使用Default和Rules模式
        /// </summary>
        Complex = 3,
    }
}
