import { domInfoHelper, domManipulationHelper } from "../dom/exports";

let restore = "";
export class bcdFormHelper {

    static onShow(formRoot: string | HTMLElement) {
        let rootEle = domInfoHelper.get(formRoot);
        // maybe dom not be rendered
        if (!rootEle) {
            setTimeout(() => bcdFormHelper.onShow(formRoot), 50);
        } else {
            bcdFormHelper.calcBcdFormRootOffset(formRoot);
        }
    }

    static minResetStyle(formRoot: string | HTMLElement, lastIsNormal: boolean) {
        let rootEle = domInfoHelper.get(formRoot);
        let lastNormalStyle: string | null = "";
        if (document.body.style.overflow !== 'auto') {
            restore = document.body.style.overflow;
            document.body.style.overflow = 'auto';
        }
        if (rootEle) {
            rootEle.style.top = "";
            rootEle.style.left = "";

            let form = rootEle.querySelector(".bcd-form") as HTMLElement;

            if (lastIsNormal) {
                lastNormalStyle = domInfoHelper.attr(form, "style");
            }

            form!.style.left = "";
            form!.style.top = "";
            form!.style.right = "";
        }
        return lastNormalStyle;
    }

    static maxResetStyle(formRoot: string | HTMLElement, lastIsNormal: boolean) {
        let rootEle = domInfoHelper.get(formRoot);
        let lastNormalStyle: string | null = "";
        if (rootEle) {
            let form = rootEle.querySelector(".bcd-form") as HTMLElement;
            if (lastIsNormal) {
                lastNormalStyle = domInfoHelper.attr(form, "style");
            }

            form!.style.left = "";
            form!.style.top = "";
        }
        bcdFormHelper.calcBcdFormRootOffset(formRoot);
        return lastNormalStyle;
    }


    static calcBcdFormRootOffset(formRoot: string | HTMLElement) {
        const scrollY = document.documentElement.scrollTop || document.body.scrollTop;
        const scrollX = document.documentElement.scrollLeft || document.body.scrollLeft;
        const roots = document.querySelectorAll(".bcd-form-container > div > .bcd-form-root");
        roots.forEach(e => {
            const ele = e as HTMLElement;
            ele.style.top = scrollY + "px";
            ele.style.left = scrollX + "px";
        });
    }
}
