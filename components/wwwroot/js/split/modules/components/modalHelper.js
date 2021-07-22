import { domInfoHelper } from '../dom/exports';
export class modalHelper {
    static focusDialog(selector, count = 0) {
        let ele = document.querySelector(selector);
        if (ele && !ele.hasAttribute("disabled")) {
            setTimeout(() => {
                ele.focus();
                let curId = "#" + domInfoHelper.getActiveElement();
                if (curId !== selector) {
                    if (count < 10) {
                        this.focusDialog(selector, count + 1);
                    }
                }
            }, 10);
        }
    }
    static destroyAllDialog() {
        document.querySelectorAll('.ant-modal-root')
            .forEach(e => document.body.removeChild(e.parentNode));
    }
}
