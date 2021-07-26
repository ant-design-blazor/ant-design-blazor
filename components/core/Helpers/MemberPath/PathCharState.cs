// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

namespace AntDesign.Core.Helpers.MemberPath
{
    internal enum PathCharState
    {
        Begin,

        MemberName,

        // .
        Dot,

        // [
        LeftBracket,

        // ]
        RightBracket,

        // string key char
        StringKey,

        // number key char
        NumberKey,

        // '
        SingleQuote,
    }
}
