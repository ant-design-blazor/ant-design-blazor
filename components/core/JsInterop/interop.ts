export { state } from './modules/stateProvider';
export * as observable from './ObservableApi/observableApi';
export { domInfoHelper, domTypes, domManipulationHelper, eventHelper } from './modules/dom/exports';
export { styleHelper } from './modules/styleHelper';
export {
  backtopHelper,
  iconHelper,
  inputHelper,
  mentionsHelper,
  modalHelper,
  overlayHelper,
  tableHelper,
  uploadHelper
} from './modules/components/export'
export { enableDraggable, disableDraggable, resetModalPosition } from "./modules/dom/dragHelper";

export function log(text) {
  console.log(text);  
}
