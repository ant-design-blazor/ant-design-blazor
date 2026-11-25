// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System;

namespace AntDesign;

/// <summary>Context supplied to range picker disabled-date callbacks.</summary>
public sealed record DatePickerDisabledDateInfo(DateTime? From, DatePickerType Type);
