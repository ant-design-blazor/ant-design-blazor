// This is a JavaScript module that is loaded on demand. It can export any number of
// functions, and may import other JavaScript modules if required.

//定义一个对象      

function stopEvent(evt) {
    var event = window.event ? window.event : evt;
    if (event.preventDefault) {
        event.preventDefault();
        event.stopPropagation();
    } else {
        event.returnValue = false;
    }
}

export class splitbar {
    el: Element;
    elementId: string;
    dragging: boolean;
    enableOverflow: boolean;
    clickPoint: Number;
    Max_Px: Number;
    Min_Px: Number;
    cssProp: string;
    isVertical: boolean;
    constructor(id: string, isVertical: boolean, enableOverflow: boolean) {
        let thisObject = this;
        thisObject.elementId = id;
        thisObject.isVertical = isVertical;
        thisObject.el = document.querySelector("#" + id);
        thisObject.dragging = false;
        //不允许超出前提 要有父级才能限制 使用flex布局 也会导致不能超出
        thisObject.enableOverflow = enableOverflow;
        thisObject.clickPoint = 0;
        thisObject.Max_Px = 0;
        thisObject.Min_Px = 20;//最小保留区域
        thisObject.cssProp = isVertical ? "height" : "width";
        if (!thisObject.enableOverflow) {
            if (!thisObject.el.parentElement) {
                thisObject.enableOverflow = false;
                throw Error("启用禁止超出属性时,必须给分割条配置父容器")
            }
            thisObject.Max_Px = thisObject.el.parentElement[isVertical ? "clientHeight" : "clientWidth"];
            if (thisObject.Max_Px < thisObject.Min_Px) {
                thisObject.Max_Px = thisObject.Min_Px
            }
        }
        thisObject.bind();
    }

    static CreateSplitbar(id: string, isVertical: boolean, enableOverflow: boolean) {
        var sb = new splitbar(id, isVertical, enableOverflow);
    }

    bind() {
        let thisObject = this;
        thisObject.el.addEventListener('mousedown', (e: MouseEvent) =>thisObject.mousedown(e), new eventOptions(thisObject.elementId));
        thisObject.el.addEventListener('mousedown', (e: MouseEvent) =>thisObject.mousedown(e), new eventOptions(thisObject.elementId));
        document.addEventListener("mousemove", (e: MouseEvent) =>thisObject.mousemove(e), new eventOptions(thisObject.elementId));
        document.addEventListener("mouseup", (e: MouseEvent) =>thisObject.mouseup(e), new eventOptions(thisObject.elementId));
    }
    dispose() {
        let thisObject = this;
        thisObject.el.removeEventListener('mousedown', (e: MouseEvent) => thisObject.mousedown(e), new eventOptions(thisObject.elementId));
        document.removeEventListener("mousemove", (e: MouseEvent) => thisObject.mousemove(e), new eventOptions(thisObject.elementId));
        document.removeEventListener("mouseup", (e: MouseEvent) => thisObject.mouseup(e), new eventOptions(thisObject.elementId));
        setTimeout(() => { thisObject = null; console.log("dispose") }, 0);
    }



    mousedown(e: MouseEvent) {
        stopEvent(e)
        let thisObject = this;
        thisObject.dragging = true;
        thisObject.clickPoint = thisObject.isVertical ? e.pageY : e.pageX;
    }
    mousemove(e) {
        let thisObject = this;

        if (thisObject.dragging) {
            if (thisObject != null) {

                if (!thisObject.el.previousElementSibling) return;
                var changeDistance = 0;
                var currentPoint = thisObject.isVertical ? e.pageY : e.pageX;

                //获取元素的高度 不包含margin padding
                var prevPoint = Number.parseInt(getComputedStyle(thisObject.el.previousElementSibling).getPropertyValue(thisObject.cssProp));
                //鼠标向距离
                changeDistance = Number(currentPoint) - Number(thisObject.clickPoint);
                prevPoint += changeDistance;
                var nextPoint = thisObject.ComputerNextElementPoint(changeDistance);
                if (!thisObject.enableOverflow) {
                    //必须前后两个元素一起调整 切不能超出父容器区域
                    var min = prevPoint < thisObject.Min_Px || nextPoint < thisObject.Min_Px;
                    var max = prevPoint > thisObject.Max_Px || nextPoint > thisObject.Max_Px;
                    if (min || max) {
                        return;
                    } else {
                        //设置后一个元素的样式
                        (<HTMLElement>thisObject.el.nextElementSibling).style[thisObject.cssProp] = nextPoint + "px";
                        //设置前一个元素的样式
                        (<HTMLElement>thisObject.el.previousElementSibling).style[thisObject.cssProp] = prevPoint + "px";
                    }

                } else {
                    if (nextPoint >= thisObject.Min_Px) {
                        //设置后一个元素的样式
                        (<HTMLElement>thisObject.el.nextElementSibling).style[thisObject.cssProp] = nextPoint + "px";
                    }
                    if (prevPoint >= thisObject.Min_Px) {
                        //设置前一个元素的样式
                        (<HTMLElement>thisObject.el.previousElementSibling).style[thisObject.cssProp] = prevPoint + "px";
                    }
                }
                thisObject.clickPoint = currentPoint;

            }
        }
    }

    mouseup(e) {
        let thisObject = this;

        thisObject.dragging = false;
        e.cancelBubble = true;
    }

    ComputerNextElementPoint(changeDistance) {
        let thisObject = this;

        if (thisObject.el.nextElementSibling) {
            var nextPoint = Number.parseInt(getComputedStyle(thisObject.el.nextElementSibling).getPropertyValue(thisObject.cssProp));
            //计算后一个元素的样式
            nextPoint -= changeDistance;
            return nextPoint;
        }
        return 0;
    }



}

class eventOptions implements EventListenerOptions {
    n: string
    capture?: boolean;
    constructor(id: string) {
        this.n=id
    }
}





