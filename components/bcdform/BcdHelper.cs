namespace AntDesign
{
    internal static class BcdHelper
    {
        /// <summary>
        /// return state == FormState.Normal;
        /// </summary>
        /// <param name="state"></param>
        /// <returns></returns>
        public static bool IsNormal(this FormState state)
        {
            return state == FormState.Normal;
        }

        /// <summary>
        /// return state == FormState.Min;
        /// </summary>
        /// <param name="state"></param>
        /// <returns></returns>
        public static bool IsMin(this FormState state)
        {
            return state == FormState.Min;
        }


        /// <summary>
        /// return state == FormState.Max;
        /// </summary>
        /// <param name="state"></param>
        /// <returns></returns>
        public static bool IsMax(this FormState state)
        {
            return state == FormState.Max;
        }


        internal static string ToCls(this FormState state)
        {
            switch (state)
            {
                case FormState.Min:
                    return $"{BcdForm.Prefix}-min";
                case FormState.Max:
                    return $"{BcdForm.Prefix}-max";
                default:
                    return "";
            }
        }

        internal static string ToCls(this MinPosition state)
        {
            switch (state)
            {
                case MinPosition.RightBottom:
                    return $"min-rb";
                default:
                    return $"min-lb";
            }
        }
    }
}
