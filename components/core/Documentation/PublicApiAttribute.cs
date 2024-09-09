// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;

namespace AntDesign.Core.Documentation
{
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Method, AllowMultiple = false, Inherited = false)]
    internal class PublicApiAttribute : Attribute
    {
        public string ReleaseVersion { get; set; }

        public string DeprecationVersion { get; set; }

        public bool Deprecated { get; set; }

        public PublicApiAttribute(string releaseVersion)
        {
            ReleaseVersion = releaseVersion;
        }
    }
}
