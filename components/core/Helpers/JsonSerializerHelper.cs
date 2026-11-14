// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Text.Json;

namespace AntDesign.Core.Helpers;

internal partial class JsonSerializerHelper
{
    public static JsonSerializerOptions DefaultOptions { get; }
#if NET9_0_OR_GREATER
    = JsonSerializerOptions.Web;
#else
    = new(JsonSerializerDefaults.Web);
#endif
}
