import { domInfoHelper } from '../dom/exports'

export class bcdFormHelper {
    static minResetStyle(formRoot: string | HTMLElement, lastIsNormal: boolean) {
        let rootEle = domInfoHelper.get(formRoot);
        let lastNormalStyle: string | null = "";
        if (rootEle) {
            let form = rootEle.querySelector('.bcd-form') as HTMLElement;

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
            let form = rootEle.querySelector('.bcd-form') as HTMLElement;
            if (lastIsNormal) {
                lastNormalStyle = domInfoHelper.attr(form, "style");
            }

            form!.style.left = "";
            form!.style.top = "";
        }
        return lastNormalStyle;
    }

}