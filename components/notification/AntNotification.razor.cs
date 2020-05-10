using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.AspNetCore.Components;

namespace AntBlazor
{
    public partial class AntNotification
    {
        #region Parameters

        [Parameter]
        public bool IsRtl { get; set; }

        [Parameter]
        public double Top { get; set; }

        [Parameter]
        public double Bottom { get; set; }

        [Parameter]
        public AntNotificationPlacement Placement { get; set; }

        #endregion

        private string GetClassName()
        {
            string placementStr = Placement.ToString();
            placementStr = placementStr[0] == 'T'
                ? "t" + placementStr.Substring(1)
                : "b" + placementStr.Substring(1);
            string className = "ant-notification ant-notification-" + placementStr;

            if (IsRtl)
            {
                className += " ant-notification-rtl";
            }

            return className;
        }

        private string GetStyle()
        {
            switch (Placement)
            {
                case AntNotificationPlacement.TopRight:
                    return $"right: 0px; top:{Top}px; bottom: auto;";
                case AntNotificationPlacement.TopLeft:
                    return $"left: 0px; top:{Top}px; bottom: auto;";
                case AntNotificationPlacement.BottomLeft:
                    return $"left: 0px; top: auto; bottom: {Bottom}px;";
                default:
                    return $"right: 0px; top: auto; bottom: {Bottom}px;";
            }
        }
    }
}
