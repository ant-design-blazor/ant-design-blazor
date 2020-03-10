import * as interop from './core/JsInterop/interop'

declare global {
  interface Window { antBlazor: any; }
}

window.antBlazor = {
  interop
}