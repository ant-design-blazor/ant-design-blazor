import { getBoundingClientRect } from "../interop";

class ResizableService {

    private static DotNetObjRef: any;

    private static that: any;

    static init(element: any, instance: any) {
        this.DotNetObjRef = instance;
        this.that = this;
        this.addEvt();
        return getBoundingClientRect(element);
    }

    static dispose() {
        this.removeEvt();
    }

    private static addEvt() {
        const body = document.body;
        body.addEventListener("mouseup", this.onMouseUp);
        body.addEventListener("mousemove", this.onMouseMove);
    }

    private static removeEvt() {
        const body = document.body;
        body.style.cursor = '';
        body.style.userSelect = '';
        body.removeEventListener("mouseup", this.onMouseUp);
        body.removeEventListener("mousemove", this.onMouseMove);
    }

    private static onMouseUp(e: MouseEvent) {
        if (!!ResizableService.that.DotNetObjRef) {
            ResizableService.that.DotNetObjRef.invokeMethodAsync("ClientMouseUp", e);
        }
    }

    private static onMouseMove(e: MouseEvent) {
        if (!!ResizableService.that.DotNetObjRef) {
            const data = {
                ClientX: e.clientX,
                ClientY: e.clientY
            };
            ResizableService.that.DotNetObjRef.invokeMethodAsync("ClientMouseMove", data);
        }
    }
}

export function startResize(element, instance) {
    ResizableService.init(element, instance);
}

export function endResize() {
    ResizableService.dispose();
}