namespace AntDesign
{
    public partial class GlobalToken
    {
        /**
        * @nameZH XXL
        * @default 48
        */
        public int SizeXXL { get; set; }
        /**
        * @nameZH XL
        * @default 32
        */
        public int SizeXL { get; set; }
        /**
        * @nameZH LG
        * @default 24
        */
        public int SizeLG { get; set; }
        /**
        * @nameZH MD
        * @default 20
        */
        public int SizeMD { get; set; }
        /** Same as size by default, but could be larger in compact mode */
        public int SizeMS { get; set; }
        /**
        * @nameZH 默认
        * @desc 默认尺寸
        * @default 16
        */
        public int Size { get; set; }
        /**
        * @nameZH SM
        * @default 12
        */
        public int SizeSM { get; set; }
        /**
        * @nameZH XS
        * @default 8
        */
        public int SizeXS { get; set; }
        /**
        * @nameZH XXS
        * @default 4
        */
        public int SizeXXS { get; set; }
    }

    public partial class GlobalToken
    {
        // Control
        /** Only Used for control inside component like Multiple Select inner selection item */

        /**
        * @nameZH 更小的组件高度
        * @nameEN XS component height
        * @desc 更小的组件高度
        * @descEN XS component height
        */
        public int ControlHeightXS { get; set; }

        /**
        * @nameZH 较小的组件高度
        * @nameEN SM component height
        * @desc 较小的组件高度
        * @descEN SM component height
        */
        public int ControlHeightSM { get; set; }

        /**
        * @nameZH 较高的组件高度
        * @nameEN LG component height
        * @desc 较高的组件高度
        * @descEN LG component height
        */
        public int ControlHeightLG { get; set; }
    }

}