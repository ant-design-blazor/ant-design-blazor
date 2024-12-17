﻿// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using System.Linq;

namespace AntDesign.TableModels;

public class GroupResult<TEntity>
{
    public int Level { get; set; }
    public object Key { get; set; }
    internal List<GroupResult<TEntity>> Children { get; set; } = [];
    public List<TEntity> Items { get; set; } = [];
}
