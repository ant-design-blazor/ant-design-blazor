import * as interop from "./interop";

declare global {
  interface Window {
    AntDesign: any;
  }
}

window.AntDesign = {
  interop,
};
