
interface DotNetInstance {
    invokeMethod(methodName: string, args: any)
}

class ResizableService {
    DotNet: DotNetInstance;

    init(instance: DotNetInstance) {
        this.DotNet = instance;
        this.addEvt();
    }

    dispose() {
        this.removeEvt();
    }

    private addEvt() {
        const body = document.body;
        body.addEventListener("mouseup", this.onMouseUp);
        body.addEventListener("mousemove", this.onMouseMove);
    }

    private removeEvt() {
        const body = document.body;
        body.removeEventListener("mouseup", this.onMouseUp);
        body.removeEventListener("mousemove", this.onMouseMove);
    }

    private onMouseUp(e: MouseEvent) {
        console.log(e);
        this.DotNet.invokeMethod("ClientMouseUp", e);
        if (!!this.DotNet) {
            this.DotNet.invokeMethod("ClientMouseUp", e);
        }
    }

    private onMouseMove(e: MouseEvent) {
        this.DotNet.invokeMethod("ClientMouseMove", e);
        if (!!this.DotNet) {
            this.DotNet.invokeMethod("ClientMouseMove", e);
        }
    }
}

export function startResize(instance) {
    console.log(instance);
    new ResizableService().init(instance);
}

export function endResize() {
    new ResizableService().dispose();
}