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

        public AntLableAlignType LableAlignType { get; set; } = AntLableAlignType.Right;

        public Dictionary<string, object> ToAttributes()
        {
            Dictionary<string, object> attributes = new Dictionary<string, object>();

            attributes.Add("Flex", Flex);
            attributes.Add("Span", Span);
            attributes.Add("Order", Order);
            attributes.Add("Offset", Offset);
            attributes.Add("Push", Push);
            attributes.Add("Pull", Pull);
            attributes.Add("Xs", Xs);
            attributes.Add("Sm", Sm);
            attributes.Add("Md", Md);
            attributes.Add("Lg", Lg);
            attributes.Add("Xl", Xl);
            attributes.Add("Xxl", Xxl);
            attributes.Add("LableAlignType", LableAlignType);

            return attributes;
        }
    }
}
