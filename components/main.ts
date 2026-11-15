import "regenerator-runtime/runtime";
import * as interop from "./core/JsInterop/interop";

declare global {
  interface Window {
    AntDesign: any;
  }
}

window.AntDesign = {
  interop,
};
