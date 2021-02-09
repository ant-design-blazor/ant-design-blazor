namespace AntDesign
{
    public enum TriggerBoundaryAdjustMode
    {
        /// <summary>
        /// 不自动调整
        /// do not auto adjust
        /// </summary>
        None,
        /// <summary>
        /// 在可视范围内（默认模式）
        /// in view(default mode)
        /// </summary>
        InView,
        /// <summary>
        /// 在滚动范围内
        /// in scroll
        /// </summary>
        InScroll,
    }
}
