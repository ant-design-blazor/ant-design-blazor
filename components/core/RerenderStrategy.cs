namespace AntDesign
{
    /// <summary>
    /// Rerender strategy
    /// </summary>
    public enum RerenderStrategy
    {
        /// <summary>
        /// Always to rerender
        /// </summary>
        Always,

        /// <summary>
        /// Rerender only when any of the component's parameter values are changed
        /// </summary>
        ParametersHashCodeChanged,
    }
}
