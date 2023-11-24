namespace AntDesign
{
    public partial class GlobalToken
    {
        // Font Size
        /**
        * @desc 小号字体大小
        * @descEN Small font size
        */
        public int FontSizeSM { get; set; }
        /**
        * @desc 标准字体大小
        * @descEN Standard font size
        */
        public int FontSize { get; set; }
        /**
        * @desc 大号字体大小
        * @descEN Large font size
        */
        public int FontSizeLG { get; set; }
        /**
        * @desc 超大号字体大小
        * @descEN Super large font size
        */
        public int FontSizeXL { get; set; }

        /**
        * @nameZH 一级标题字号
        * @nameEN Font size of heading level 1
        * @desc H1 标签所使用的字号
        * @descEN Font size of h1 tag.
        * @default 38
        */
        public int FontSizeHeading1 { get; set; }
        /**
        * @nameZH 二级标题字号
        * @nameEN Font size of heading level 2
        * @desc h2 标签所使用的字号
        * @descEN Font size of h2 tag.
        * @default 30
        */
        public int FontSizeHeading2 { get; set; }
        /**
        * @nameZH 三级标题字号
        * @nameEN Font size of heading level 3
        * @desc h3 标签使用的字号
        * @descEN Font size of h3 tag.
        * @default 24
        */
        public int FontSizeHeading3 { get; set; }
        /**
        * @nameZH 四级标题字号
        * @nameEN Font size of heading level 4
        * @desc h4 标签使用的字号
        * @descEN Font size of h4 tag.
        * @default 20
        */
        public int FontSizeHeading4 { get; set; }
        /**
        * @nameZH 五级标题字号
        * @nameEN Font size of heading level 5
        * @desc h5 标签使用的字号
        * @descEN Font size of h5 tag.
        * @default 16
        */
        public int FontSizeHeading5 { get; set; }

        // LineHeight
        /**
        * @desc 文本行高
        * @descEN Line height of text.
        */
        public int LineHeight { get; set; }
        /**
        * @desc 大型文本行高
        * @descEN Line height of large text.
        */
        public int LineHeightLG { get; set; }
        /**
        * @desc 小型文本行高
        * @descEN Line height of small text.
        */
        public int LineHeightSM { get; set; }

        /**
        * @nameZH 一级标题行高
        * @nameEN Line height of heading level 1
        * @desc H1 标签所使用的行高
        * @descEN Line height of h1 tag.
        * @default 1.4
        */
        public int LineHeightHeading1 { get; set; }
        /**
        * @nameZH 二级标题行高
        * @nameEN Line height of heading level 2
        * @desc h2 标签所使用的行高
        * @descEN Line height of h2 tag.
        * @default 1.35
        */
        public int LineHeightHeading2 { get; set; }
        /**
        * @nameZH 三级标题行高
        * @nameEN Line height of heading level 3
        * @desc h3 标签所使用的行高
        * @descEN Line height of h3 tag.
        * @default 1.3
        */
        public int LineHeightHeading3 { get; set; }
        /**
        * @nameZH 四级标题行高
        * @nameEN Line height of heading level 4
        * @desc h4 标签所使用的行高
        * @descEN Line height of h4 tag.
        * @default 1.25
        */
        public int LineHeightHeading4 { get; set; }
        /**
        * @nameZH 五级标题行高
        * @nameEN Line height of heading level 5
        * @desc h5 标签所使用的行高
        * @descEN Line height of h5 tag.
        * @default 1.2
        */
        public int LineHeightHeading5 { get; set; }
    }

}