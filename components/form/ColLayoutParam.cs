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

        public Dictionary<string, object> ToAttributes()
        {
            Dictionary<string, object> attributes = new Dictionary<string, object>();

            attributes.Add(nameof(Col.Flex), Flex);
            attributes.Add(nameof(Col.Span), Span);
            attributes.Add(nameof(Col.Order), Order);
            attributes.Add(nameof(Col.Offset), Offset);
            attributes.Add(nameof(Col.Push), Push);
            attributes.Add(nameof(Col.Pull), Pull);
            attributes.Add(nameof(Col.Xs), Xs);
            attributes.Add(nameof(Col.Sm), Sm);
            attributes.Add(nameof(Col.Md), Md);
            attributes.Add(nameof(Col.Lg), Lg);
            attributes.Add(nameof(Col.Xl), Xl);
            attributes.Add(nameof(Col.Xxl), Xxl);

            return attributes;
        }
    }
}
