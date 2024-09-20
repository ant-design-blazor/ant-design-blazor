﻿// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Runtime.InteropServices;

namespace AntDesign.Docs.Build.CLI.Utils
{
    public class PlatformInformationArbiter
    {
        public T GetValue<T>(Func<T> windowsValueProvider, Func<T> linuxValueProvider, Func<T> osxValueProvider, Func<T> defaultValueProvider)
        {
            return RuntimeInformation.IsOSPlatform(OSPlatform.Windows)
                ? windowsValueProvider()
                : RuntimeInformation.IsOSPlatform(OSPlatform.Linux)
                    ? linuxValueProvider()
                    : RuntimeInformation.IsOSPlatform(OSPlatform.OSX)
                        ? osxValueProvider()
                        : defaultValueProvider();
        }

        public void Invoke(Action windowsValueProvider, Action linuxValueProvider, Action osxValueProvider, Action defaultValueProvider)
        {
            var invoker = RuntimeInformation.IsOSPlatform(OSPlatform.Windows)
                ? windowsValueProvider
                : RuntimeInformation.IsOSPlatform(OSPlatform.Linux)
                    ? linuxValueProvider
                    : RuntimeInformation.IsOSPlatform(OSPlatform.OSX)
                        ? osxValueProvider
                        : defaultValueProvider;
            invoker();
        }
    }
}
