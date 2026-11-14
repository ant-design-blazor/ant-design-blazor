// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Collections.Generic;
using OneOf;

namespace AntDesign
{
    using StringNumber = OneOf<string, int>;

    public class ColLayoutParam
    {
        public StringNumber Flex { get; set; }

        public StringNumber Span { get; set; }

        public StringNumber Order { get; set; }

        public StringNumber Offset { get; set; }

        public StringNumber Push { get; set; }

        public StringNumber Pull { get; set; }

        public OneOf<int, EmbeddedProperty> Xs { get; set; }

        public OneOf<int, EmbeddedProperty> Sm { get; set; }

        public OneOf<int, EmbeddedProperty> Md { get; set; }

        public OneOf<int, EmbeddedProperty> Lg { get; set; }

        public OneOf<int, EmbeddedProperty> Xl { get; set; }

        public OneOf<int, EmbeddedProperty> Xxl { get; set; }

        public Dictionary<string, object> ToAttributes() => new()
        {
            { nameof(Col.Flex), Flex },
            { nameof(Col.Span), Span },
            { nameof(Col.Order), Order },
            { nameof(Col.Offset), Offset },
            { nameof(Col.Push), Push },
            { nameof(Col.Pull), Pull },
            { nameof(Col.Xs), Xs },
            { nameof(Col.Sm), Sm },
            { nameof(Col.Md), Md },
            { nameof(Col.Lg), Lg },
            { nameof(Col.Xl), Xl },
            { nameof(Col.Xxl), Xxl }
        };
    }
}
