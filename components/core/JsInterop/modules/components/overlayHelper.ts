import { domInfoHelper, eventHelper, domManipulationHelper, domTypes } from '../dom/exports'
import { Placement, TriggerBoundyAdjustMode, overlayConstraints, overlayPosition, Overlay } from './overlay'
import { state } from '../stateProvider';

export class overlayHelper {
  static overlayRegistry: { [key: string]: Overlay} = {};

  static addOverlayToContainer(blazorId: string, 
    overlaySelector, triggerSelector, placement: Placement,  containerSelector: string,
    triggerBoundyAdjustMode: TriggerBoundyAdjustMode, triggerIsWrappedInDiv: boolean, triggerPrefixCls: string,
    verticalOffset: number, horizontalOffset: number, arrowPointAtCenter: boolean,    
    overlayTop?: number, overlayLeft?: number
  ): overlayPosition {      
    const overlayElement = domInfoHelper.get(overlaySelector) as HTMLDivElement;    
    const containerElement = domInfoHelper.get(containerSelector) as HTMLElement;
    const triggerElement = domInfoHelper.get(triggerSelector) as HTMLElement;    

    if (!domManipulationHelper.addElementTo(overlaySelector, containerElement)) {
      console.log("Failed to add overlay. Details:", {
        triggerPrefixCls: triggerPrefixCls,
        overlaySelector: overlaySelector,
        containerElement: containerElement
      } );
      return null;
    }    

    let overlayPresets: domTypes.position;
    if (overlayTop || overlayLeft) {
      overlayPresets = { x: overlayLeft, y: overlayTop };
    }

    const overlayConstraints: overlayConstraints = {
      verticalOffset: verticalOffset,
      horizontalOffset: horizontalOffset,
      arrowPointAtCenter: arrowPointAtCenter
    };

    const overlay = new Overlay(blazorId, overlayElement, containerElement, triggerElement, placement, triggerBoundyAdjustMode, triggerIsWrappedInDiv, triggerPrefixCls, overlayConstraints);   
    //register object in store, so it can be retrieved during update/dispose
    this.overlayRegistry[blazorId] = overlay;
    
    return overlay.calculatePosition(false, true, overlayPresets);
  }


  static updateOverlayPosition(blazorId: string, overlaySelector, triggerSelector, placement: Placement,  containerSelector: string,
    triggerBoundyAdjustMode: TriggerBoundyAdjustMode, triggerIsWrappedInDiv: boolean, triggerPrefixCls: string,
    verticalOffset: number, horizontalOffset: number, arrowPointAtCenter: boolean,  
    overlayTop?: number, overlayLeft?: number): overlayPosition {
    const overlay = this.overlayRegistry[blazorId];
    if (overlay){
      let overlayPresets: domTypes.position;
      if (overlayTop || overlayLeft) {
        overlayPresets = { x: overlayLeft, y: overlayTop };
      }      
      return overlay.calculatePosition(false, false, overlayPresets);      
    } else {
      //When page is slow, it may happen that rendering of an overlay may not happen, even if 
      //blazor thinks it did happen. In such a case, when overlay object is not found, just try
      //to render it again.
      return overlayHelper.addOverlayToContainer(blazorId, overlaySelector, triggerSelector, placement,  containerSelector,triggerBoundyAdjustMode, triggerIsWrappedInDiv, triggerPrefixCls, 
        verticalOffset, horizontalOffset, arrowPointAtCenter,  
        overlayTop, overlayLeft);      
    }    
  }

  static deleteOverlayFromContainer(blazorId: string) {
    const overlay = this.overlayRegistry[blazorId];
    if (overlay) {      
      overlay.dispose();
      delete this.overlayRegistry[blazorId];
    }
  }

  static addPreventEnterOnOverlayVisible(element, overlayElement) {
    if (element && overlayElement) {
      const dom: HTMLElement = domInfoHelper.get(element);
      if (dom) {
        state.eventCallbackRegistry[element.id + "keydown:Enter"] = (e) => eventHelper.preventKeyOnCondition(e, "enter", () => overlayElement.offsetParent !== null);
        dom.addEventListener("keydown", state.eventCallbackRegistry[element.id + "keydown:Enter"], false);
      }
    }
  }

  static removePreventEnterOnOverlayVisible(element) {
    if (element) {
      const dom: HTMLElement = domInfoHelper.get(element);
      if (dom) {
        dom.removeEventListener("keydown", state.eventCallbackRegistry[element.id + "keydown:Enter"]);
        state.eventCallbackRegistry[element.id + "keydown:Enter"] = null; 
      }
    }
  }
}

