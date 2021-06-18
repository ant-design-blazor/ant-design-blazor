namespace AntDesign
{
    public enum FocusBehavior
    {
        /// <summary>
        /// When focuses, cursor will move to the last character
        /// This is default behavior.
        /// </summary>
        FocusAtLast,
        /// <summary>
        /// When focuses, cursor will move to the first character
        /// </summary>
        FocusAtFirst,
        /// <summary>
        /// When focuses, the content will be selected
        /// </summary>
        FocusAndSelectAll,
        /// <summary>
        /// When focuses, content will be cleared
        /// </summary>
        FocusAndClear
    }
}
