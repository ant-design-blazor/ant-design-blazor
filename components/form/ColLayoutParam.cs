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

        internal Dictionary<string, object> ToAttributes()
        {
            var attributes = new Dictionary<string, object>
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

            return attributes;
        }
    }
}
