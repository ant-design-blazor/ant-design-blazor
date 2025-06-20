// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

namespace AntDesign.Internal.Form.Validate;

internal sealed class NumberAttribute(decimal number) : NumberCompareAttribute(number, 0)
{
}

internal sealed class NumberMinAttribute(decimal min) : NumberCompareAttribute(min, 1)
{
    // internal decimal Min => Number;
}

internal sealed class NumberMaxAttribute(decimal max) : NumberCompareAttribute(max, -1)
{
    // internal decimal Max => Number;
}
