using System;
using System.Collections.Generic;
using System.Text;
using AntBlazor.Internal;
using AntBlazor.JsInterop;
using Microsoft.AspNetCore.Components;

namespace AntBlazor
{
    public partial class TooltipPopup : Overlay
    {
        private const int ARROW_WIDTH = 5;
        private const int HORIZONTAL_ARROW_SHIFT = 16;
        private const int VERTICAL_ARROW_SHIFT = 8;

        [CascadingParameter(Name = "Title")]
        public string Title { get; set; }

        [CascadingParameter(Name = "ArrowPointAtCenter")]
        public bool ArrowPointAtCenter { get; set; }

        protected override int GetOverlayLeft(Element trigger, Element overlay, Element containerElement)
        {
            int left = 0;
            int triggerLeft = trigger.absoluteLeft - containerElement.absoluteLeft;
            int triggerWidth = trigger.clientWidth;

            // contextMenu
            if (_overlayLeft != null)
            {
                triggerLeft += (int)_overlayLeft;
                triggerWidth = 0;
            }

            if (Trigger.Placement.IsIn(PlacementType.Left, PlacementType.LeftTop, PlacementType.LeftBottom))
            {
                left = triggerLeft - overlay.clientWidth - OVERLAY_OFFSET;
            }
            else if (Trigger.Placement.IsIn(PlacementType.BottomLeft, PlacementType.TopLeft))
            {
                left = ArrowPointAtCenter ?
                       triggerLeft + triggerWidth / 2 - (HORIZONTAL_ARROW_SHIFT + ARROW_WIDTH) :
                       triggerLeft;
            }
            else if (Trigger.Placement.IsIn(PlacementType.BottomCenter, PlacementType.TopCenter))
            {
                left = (int)(triggerLeft + triggerWidth / 2d - overlay.clientWidth / 2d);
            }
            else if (Trigger.Placement.IsIn(PlacementType.BottomRight, PlacementType.TopRight))
            {
                left = ArrowPointAtCenter ?
                       triggerLeft + triggerWidth / 2 - overlay.clientWidth + (HORIZONTAL_ARROW_SHIFT + ARROW_WIDTH) :
                       triggerLeft + triggerWidth - overlay.clientWidth;
            }
            else if (Trigger.Placement.IsIn(PlacementType.Right, PlacementType.RightTop, PlacementType.RightBottom))
            {
                left = triggerLeft + triggerWidth + OVERLAY_OFFSET;
            }
            return left;
        }

        protected override int GetOverlayTop(Element trigger, Element overlay, Element containerElement)
        {
            int top = 0;

            int triggerTop = trigger.absoluteTop - containerElement.absoluteTop;
            int triggerHeight = trigger.clientHeight != 0 ? trigger.clientHeight : trigger.offsetHeight;

            // contextMenu
            if (_overlayTop != null)
            {
                triggerTop += (int)_overlayTop;
                triggerHeight = 0;
            }

            if (Trigger.Placement.IsIn(PlacementType.Left, PlacementType.Right))
            {
                top = triggerTop;
            }
            else if (Trigger.Placement.IsIn(PlacementType.TopCenter, PlacementType.TopLeft, PlacementType.TopRight))
            {
                top = triggerTop - overlay.clientHeight - OVERLAY_OFFSET;
            }
            else if (Trigger.Placement.IsIn(PlacementType.BottomCenter, PlacementType.BottomLeft, PlacementType.BottomRight))
            {
                top = triggerTop + triggerHeight + OVERLAY_OFFSET;
            }
            else if (Trigger.Placement.IsIn(PlacementType.LeftTop, PlacementType.RightTop))
            {
                top = ArrowPointAtCenter ?
                      triggerTop + overlay.clientHeight / 2 - (VERTICAL_ARROW_SHIFT + ARROW_WIDTH) :
                      triggerTop - triggerHeight / 2 + overlay.clientHeight / 2;
            }
            else if (Trigger.Placement.IsIn(PlacementType.LeftBottom, PlacementType.RightBottom))
            {
                top = ArrowPointAtCenter ?
                      triggerTop - overlay.clientHeight / 2 + (VERTICAL_ARROW_SHIFT + ARROW_WIDTH) :
                      triggerTop + triggerHeight / 2 - overlay.clientHeight / 2;
            }
            return top;
        }
    }
}
