namespace AntDesign
{
    /// <summary>
    /// Rendering mode
    /// </summary>
    public enum RenderMode
    {
        /// <summary>
        /// Always to render
        /// </summary>
        Always,

        /// <summary>
        /// Render when the hashCode of the parameter value changes
        /// </summary>
        ParametersHashCodeChanged,
    }
}
