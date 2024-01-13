export { state } from './modules/stateProvider';
export * as observable from './ObservableApi/observableApi';
export { domInfoHelper, domTypes, domManipulationHelper, eventHelper } from './modules/dom/exports';
export { styleHelper } from './modules/styleHelper';
export {
    backtopHelper,
    iconHelper,
    imageHelper,
    inputHelper,
    mentionsHelper,
    modalHelper,
    overlayHelper,
    tableHelper,
    uploadHelper,
    downloadHelper,
    watermarkHelper,
} from './modules/components/export'
export { enableDraggable, disableDraggable, resetModalPosition } from "./modules/dom/dragHelper";

export { generate as generateColor } from "@ant-design/colors";

import {modalHelper} from './modules/components/export'
import {manipulationHelper}from './modules/dom/manipulationHelper'

export function log(text) {
    console.log(text);
}

/**
 * 1. enableBodyScroll
 * 2. remove all old modal and drawer dom instance
 */
export function onLocationChanged() {
    manipulationHelper.enableBodyScroll(true)
    modalHelper.destroyAllDialog();
    document.querySelectorAll('.ant-drawer').forEach(e => document.body.removeChild(e));
}
