---
order: 20
title:
  zh-CN: 遇到边界时自动调整方向
  en-US: boundary detection and orientation adjustment 
---

## zh-CN

Use enum `TriggerBoundaryAdjustMode` to choose pop-up repositioning strategy if the dropdown does not fit in the visible area. There are 3 available strategies:
- None - Do not reposition.
- InView - Attempt to fit the overlay so it is always fully visible in the viewport.
        So if the overlay is outside of the viewport ("overflows"), then reposition calculation is going 
        to be attempted.
- InScroll - The document boundaries are the boundaries used for calculation if overlay needs to be reposition. 
        So even if the overlay is outside of the viewport, the overlay may still be shown as long as it 
        does not "overflow" the document boundaries.

## en-US

Use enum `TriggerBoundaryAdjustMode` to choose pop-up repositioning strategy if the dropdown does not fit in the visible area. There are 3 available strategies:
- None - Do not reposition.
- InView - Attempt to fit the overlay so it is always fully visible in the viewport.
        So if the overlay is outside of the viewport ("overflows"), then reposition calculation is going 
        to be attempted.
- InScroll - The document boundaries are the boundaries used for calculation if overlay needs to be reposition. 
        So even if the overlay is outside of the viewport, the overlay may still be shown as long as it 
        does not "overflow" the document boundaries.