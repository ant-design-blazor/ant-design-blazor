namespace AntDesign
{
    public partial class GlobalToken
    {
        // Motion
        /**
        * @desc 动效播放速度，快速。用于小型元素动画交互
        * @descEN Motion speed, fast speed. Used for small element animation interaction.
        */
        public string MotionDurationFast { get; set; }
        /**
        * @desc 动效播放速度，中速。用于中型元素动画交互
        * @descEN Motion speed, medium speed. Used for medium element animation interaction.
        */
        public string MotionDurationMid { get; set; }
        /**
        * @desc 动效播放速度，慢速。用于大型元素如面板动画交互
        * @descEN Motion speed, slow speed. Used for large element animation interaction.
        */
        public string MotionDurationSlow { get; set; }
    }
}