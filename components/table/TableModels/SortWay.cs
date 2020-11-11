namespace AntDesign.TableModels
{

    /// <summary>
    /// 表格排序的方式
    /// </summary>
    public enum SortWay
    {

        Default = 0,

        /// <summary>
        /// 单一
        /// </summary>
        Singleness = 0x01,

        /// <summary>
        /// 多选
        /// </summary>
        Multiple = 0x01 << 1,

    }
}
