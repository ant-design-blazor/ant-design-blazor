namespace AntDesign
{
    public enum FormRequiredMark
    {
        /// <summary>
        /// When set to None, the form will not display any indicators by any fields, regardless of their required status
        /// </summary>
        None,

        /// <summary>
        /// When set to Required, the form will display an indicator next to required fields
        /// </summary>
        Required,

        /// <summary>
        /// When set to Optional, the form will display an indicator next to optional fields
        /// </summary>
        Optional
    }
}
