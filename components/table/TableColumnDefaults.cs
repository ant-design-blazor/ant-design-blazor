namespace AntDesign
{
    /// <summary>
    /// Shared presentation defaults for Table columns, matching React Table's
    /// <c>column</c> prop.
    /// </summary>
    public sealed class TableColumnDefaults
    {
        /// <summary>Default alignment for columns without an explicit Align value.</summary>
        public ColumnAlign? Align { get; set; }

        /// <summary>Default ellipsis behavior for columns without an explicit Ellipsis value.</summary>
        public bool? Ellipsis { get; set; }
    }
}
