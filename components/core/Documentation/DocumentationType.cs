// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.ComponentModel;

namespace AntDesign
{
    public enum DocumentationType
    {
        [Description("Feedback")]
        Feedback,

        [Description("Data Display")]
        DataDisplay,

        [Description("Navigation")]
        Navigation,

        [Description("Data Entry")]
        DataEntry,

        [Description("General")]
        General,

        [Description("Layout")]
        Layout,

        [Description("Overview")]
        Overview,

        [Description("Other")]
        Other,
    }
}
