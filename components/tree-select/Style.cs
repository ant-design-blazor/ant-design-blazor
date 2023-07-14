using System;
using CssInCs;
using static AntDesign.GlobalStyle;
using static AntDesign.Theme;

namespace AntDesign
{
    public partial class TreeSelectToken : TokenWithCommonCls
    {
        public string TreePrefixCls { get; set; }

    }

    public partial class TreeSelect
    {
        public Unknown_1 GenBaseStyle(Unknown_3 token)
        {
            var componentCls = token.ComponentCls;
            var treePrefixCls = token.TreePrefixCls;
            var colorBgElevated = token.ColorBgElevated;
            var treeCls = @$".{treePrefixCls}";
            return new Unknown_4 { [object Object] };
        }

        public Unknown_2 UseTreeSelectStyle(string prefixCls, string treePrefixCls)
        {
            return GenComponentStyleHook('TreeSelect', (token) => {
    const treeSelectToken = mergeToken<TreeSelectToken>(token, { treePrefixCls });
    return [genBaseStyle(treeSelectToken)];
  })(prefixCls);
        }

    }

}