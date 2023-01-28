namespace AntDesign
{
    public class ListGridType
    {
        /// <summary>
        /// spacing between grid
        /// </summary>
        public int Gutter { get; set; }

        /// <summary>
        /// column of grid
        /// </summary>
        public int Column { get; set; }

        /// <summary>
        /// &lt;576px column of grid
        /// </summary>
        public int Xs { get; set; }

        /// <summary>
        /// ≥576px column of grid
        /// </summary>
        public int Sm { get; set; }

        /// <summary>
        /// ≥768px column of grid
        /// </summary>
        public int Md { get; set; }

        /// <summary>
        /// ≥992px column of grid
        /// </summary>
        public int Lg { get; set; }

        /// <summary>
        /// ≥1200px column of grid
        /// </summary>
        public int Xl { get; set; }

        /// <summary>
        /// ≥1600px column of grid
        /// </summary>
        public int Xxl { get; set; }
    }
}
