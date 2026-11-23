namespace AntDesign
{
    /// <summary>A color stop in a <see cref="BorderBeam.Color"/> gradient.</summary>
    public sealed class BorderBeamGradientStop
    {
        /// <summary>CSS color value for this stop.</summary>
        public string Color { get; set; }

        /// <summary>Position within the visible beam segment, from 0 to 100.</summary>
        public double Percent { get; set; }
    }
}
