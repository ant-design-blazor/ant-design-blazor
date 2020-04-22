using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.AspNetCore.Components.Web;
using System.Threading.Tasks;
using AntBlazor.JsInterop;
using Microsoft.AspNetCore.Components.Rendering;
using AntBlazor.Internal;
using System.Linq;

namespace AntBlazor
{
    public partial class Tooltip : OverlayTrigger
    {
        [Parameter]
        public string Title { get; set; } = string.Empty;

        [Parameter]
        public bool ArrowPointAtCenter { get; set; } = false;

        [Parameter]
        public double MouseEnterDelay { get; set; } = 0.1;

        [Parameter]
        public double MouseLeaveDelay { get; set; } = 0.1;

        public Tooltip()
        {
            PrefixCls = "ant-tooltip";
            Placement = PlacementType.TopCenter;
        }

        public override string GetOverlayEnterClass()
        {
            return string.Empty;
        }

        public override string GetPlacementClass()
        {
            if (!string.IsNullOrEmpty(PlacementCls))
            {
                return PlacementCls;
            }

            if (PlacementType.TopCenter.Equals(Placement))
            {
                return $"{PrefixCls}-placement-top";
            }
            else if (PlacementType.BottomCenter.Equals(Placement))
            {
                return $"{PrefixCls}-placement-bottom";
            }
            else
            {
                return $"{PrefixCls}-placement-{Placement.Name}";
            }
        }

        public override async Task Show(int? overlayLeft = null, int? overlayTop = null)
        {
            if (Trigger.Contains(TriggerType.Hover))
            {
                await Task.Delay((int)(MouseEnterDelay * 1000));
            }
            await base.Show(overlayLeft, overlayTop);
        }

        public override async Task Hide()
        {
            if (Trigger.Contains(TriggerType.Hover))
            {
                await Task.Delay((int)(MouseLeaveDelay * 1000));
            }
            await base.Hide();
        }
    }
}
