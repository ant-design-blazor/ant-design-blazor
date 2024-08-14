// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;

namespace AntDesign.TableModels;

public class GroupResult<TEntity>
{
    public int Level { get; set; }
    public object Key { get; set; }
    public List<GroupResult<TEntity>> Children { get; set; } = [];
    public List<TEntity> Entities { get; set; } = [];

    public override int GetHashCode()
    {
        return HashCode.Combine(Entities);
    }
}
