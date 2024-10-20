/* eslint-disable @typescript-eslint/no-unused-vars */
import { domInfoHelper, domTypes } from '../dom/exports'
import { resize } from '../../ObservableApi/observableApi';
import { mutationObserver as mutation } from '../../ObservableApi/mutationObserver';

//Make sure the enum is identical as C# AntDesign.Placement enum
export enum Placement {
  TopLeft       = 0,
  Top           = 2,
  TopRight      = 3,
  Left          = 4,
  LeftTop       = 5,
  LeftBottom    = 6,
  Right         = 7,
  RightTop      = 8,
  RightBottom   = 9,
  BottomLeft    = 10,
  Bottom        = 12,
  BottomRight   = 13
}

//Make sure the enum is identical as C# AntDesign.TriggerBoundyAdjustMode enum
export enum TriggerBoundyAdjustMode {
  None     = 0,
  InView   = 1,
  InScroll = 2
}

type verticalPosition = {
  top?: number,
  bottom?: number
}

type horizontalPosition = {
  left?: number,
  right?: number
}

export type overlayConstraints = {
  verticalOffset: number,
  horizontalOffset: number,
  arrowPointAtCenter: boolean
}
export type coordinates = {
  top?: number,
  bottom?: number,
  left?: number,
  right?: number
}


export type overlayPosition = {
  top?: number,
  bottom?: number,
  left?: number,
  right?: number,
  zIndex: number,
  placement?: Placement,
}

export class Overlay {
  private static appliedStylePositionMap: Map<Placement,
        { horizontal: "left" | "right", vertical: "top" | "bottom", class: string }> =
      new Map([
        [Placement.TopLeft,       { horizontal: "left", vertical: "bottom", class: "topLeft"  }],
        [Placement.Top,           { horizontal: "left", vertical: "bottom", class: "top" }],
        [Placement.TopRight,      { horizontal: "right", vertical: "bottom", class: "topRight" }],
        [Placement.Left,          { horizontal: "right", vertical: "top", class: "left" }],
        [Placement.LeftTop,       { horizontal: "right", vertical: "top", class: "leftTop" }],
        [Placement.LeftBottom,    { horizontal: "right", vertical: "bottom", class: "leftBottom" }],
        [Placement.Right,         { horizontal: "left", vertical: "top", class: "right" }],
        [Placement.RightTop,      { horizontal: "left", vertical: "top", class: "rightTop" }],
        [Placement.RightBottom,   { horizontal: "left", vertical: "bottom", class: "rightBottom" }],
        [Placement.BottomLeft,    { horizontal: "left", vertical: "top", class: "bottomLeft" }],
        [Placement.Bottom,        { horizontal: "left", vertical: "top", class: "bottom" }],
        [Placement.BottomRight,   { horizontal: "right", vertical: "top", class: "bottomRight" }],
      ]);

  // eslint-disable-next-line @typescript-eslint/ban-types
  private static reverseVerticalPlacementMap: Map<Placement, Function> =
    new Map([
      [Placement.TopLeft,       (position: string) => Placement.BottomLeft],
      [Placement.Top,           (position: string) => Placement.Bottom],
      [Placement.TopRight,      (position: string) => Placement.BottomRight],
      [Placement.Left,          (position: string) => position === "top" ? Placement.LeftBottom : Placement.LeftTop],
      [Placement.LeftTop,       (position: string) => Placement.LeftBottom],
      [Placement.LeftBottom,    (position: string) => Placement.LeftTop],
      [Placement.Right,         (position: string) => position === "top" ? Placement.RightBottom : Placement.RightTop],
      [Placement.RightTop,      (position: string) => Placement.RightBottom],
      [Placement.RightBottom,   (position: string) => Placement.RightTop],
      [Placement.BottomLeft,    (position: string) => Placement.TopLeft],
      [Placement.Bottom,        (position: string) => Placement.Top],
      [Placement.BottomRight,   (position: string) => Placement.TopRight]
    ]);
    
  // eslint-disable-next-line @typescript-eslint/ban-types
  private static reverseHorizontalPlacementMap: Map<Placement, Function> =
    new Map([
      [Placement.TopLeft,       (position: string) => Placement.TopRight],
      [Placement.Top,     (position: string) => position === "left" ? Placement.TopRight : Placement.TopLeft],
      [Placement.TopRight,      (position: string) => Placement.TopLeft],
      [Placement.Left,          (position: string) => Placement.Right],
      [Placement.LeftTop,       (position: string) => Placement.RightTop],
      [Placement.LeftBottom,    (position: string) => Placement.RightBottom],
      [Placement.Right,         (position: string) => Placement.Left],
      [Placement.RightTop,      (position: string) => Placement.LeftBottom],
      [Placement.RightBottom,   (position: string) => Placement.LeftTop],
      [Placement.BottomLeft,    (position: string) => Placement.BottomRight],
      [Placement.Bottom,        (position: string) => position === "left" ? Placement.BottomRight : Placement.BottomLeft],
      [Placement.BottomRight,   (position: string) => Placement.BottomLeft]
    ]);    
    
  private static arrowCenterPlacementMatch: Map<Placement, Placement> =
    new Map([
      [Placement.TopLeft,       Placement.Top],
      [Placement.Top,           Placement.Top],
      [Placement.TopRight,      Placement.Top],
      [Placement.Left,          Placement.Left],
      [Placement.LeftTop,       Placement.Left],
      [Placement.LeftBottom,    Placement.Left],
      [Placement.Right,         Placement.Right],
      [Placement.RightTop,      Placement.Right],
      [Placement.RightBottom,   Placement.Right],
      [Placement.BottomLeft,    Placement.Bottom],
      [Placement.Bottom,        Placement.Bottom],
      [Placement.BottomRight,   Placement.Bottom]
    ]);   
    
  private blazorId: string;
  public overlay: HTMLDivElement;
  private container: HTMLElement;
  private trigger: HTMLElement;
  private scrollableContainers: HTMLElement[];

  private overlayInfo: domTypes.domInfo;
  private containerInfo: domTypes.domInfo;
  private triggerInfo: domTypes.domInfo;
  
  private containerBoundarySize: coordinates;
  private bodyBoundarySize: coordinates;
  
  private placement: Placement;
  private recentPlacement: Placement;
  private initialPlacement?: Placement;

  private triggerPrefixCls: string; 

  private boundyAdjustMode: TriggerBoundyAdjustMode
  public position: overlayPosition;
  public sanitizedPosition: overlayPosition;
  
  private overlayPreset: domTypes.position;

  private verticalCalculation:
    (triggerPosition: number, triggerHeight: number, container: domTypes.domInfo,
    trigger: domTypes.domInfo, overlayHeight: number, constraints: overlayConstraints)
      => verticalPosition;

  private horizontalCalculation:
    (triggerPosition: number, triggerWidth: number, container: domTypes.domInfo,
    trigger: domTypes.domInfo, overlayWidth: number, constraints: overlayConstraints)
        => horizontalPosition;

  private overlayConstraints: overlayConstraints; 
  private duringInit = true;
  private selectedVerticalPosition: "top" | "bottom";
  private selectedHorizontalPosition: "left" | "right";
  private calculationsToPerform: Set<"horizontal"|"vertical">;

  private triggerPosition: coordinates & { absoluteTop?: number, absoluteLeft?: number, height?: number, width?: number } = { };  

  private isContainerBody: boolean;
  private isContainerOverBody = false;
  private isTriggerFixed: boolean; //refers to trigger or any of its parent having "position:fixed"
  private lastScrollPosition: number; //used only if isTriggerFixed === true

  private scrollbarSize: {
    horizontalHeight: number,
    verticalWidth: number
  }  

  constructor(blazorId: string,
    overlay: HTMLDivElement, container: HTMLElement, trigger: HTMLElement, placement: Placement, 
    triggerBoundyAdjustMode: TriggerBoundyAdjustMode, triggerIsWrappedInDiv: boolean, triggerPrefixCls: string,
    overlayConstraints: overlayConstraints) {
    this.blazorId = blazorId;
    this.overlay = overlay;  
    //containerInfo & scrollbars have to be obtained here, because after
    //removal of classes, the overlay goes to left: -9999 what causes artificial 
    //scrollbars and viewport dimensions are changing
    this.containerInfo = domInfoHelper.getInfo(container);
    this.container = container;
    this.isContainerBody = container === document.body;
    this.calculateScrollBarSizes()
    if (!this.isContainerBody) {
      this.isContainerOverBody = domInfoHelper.findAncestorWithZIndex(this.container) > 0;
    }

    this.overlay.style.cssText = this.overlay.style.cssText.replace("display: none;", "");
    this.overlay.style.top = "0px"; //reset to prevent scrollbars if do not exist
    this.removeHiddenClass()

    //The trigger is actually wrapping div, which can have its own dimensions (coming from styles).
    //So, first valid HTML element is picked and if there is none, the wrapping div is set as trigger.
    //Triggers are always wrapped in div if the <ChildElement> instead of <Unbound> is used.
    this.trigger = Overlay.getFirstValidChild(trigger, triggerIsWrappedInDiv);
    this.triggerPrefixCls = triggerPrefixCls;
    if (overlayConstraints.arrowPointAtCenter){      
      this.placement = Overlay.arrowCenterPlacementMatch.get(placement);
    } else {
      this.placement = placement;
    }
    this.initialPlacement = this.placement;
    this.boundyAdjustMode = triggerBoundyAdjustMode;    
    this.overlayConstraints = overlayConstraints;

    this.position = { zIndex: 0 };
    this.selectedHorizontalPosition = Overlay.appliedStylePositionMap.get(this.placement).horizontal;
    this.selectedVerticalPosition = Overlay.appliedStylePositionMap.get(this.placement).vertical;

    this.verticalCalculation = Overlay.setVerticalCalculation(this.placement, this.selectedVerticalPosition);
    this.horizontalCalculation = Overlay.setHorizontalCalculation(this.placement, this.selectedHorizontalPosition);
    this.isTriggerFixed = domInfoHelper.isFixedPosition(this.trigger);
    this.scrollableContainers = domInfoHelper.getScrollableParents(this.trigger);
    this.observe();
  }

  static getFirstValidChild(element: HTMLElement, triggerIsWrappedInDiv: boolean): HTMLElement {
    if (triggerIsWrappedInDiv)
    {      
      for(let i = 0; i < element.childNodes.length; i++) {
        const childElement = element.childNodes[i] as HTMLElement;
        if (childElement.innerHTML)
          return childElement;
      }
    }
    return element
  }

  static setVerticalCalculation(placement: Placement, position: "top" | "bottom") {
    if (position === "top") {
      switch (placement) {
        case Placement.LeftTop:
        case Placement.RightTop:          
          return function(triggerTop: number, triggerHeight: number, container: domTypes.domInfo, trigger: domTypes.domInfo, overlayHeight: number, constraints: overlayConstraints) {               
            return { 
              top: triggerTop,
              bottom: Overlay.reversePositionValue(triggerTop, container.scrollHeight, overlayHeight) 
            };              
          }; 
        case Placement.BottomLeft:
        case Placement.Bottom:
        case Placement.BottomRight:
          return function(triggerTop: number, triggerHeight: number, container: domTypes.domInfo, trigger: domTypes.domInfo, overlayHeight: number, constraints: overlayConstraints) { 
            const position: verticalPosition = {
              top: triggerTop + triggerHeight + constraints.verticalOffset,
            };
            position.bottom = Overlay.reversePositionValue(position.top, container.scrollHeight, overlayHeight)
            return position;
          }; 
        case Placement.Left:
        case Placement.Right:
          return function(triggerTop: number, triggerHeight: number, container: domTypes.domInfo, trigger: domTypes.domInfo, overlayHeight: number, constraints: overlayConstraints) {     
            const position: verticalPosition = { 
              top: triggerTop + (triggerHeight / 2) - (overlayHeight / 2)
            };
            position.bottom = Overlay.reversePositionValue(position.top, container.scrollHeight, overlayHeight)
            return position;
          }; 
      }
    }    
    if (position === "bottom") {
      switch (placement) {
        case Placement.TopLeft:
        case Placement.Top:
        case Placement.TopRight:
          return function(triggerBottom: number, triggerHeight: number, container: domTypes.domInfo, trigger: domTypes.domInfo, overlayHeight: number, constraints: overlayConstraints) { 
            const position: verticalPosition = { 
              bottom: triggerBottom + triggerHeight + constraints.verticalOffset,
            };
            position.top = Overlay.reversePositionValue(position.bottom, container.scrollHeight, overlayHeight);
            return position;
          };           
        case Placement.LeftBottom:
        case Placement.RightBottom:
          return function(triggerBottom: number, triggerHeight: number, container: domTypes.domInfo, trigger: domTypes.domInfo, overlayHeight: number, constraints: overlayConstraints) {  
            const position: verticalPosition = { 
              bottom: triggerBottom,
              top: Overlay.reversePositionValue(triggerBottom, container.scrollHeight, overlayHeight)
            };            
            return position;                               
          };   
      }
    }
    //fallback - should not happen, but to avoid crashing scenarios, revert to BottomLeft
    console.log("Error: setVerticalCalculation did not match, nothing selected!!! Fallback.", placement, position);
    return Overlay.setVerticalCalculation(Placement.BottomLeft, "top");
  }

  static setHorizontalCalculation(placement: Placement, position: "left" | "right") {
    if (position === "left") {
      switch (placement) {
        case Placement.TopLeft:
        case Placement.BottomLeft:
          return function(triggerLeft: number, triggerWidth: number, container: domTypes.domInfo, trigger: domTypes.domInfo, overlayWidth: number, constraints: overlayConstraints) {        
            return { 
              left: triggerLeft,
              right: Overlay.reversePositionValue(triggerLeft, container.scrollWidth, overlayWidth)
            };              
          }; 
        case Placement.Right:
        case Placement.RightTop:
        case Placement.RightBottom:
          return function(triggerLeft: number, triggerWidth: number, container: domTypes.domInfo, trigger: domTypes.domInfo, overlayWidth: number, constraints: overlayConstraints) {        
            const position: horizontalPosition = {
              left: triggerLeft + triggerWidth + constraints.horizontalOffset
            };
            position.right = Overlay.reversePositionValue(position.left, container.scrollWidth, overlayWidth)
            return position;
          };

        case Placement.Top:
        case Placement.Bottom:
          return function(triggerLeft: number, triggerWidth: number, container: domTypes.domInfo, trigger: domTypes.domInfo, overlayWidth: number, constraints: overlayConstraints) {        
            const position: horizontalPosition = {
              left: triggerLeft + (triggerWidth / 2) - (overlayWidth / 2)
            };
            position.right = Overlay.reversePositionValue(position.left, container.scrollWidth, overlayWidth)
            return position;
          };
      }
    }
    if (position === "right") {
      switch (placement) {
        case Placement.TopRight:
        case Placement.BottomRight:
          return function(triggerRight: number, triggerWidth: number, container: domTypes.domInfo, trigger: domTypes.domInfo, overlayWidth: number, constraints: overlayConstraints) {        
            const position: horizontalPosition = {
              right: triggerRight,
              left: Overlay.reversePositionValue(triggerRight, container.scrollWidth, overlayWidth)
            };            
            return position;
          };
        case Placement.Left:
        case Placement.LeftTop:
        case Placement.LeftBottom:
          return function(triggerRight: number, triggerWidth: number, container: domTypes.domInfo, trigger: domTypes.domInfo, overlayWidth: number, constraints: overlayConstraints) {
            const position: horizontalPosition = {
              right: triggerRight + triggerWidth + constraints.horizontalOffset
            };
            position.left = Overlay.reversePositionValue(position.right, container.scrollWidth, overlayWidth)
            return position;
          };          
      }      
    }
    //fallback - should not happen, but to avoid crashing scenarios, revert to BottomLeft
    console.log("Error: setHorizontalCalculation did not match, nothing selected!!! Fallback.", placement, position);
    return Overlay.setVerticalCalculation(Placement.BottomLeft, "top");    
  }

  /**
   * Calculates reversed position. So for given left will return right,
   * for top => bottom, etc.
   * @param the value that needs to be reversed (left in scenario: left => right)
   * @param for horizontal (left, right) container width & for vertical (top, bottom) container height
   * @param for horizontal (left, right) overlay width & for vertical (top, bottom) overlay height
   * @returns number
   */
  static reversePositionValue(position: number, containerDimension: number, overlayDimension: number) {    
    return containerDimension - position - overlayDimension;
  }

  private removeHiddenClass() {    
    const end = this.overlay.className.indexOf("-hidden");
    const start = this.overlay.className.lastIndexOf(" ", end)
    if (start >= 0) {
      const className = this.overlay.className.substr(start + 1, end);
      if (className !== "") {
        this.overlay.classList.remove(className);
      }
    }
  }

  private calculateScrollBarSizes() {
    if (this.isContainerBody) {
      this.scrollbarSize = { 
        horizontalHeight: window.innerHeight - document.documentElement.clientHeight,
        verticalWidth: window.innerWidth - document.documentElement.clientWidth
      }
    } 
    else {
      this.scrollbarSize = { 
        horizontalHeight: this.container.offsetHeight - this.container.clientHeight, 
        verticalWidth: this.container.offsetWidth - this.container.clientWidth 
      }
    }
  }

  private observe() {
    resize.create(`container-${this.blazorId}`, this.resizing.bind(this), false);
    resize.observe(`container-${this.blazorId}`, this.container);    
    resize.observe(`container-${this.blazorId}`, this.trigger);    
    mutation.create(`trigger-${this.blazorId}`, this.mutating.bind(this), false);
    mutation.observe(`trigger-${this.blazorId}`, this.trigger, {
      attributes: true,
      characterData: false,
      childList: false,
      subtree: false,
      attributeOldValue: false,
      characterDataOldValue: false
    });
    
    if (this.isContainerBody) {
      window.addEventListener("scroll", this.onScroll.bind(this));
    }
    else {
      this.container.addEventListener("scroll", this.onScroll.bind(this));
    }

    // for sometime the trigger would be scrolled by any parent contaniners but not body or the popup container.
    this.scrollableContainers.forEach(container => {
      container.addEventListener('scroll', this.onScroll.bind(this));
    });
  }  

  private onScroll() {
    if (this.isTriggerFixed) {
      if (this.lastScrollPosition !== window.pageYOffset) {      
        const diff = window.pageYOffset - this.lastScrollPosition; //positive -> down, negative -> up        
        this.position.top += diff;
        this.position.bottom = Overlay.reversePositionValue(this.position.top, this.containerInfo.scrollHeight, this.overlayInfo.clientHeight);      
        if (this.selectedVerticalPosition === "top") {        
          this.sanitizedPosition.top = this.position.top;
          this.overlay.style.top = this.sanitizedPosition.top + "px";        
        } else {
          this.sanitizedPosition.bottom = this.getAdjustedBottom();
          this.overlay.style.bottom = this.sanitizedPosition.bottom + "px";
        }
        this.lastScrollPosition = window.pageYOffset;
      }
    } else {
      //Commented out code is a non-optimized calculation only if overlay stops fitting during scroll
      //It misses active check for initialPlacement being different to current placement
      // this.getKeyElementDimensions(false);
      // this.containerBoundarySize = this.getContainerBoundarySize();
      // if (!this.overlayFitsContainer("horizontal", this.position.left, this.position.right)
      //   || !this.overlayFitsContainer("vertical", this.position.top, this.position.bottom)) {    
      //     this.calculatePosition(true, false, this.overlayPreset)
      // }    
      this.calculatePosition(true, false, this.overlayPreset);
    }
  }

  private resizing(entries, observer) {
    //prevents from recalculation right on the spot during constructor run
    if (this.duringInit) { 
      this.duringInit = false;
      return;
    }
    this.calculatePosition(true, false, this.overlayPreset);
  }

  private lastStyleMutation = "";

  /**
   * Mutation observer will fire whenever trigger style changes. This is first and foremost
   * to monitor position/size changes, so overlay can adjust itself to the new position.
   * @param mutations 
   * @returns 
   */
  private mutating(mutations) {
    if (this.duringInit) { 
      this.duringInit = false;
      return;
    }    
    if (this.lastStyleMutation !== this.trigger.style.cssText) {
      this.lastStyleMutation = this.trigger.style.cssText;
      this.calculatePosition(true, false, this.overlayPreset);
    }
    
  }

  public dispose(): void {    
    resize.dispose(`container-${this.blazorId}`);
    mutation.dispose(`trigger-${this.blazorId}`);
    if (this.container.contains(this.overlay)) {
      this.container.removeChild(this.overlay);
    }

    if (this.isContainerBody) {      
      window.removeEventListener("scroll", this.onScroll);
    }
    else {
      this.container.removeEventListener("scroll", this.onScroll);
    }

    this.scrollableContainers.forEach(container => {
      container.removeEventListener('scroll', this.onScroll);
    });
  }

  public calculatePosition(applyLocation: boolean, firstTime = false, overlayPreset?: domTypes.position): overlayPosition {        
    //check if hidden, if yes, no need to recalculate (only if not first time)
    if (!firstTime && !this.overlay.offsetParent) {      
      return;
    }
    //trigger no longer visible, hide
    if (!overlayPreset && !this.trigger.offsetParent) {      
      if (!this.overlay.classList.contains(this.triggerPrefixCls + "-hidden")) {
        this.overlay.classList.add(this.triggerPrefixCls + "-hidden");
      }
      return this.position;
    }
    this.lastScrollPosition = window.pageYOffset;
    this.recentPlacement = this.placement;
    this.overlayPreset = overlayPreset;

    this.getKeyElementDimensions(firstTime);

    this.restoreInitialPlacement();

    //add a very basic check - if overlay width exceeds container width, left defaults to 0     
    this.calculationsToPerform = this.getNominalPositions();
    if (this.calculationsToPerform.size > 0) {
      this.adjustToContainerBoundaries();
    }

    this.sanitizeCalculatedPositions();    
    //first positioning is applied by blazor - without it, a flicker is visible
    if (applyLocation) {
      this.applyLocation();
    }    
    return this.sanitizedPosition;
  }

  /**
   * All variants of positions are stored during calculations, but only key positions are 
   * returned (so only left or right and only top or bottom).
   * Also, bottom & right positions need to be recalculated, due to the fact that during 
   * calculations:
   *  - bottom is represented as a value counting from top 
   *  - right is represented as a value counting from left
   * Browsers use different reference for bottom & right.
   */
  private sanitizeCalculatedPositions() {
    this.sanitizedPosition = { ...this.position};
    this.sanitizedPosition.zIndex = domInfoHelper.getMaxZIndex();
    this.sanitizedPosition.placement = this.placement;    
    if (this.selectedHorizontalPosition === "left") {
      this.sanitizedPosition.right = null;
    }
    else {
      this.sanitizedPosition.left = null;
      this.sanitizedPosition.right = this.getAdjustedRight();
    }

    if (this.selectedVerticalPosition === "top") {
      this.sanitizedPosition.bottom = null;
    }
    else {
      this.sanitizedPosition.top = null;
      this.sanitizedPosition.bottom = this.getAdjustedBottom();
    }
  }

  /**
   * Gets first calculations of the overlay. For each direction, there is a single scenario
   * when it is immediately known that no further calculation is needed:
   * - for vertical direction - when overlay's height is larger than container vertical boundaries
   * - for vertical direction - when overlay's width is larger than container horizontal boundaries
   * These scenarios are ignored, when boundyAdjustMode === TriggerBoundyAdjustMode.None
   * @returns collection containing directions that will be calculable (not final)
   */
  private getNominalPositions(): Set<"horizontal"|"vertical"> {
    this.containerBoundarySize = this.getContainerBoundarySize();
    const height = this.containerBoundarySize.bottom - this.containerBoundarySize.top;
    const width = this.containerBoundarySize.right - this.containerBoundarySize.left;
    const directionsToCalculate = new Set<"horizontal"|"vertical">();

    if (this.boundyAdjustMode != TriggerBoundyAdjustMode.None && width < this.overlayInfo.clientWidth && this.isContainerBody) {
      if (this.selectedHorizontalPosition === "left") {
        this.position.left = 0;
      } else {
        this.position.right = 0;
      }
    } else {
      const horizontalPosition = this.getHorizontalPosition();
      this.position.left = horizontalPosition.left;
      this.position.right = horizontalPosition.right;
      directionsToCalculate.add("horizontal");
    }
    //same for height exceeding container height - top defaults to 0   
    if (this.boundyAdjustMode != TriggerBoundyAdjustMode.None && height < this.overlayInfo.clientHeight && this.isContainerBody) {      
      if (this.selectedVerticalPosition === "top") {
        this.position.top = 0;
      } else {
        this.position.bottom = 0;
      }
    } else {
      
      const verticalPosition = this.getVerticalPosition();
      this.position.top = verticalPosition.top;
      this.position.bottom = verticalPosition.bottom;      
      directionsToCalculate.add("vertical");
    }
    return directionsToCalculate;
  }

  /**
   * Restore initial placement (and following connected variables & functions) on calculation.
   * This never kicks in on first calculation. This is done because the overlay should always
   * try to position itself to the initial placement. So on every recalculation initial settings
   * (used during object creation) are reloaded.
   */
  private restoreInitialPlacement() {
    if (this.placement !== this.initialPlacement) {
      this.placement = this.initialPlacement;
      this.selectedHorizontalPosition = Overlay.appliedStylePositionMap.get(this.placement).horizontal;
      this.selectedVerticalPosition = Overlay.appliedStylePositionMap.get(this.placement).vertical;
      this.verticalCalculation = Overlay.setVerticalCalculation(this.placement, this.selectedVerticalPosition);
      this.horizontalCalculation = Overlay.setHorizontalCalculation(this.placement, this.selectedHorizontalPosition);
    }
  }

  /**
   * Very basic logging, useful during debugging.
   * @param extraMessage 
   */
  /* istanbul ignore next */
  private logToConsole(extraMessage = "") {
    console.log(extraMessage + " Overlay position:", this.position,
      "Input",
      { 
        blazorId: this.blazorId,        
        container: {
          info: this.containerInfo,
          parentInfo: {
            clientHeight: this.container.parentElement.clientHeight,
            clientWidth: this.container.parentElement.clientWidth,
            scrollLeft: this.container.parentElement.scrollLeft,
            scrollTop: this.container.parentElement.scrollTop
          },
          containerId: this.container.id,        
          containerBoundarySize: this.containerBoundarySize,
        },
        trigger: { 
          absoluteTop: this.triggerInfo.absoluteTop,
          absoluteLeft: this.triggerInfo.absoluteLeft,
          clientHeight: this.triggerInfo.clientHeight,
          clientWidth: this.triggerInfo.clientWidth,
          offsetHeight: this.triggerInfo.offsetHeight,
          offsetWidth: this.triggerInfo.offsetWidth,
          boundyAdjustMode: this.boundyAdjustMode,
          //triggerType: this.triggerType,
          triggerHtml: this.trigger.outerHTML,
          triggerPrefixCls: this.triggerPrefixCls
        },
        overlay: { 
          clientHeight: this.overlayInfo.clientHeight,
          clientWidth: this.overlayInfo.clientWidth,
          offsetHeight: this.overlayInfo.offsetHeight,
          offsetWidth: this.overlayInfo.offsetWidth,
          class: this.overlay.className,
          appliedCssPosition: {
            overlay_style_top: this.overlay.style.top,
            overlay_style_bottom: this.overlay.style.bottom,
            overlay_style_left: this.overlay.style.left,
            overlay_style_right: this.overlay.style.right
          }
        },
        window: {
          innerHeight: window.innerHeight,
          innerWidth: window.innerWidth,
          pageXOffset: window.pageXOffset,
          pageYOffset: window.pageYOffset,
        },        
        documentElement: {
          clientHeight: document.documentElement.clientHeight,
          clientWidth: document.documentElement.clientWidth,
          containerIsBody: this.isContainerBody,
        },
        scrollbars: this.scrollbarSize,
        overlayPreset: this.overlayPreset,
        overlayConstraints: this.overlayConstraints,
        position: this.position,           
        sanitizedPosition: this.sanitizedPosition,
        placment: {
          initialPlacement: this.initialPlacement,
          recentPlacement: this.recentPlacement,
          placement: this.placement,
          selectedHorizontalPosition: this.selectedHorizontalPosition,
          selectedVerticalPosition: this.selectedVerticalPosition
        }
      }
    );    
  }

  /**
   * Right in the class is calculated with assumption that it is just reversed Left. 
   * This works well for containers that are not body. When in body, then different Right 
   * calculation is executed. Example:
   * In a document of width of 5000px, the first Left = 0 and the first Right = 0 as well 
   * (and respectively, max Left = 5000 and max Right = 5000). However, browsers are behaving 
   * differently. Left indeed is 0 until the document width (5000). Right however is different. 
   * Right = 0 means the point of original viewport most Right. So, if you viewport is 1000px 
   * wide, Right = 0 will mean same as Left = 1000. So to reach Left = 5000, Right has to 
   * be equal to -4000. 
   * @returns number - right position
   */
  private getAdjustedRight(): number {    
    if (this.isContainerBody) {
      return this.position.right - (this.containerInfo.scrollWidth - window.innerWidth)
        - this.scrollbarSize.verticalWidth;
    }
    return this.position.right;    
  }

  /**
   * Bottom in the class is calculated with assumption that it is just reversed Top. 
   * This works well for containers that are not body. When in body, then different Bottom
   * calculation is executed. Example:
   * In a document of height of 5000px, the first Top = 0 and the first Bottom = 0 as well 
   * (and respectively, max Top = 5000 and max Bottom = 5000). However, browsers are behaving 
   * differently. Top indeed is 0 until the document height (5000). Bottom however is different. 
   * Bottom = 0 means the point of original viewport most bottom. So, if you viewport is 1000px 
   * in height, Bottom = 0 will mean same as Top = 1000. So to reach Top = 5000, Bottom has to 
   * be equal to -4000. 
   * @returns number - bottom position
   */
  private getAdjustedBottom(): number {    
    if (this.isContainerBody) {
      return this.position.bottom - (this.containerInfo.scrollHeight - window.innerHeight)
        - this.scrollbarSize.horizontalHeight;
    }
    return this.position.bottom;
  }

  private applyLocation() {
    if (this.selectedHorizontalPosition === "left") {
      this.overlay.style.left = this.sanitizedPosition.left + "px";
      this.overlay.style.right = "unset";
    } else {
      this.overlay.style.right = this.sanitizedPosition.right + "px";
      this.overlay.style.left = "unset";
    }

    if (this.selectedVerticalPosition === "top") {
      this.overlay.style.top = this.sanitizedPosition.top + "px";
      this.overlay.style.bottom = "unset";
    } else {
      this.overlay.style.bottom = this.sanitizedPosition.bottom + "px";
      this.overlay.style.top = "unset";
    }

    this.applyPlacement();
  }

  private applyPlacement() {
    if (this.recentPlacement !== this.placement) {
      let currentPlacement: string;
      const stringMach = `${this.triggerPrefixCls}-placement-`;
      const start = this.overlay.className.indexOf(stringMach);
      const end = this.overlay.className.indexOf(" ", start + stringMach.length);
      if (start >= 0) {
        currentPlacement = this.overlay.className.substr(start, end-start);
      } else {
        currentPlacement = Overlay.appliedStylePositionMap.get(this.initialPlacement).class;
      }
      const newPlacement = stringMach + Overlay.appliedStylePositionMap.get(this.placement).class;
      this.overlay.classList.replace(currentPlacement, newPlacement);
    }
  }

  /**
   * Loads all important dimensions of the key elements (container of the trigger, trigger & overlay)
   * into domType.domInfo structures. This could be accessed directly, except absolute positions.
   * Also simplifies mocking.
   * @param firstTime - if this method is called first time, then no need to load information on
   *  container, as it was already loaded in the constructor. This is due to the fact that first time,
   *  when overlay is added it has default left set to -9999 which causes the scrollbars to 
   * appear (which will be gone by the time overlay becomes visible). Scrollbars change
   *  dimensions, so often calculations were incorrect.
   */
  private getKeyElementDimensions(firstTime: boolean) {
    if (!firstTime) {
      this.containerInfo = domInfoHelper.getInfo(this.container);
      this.calculateScrollBarSizes()
    }    
    this.triggerInfo = domInfoHelper.getInfo(this.trigger);
    this.overlayInfo = domInfoHelper.getInfo(this.overlay);    
  }

  /**
   * Calculates trigger top & bottom positions and trigger height and
   * uses these to return nominal position values depending on placement and
   * expected attachment point (top/bottom)
   * @returns verticalPosition
   */
  private getVerticalPosition(): verticalPosition {
    let position: verticalPosition;
    //usually first offsetHeight is taken, as the measurement contains the borders
    this.triggerPosition.height = this.triggerInfo.offsetHeight != 0 ? this.triggerInfo.offsetHeight 
      : this.triggerInfo.clientHeight;    
    if (this.overlayPreset) {
      this.triggerPosition.top = this.triggerInfo.absoluteTop + this.overlayPreset.y;
      this.triggerPosition.height = 0;
    } else {
      this.triggerPosition.top = this.containerInfo.scrollTop + this.triggerInfo.absoluteTop
        - this.containerInfo.absoluteTop - this.containerInfo.clientTop;
    }
    this.triggerPosition.absoluteTop = this.triggerInfo.absoluteTop;

    if (this.selectedVerticalPosition === "top"){
      position = this.verticalCalculation(this.triggerPosition.top, this.triggerPosition.height, this.containerInfo,
        this.triggerInfo, this.overlayInfo.clientHeight, this.overlayConstraints);
    }
    else { //bottom
      this.triggerPosition.bottom = this.containerInfo.scrollHeight - this.triggerPosition.top - this.triggerPosition.height;
      position = this.verticalCalculation(this.triggerPosition.bottom, this.triggerPosition.height, this.containerInfo,
        this.triggerInfo, this.overlayInfo.clientHeight, this.overlayConstraints);
    }
    return position;
  }

  /**
   * Calculates trigger left & right positions and trigger width and
   * uses these to return nominal position values depending on placement and
   * expected attachment point (left/right)
   * @returns verticalPosition
   */
  private getHorizontalPosition(): horizontalPosition {
    let position: horizontalPosition;
    //usually first offsetHeight is taken, as the measurement contains the borders    
    this.triggerPosition.width = this.triggerInfo.offsetWidth != 0 ? this.triggerInfo.offsetWidth : this.triggerInfo.clientWidth;
    
    //let triggerLeft: number;
    if (this.overlayPreset) {
      this.triggerPosition.left = this.triggerInfo.absoluteLeft + this.overlayPreset.x;
      this.triggerPosition.width = 0;
    } else {
      this.triggerPosition.left = this.containerInfo.scrollLeft + this.triggerInfo.absoluteLeft
        - this.containerInfo.absoluteLeft - this.containerInfo.clientLeft;
    }
    this.triggerPosition.absoluteLeft = this.triggerInfo.absoluteLeft;

    if (this.selectedHorizontalPosition === "left"){      
      position = this.horizontalCalculation(this.triggerPosition.left, this.triggerPosition.width, this.containerInfo,
        this.triggerInfo, this.overlayInfo.clientWidth, this.overlayConstraints);
    }
    else { //right
      this.triggerPosition.right = this.containerInfo.scrollWidth - this.triggerPosition.left - this.triggerPosition.width;
      position = this.horizontalCalculation(this.triggerPosition.right, this.triggerPosition.width, this.containerInfo,
        this.triggerInfo, this.overlayInfo.clientWidth, this.overlayConstraints);
    }
    return position;    
  }

  /**
   * Responsible for calling logic that handles situation when calculated overlay position
   * is causing overlay to be partially rendered invisible. The goal is to adjust placement 
   * in such a way, so the overlay is fully visible.
   * @returns void
   */
  private adjustToContainerBoundaries() {
    if (this.boundyAdjustMode === TriggerBoundyAdjustMode.None) {
      return;
    }    

    if (this.calculationsToPerform.has("vertical")) {
      this.adjustVerticalToContainerBoundaries();
    }

    if (this.calculationsToPerform.has("horizontal")) {
      this.adjustHorizontalToContainerBoundaries();
    }
  }

  private setBodyBoundayrSize() {
    const window = domInfoHelper.getWindow();      
    const scroll = domInfoHelper.getScroll();
    this.bodyBoundarySize = {
      top : scroll.y,
      left: scroll.x,
      right: window.innerWidth + scroll.x,
      bottom: window.innerHeight + scroll.y
    };     
  }

  /**
   * Retrieves information on current logical viewport (visible area). For 
   * InView this means actual viewport area (what you see in the browser - either the 
   * body or the scrolled to area in a container) or for InScroll this means total
   * area of the container (or body).
   * @returns coordinates - absolute values measuring from top = 0 and left = 0 (first 
   * pixels of the container)
   */
  private getContainerBoundarySize(): coordinates {    
    if (this.boundyAdjustMode === TriggerBoundyAdjustMode.InScroll) {      
      if (!this.isContainerBody) {
        this.setBodyBoundayrSize();
      }
      return { 
        left: 0,
        right: this.containerInfo.scrollWidth,
        top: 0,
        bottom: this.containerInfo.scrollHeight
      };
    }        
    this.setBodyBoundayrSize();

    if (this.isContainerBody) {       
      return this.bodyBoundarySize;
    } else {         
      //special care is needed when evaluating viewport of the container
      const parentIsInsignificant = this.container.parentElement.clientHeight === 0
        || this.container.parentElement.clientWidth === 0;
      const verticalScrollBasedOnParent = !parentIsInsignificant
        && this.container.parentElement.clientHeight < this.containerInfo.clientHeight;
      const horizontalScrollBasedOnParent = !parentIsInsignificant
        && this.container.parentElement.clientWidth < this.containerInfo.clientWidth;

      let clientHeight: number;
      let clientWidth: number;
      let scrollTop: number;
      let scrollLeft: number;

      if (verticalScrollBasedOnParent) {
        clientHeight = this.container.parentElement.clientHeight;
        scrollTop = this.container.parentElement.scrollTop;
      } else {
        clientHeight = this.containerInfo.clientHeight;
        scrollTop = this.containerInfo.scrollTop;
      }

      if (horizontalScrollBasedOnParent) {
        clientWidth = this.container.parentElement.clientWidth;clientWidth;
        scrollLeft = this.container.parentElement.scrollLeft;
      } else {
        clientWidth = this.containerInfo.clientWidth;
        scrollLeft = this.containerInfo.scrollLeft;
      }
          
      return { 
        top : scrollTop,
        bottom: scrollTop + clientHeight,
        left: scrollLeft,
        right: scrollLeft + clientWidth
      };                
    }  
  }

  /**
   * Returns how much height of the overlay is visible in current viewport 
   */
  private getOverlayVisibleHeight(visibleIn: "container" | "body"): number {
    let boundary: coordinates;
    let top: number;    
    if (visibleIn === "container") {
      boundary = this.containerBoundarySize;
      top = this.triggerPosition.top;
    } else {
      boundary = this.bodyBoundarySize;
      top = this.triggerPosition.absoluteTop;
    }

    if (this.selectedVerticalPosition === "top") {
      return boundary.bottom - (top + this.triggerPosition.height);
    } else {
      return top - boundary.top;
    }    
  }

  /**
   * Returns how much width of the overlay is visible in current viewport 
   */
  private getOverlayVisibleWidth(visibleIn: "container" | "body"): number {    
    let boundary: coordinates;
    let left: number;    
    if (visibleIn === "container") {
      boundary = this.containerBoundarySize;
      left = this.triggerPosition.left;
    } else {
      boundary = this.bodyBoundarySize;
      left = this.triggerPosition.absoluteLeft;
    }

    if (this.selectedHorizontalPosition === "left") {
      return boundary.right - (left + this.triggerPosition.width);
    } else {
      return left - boundary.left;
    }
  }

  /**
   * Checks if current position actually fits in the container and if not, then reverses
   * the placement. Then calculates which already calculated placements has the largest horizontal 
   * area of the overlay visible and picks the calculation with largest area.
   */
  private adjustHorizontalToContainerBoundaries() {
    if (!this.overlayFitsContainer("horizontal", this.position.left, this.position.right)) {
      const positionCache: overlayPosition = { ...this.position };
      const selectedPositionCache = this.selectedHorizontalPosition;
      const placementCache = this.placement;
      const horizontalCalculationCache = this.horizontalCalculation;
      const visibleWidthBeforeAdjustment = this.getOverlayVisibleWidth("container");
      let visibleWidthInBodyBeforeAdjustment: number;
      if (this.isContainerOverBody) {
        visibleWidthInBodyBeforeAdjustment = this.getOverlayVisibleWidth("body");
      } else {
        visibleWidthInBodyBeforeAdjustment = visibleWidthBeforeAdjustment
      }      

      this.getHorizontalAdjustment();

      const visibleWidthAfterAdjustment = this.getOverlayVisibleWidth("container");
      let visibleWidthInBodyAfterAdjustment: number;
      if (this.isContainerOverBody) {
        visibleWidthInBodyAfterAdjustment = this.getOverlayVisibleWidth("body");
      } else {
        visibleWidthInBodyAfterAdjustment = visibleWidthAfterAdjustment
      }        

      if (
        !(visibleWidthInBodyBeforeAdjustment < visibleWidthInBodyAfterAdjustment 
          && visibleWidthInBodyAfterAdjustment > 0
          && visibleWidthInBodyAfterAdjustment - visibleWidthInBodyBeforeAdjustment >= 0)        
        ||
        !(visibleWidthBeforeAdjustment < visibleWidthAfterAdjustment && visibleWidthAfterAdjustment > 0)) {        
        this.position = positionCache;
        this.selectedHorizontalPosition = selectedPositionCache;
        this.placement = placementCache;
        this.horizontalCalculation = horizontalCalculationCache;
      }
    }
  }

  /**
   * Checks if current position actually fits in the container and if not, then reverses
   * the placement. Then calculates which already calculated placements has the largest vertical 
   * area of the overlay visible and picks the calculation with largest area.
   */
  private adjustVerticalToContainerBoundaries() {
    if (!this.overlayFitsContainer("vertical", this.position.top, this.position.bottom)) {
      const positionCache: overlayPosition = { ...this.position };
      const selectedPositionCache = this.selectedVerticalPosition;
      const placementCache = this.placement;
      const verticalCalculationCache = this.verticalCalculation;
      const visibleHeightBeforeAdjustment = this.getOverlayVisibleHeight("container");
      let visibleHeightInBodyBeforeAdjustment: number;
      if (this.isContainerOverBody) {
        visibleHeightInBodyBeforeAdjustment = this.getOverlayVisibleHeight("body");
      } else {
        visibleHeightInBodyBeforeAdjustment = visibleHeightBeforeAdjustment
      }

      this.getVerticalAdjustment();      

      const visibleHeightAfterAdjustment = this.getOverlayVisibleHeight("container");
      let visibleHeightInBodyAfterAdjustment: number;
      if (this.isContainerOverBody) {
        visibleHeightInBodyAfterAdjustment = this.getOverlayVisibleHeight("body");
      } else {
        visibleHeightInBodyAfterAdjustment = visibleHeightAfterAdjustment
      }          

      if (
        !(visibleHeightInBodyBeforeAdjustment < visibleHeightInBodyAfterAdjustment 
          && visibleHeightInBodyAfterAdjustment > 0 
          && visibleHeightInBodyAfterAdjustment - visibleHeightInBodyBeforeAdjustment >= 0)
        ||
        !(visibleHeightBeforeAdjustment < visibleHeightAfterAdjustment && visibleHeightAfterAdjustment > 0)) {        
        this.position = positionCache;
        this.selectedVerticalPosition = selectedPositionCache;
        this.placement = placementCache;
        this.verticalCalculation = verticalCalculationCache;
      }
    }
  }

  private overlayFitsContainer(type: "horizontal" | "vertical", start: number, end: number): boolean {    
    if (type === "horizontal") {
      const endExpressedAsLeft = start + this.overlayInfo.clientWidth;
      return this.containerBoundarySize.left <= start
          && start <= this.containerBoundarySize.right //overlay left is between container left and right
          && this.containerBoundarySize.left <= endExpressedAsLeft
          && endExpressedAsLeft <= this.containerBoundarySize.right //and overlay right is between container left and right
    }
    const endExpressedAsTop = start + this.overlayInfo.clientHeight;
    return this.containerBoundarySize.top <= start
        && start <= this.containerBoundarySize.bottom //overlay top is between container top and bottom
        && this.containerBoundarySize.top <= endExpressedAsTop 
        && endExpressedAsTop <= this.containerBoundarySize.bottom //and overlay bottom is between container top and bottom
  }

  /**
   * Applies basic adjustment - switches verticaly placement (top -> bottom & bottom -> top) 
   * and recalculates based on the newly set placement 
   */
  private getVerticalAdjustment() {
    this.placement = Overlay.reverseVerticalPlacementMap.get(this.placement)(this.selectedVerticalPosition);
    this.selectedVerticalPosition = Overlay.appliedStylePositionMap.get(this.placement).vertical;
    this.verticalCalculation = Overlay.setVerticalCalculation(this.placement, this.selectedVerticalPosition);
    const verticalPosition = this.getVerticalPosition();
    this.position.top = verticalPosition.top;
    this.position.bottom = verticalPosition.bottom;
  }

  /**
   * Applies basic adjustment - switches horizontal placement (left -> right & right -> left)
   * and recalculates based on the newly set placement
   */
  private getHorizontalAdjustment() {
    this.placement = Overlay.reverseHorizontalPlacementMap.get(this.placement)(this.selectedHorizontalPosition);
    this.selectedHorizontalPosition = Overlay.appliedStylePositionMap.get(this.placement).horizontal;
    this.horizontalCalculation = Overlay.setHorizontalCalculation(this.placement, this.selectedHorizontalPosition);
    const horizontalPosition = this.getHorizontalPosition();
    this.position.left = horizontalPosition.left;
    this.position.right = horizontalPosition.right;    
  }  
}