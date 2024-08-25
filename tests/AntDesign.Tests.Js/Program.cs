// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

namespace AntDesign.Tests.Js
{
    /// <summary>
    /// Dummy class, js tests are forced to be console app. Without it, Msbuild complains the lack of 
    /// entry point.
    /// </summary>
    public static class Program
    {
        static void Main()
        {
        }
    }
}
