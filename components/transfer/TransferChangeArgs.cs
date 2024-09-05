// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

namespace AntDesign
{
    public class TransferChangeArgs
    {
        public string[] TargetKeys { get; private set; }

        public TransferDirection Direction { get; private set; }

        public string[] MoveKeys { get; private set; }

        public TransferChangeArgs(string[] targetKeys, TransferDirection direction, string[] moveKeys)
        {
            TargetKeys = targetKeys;
            Direction = direction;
            MoveKeys = moveKeys;
        }
    }
}
