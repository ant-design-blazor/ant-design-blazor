// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;

namespace AntDesign.Core.Helpers.MemberPath
{
    public class PathNode
    {
        public readonly string Name;

        public readonly PathNodeType NodeType;

        public PathNode(string name, PathNodeType nodeType)
        {
            Name = name;
            NodeType = nodeType;
        }

        public static PathNode NewMember(string memberName)
        {
            return new(memberName, PathNodeType.Member);
        }

        public static PathNode NewIndex(string indexKey, bool isStringKey)
        {
            return new(indexKey, isStringKey ? PathNodeType.StringIndex : PathNodeType.NumberIndex);
        }

        public override string ToString()
        {
            return NodeType switch
            {
                PathNodeType.Member => Name,
                PathNodeType.StringIndex => $"['{Name.Replace("'", "''")}']",
                PathNodeType.NumberIndex => $"[{Name}]",
                _ => throw new ArgumentOutOfRangeException()
            };
        }
    }
}
