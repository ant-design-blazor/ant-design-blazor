using CssInCSharp;
using static AntDesign.GlobalStyle;
using static AntDesign.Theme;
using static AntDesign.StyleUtil;

namespace AntDesign
{
    public class AffixToken : TokenWithCommonCls
    {
        public int ZIndexPopup { get; set; }

    }

    public partial class Affix
    {
        public CSSObject GenSharedAffixStyle(AffixToken token)
        {
            var componentCls = token.ComponentCls;
            return new CSSObject()
            {
                [componentCls] = new CSSObject()
                {
                    Position = "fixed",
                    ZIndex = token.ZIndexPopup,
                },
            };
        }

        protected override UseComponentStyleResult UseComponentStyle()
        {
            return GenComponentStyleHook("Affix", (token) =>
            {
                var affixToken = MergeToken(
                    token,
                    new AffixToken()
                    {
                        ZIndexPopup = token.ZIndexBase + 10,
                    });
                return new CSSInterpolation[] { GenSharedAffixStyle(affixToken) };
            });
        }
    }

}
