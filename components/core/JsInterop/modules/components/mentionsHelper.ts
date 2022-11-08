
export class mentionsHelper {

    public static setEditorKeyHandler = function (Mentions: any, textArea: HTMLTextAreaElement): void {
        textArea.onkeydown = async (ev): Promise<any> => {
            let pop = document.querySelector("ul.ant-mentions-dropdown-menu")
            //判断isPopShow不能用异步
            let isPopShow: boolean = pop != null;
            if (!isPopShow) return;
            if (ev.key == "ArrowUp") {
                ev.preventDefault();
                await Mentions.invokeMethodAsync("UpOption");
            } else if (ev.key == "ArrowDown") {
                ev.preventDefault();
                await Mentions.invokeMethodAsync("NextOption");
            }
            else if (ev.key == "Enter") {
                ev.preventDefault();
                await Mentions.invokeMethodAsync("EnterOption");
            }
            //其他按键在c#中处理
        }
    }

    public static getProp = function (e: HTMLElement, propName: string): any {
        return e[propName];
    }

    public static getCursorXY = function (textArea: HTMLTextAreaElement) {
        let format = function (value) {
            value = value.replace(/<|>|`|"|&/g, '?').replace(/\r\n|\r|\n/g, "<br/>");
            if (/firefox/i.test(navigator.userAgent)) {
                value = value.replace(/\s/g, '&nbsp;');
            }
            return value;
        };
        let inputorValue = textArea.value;
        let pos = textArea.selectionStart;
        let start_range = inputorValue.slice(0, pos);
        let end_range = inputorValue.slice(pos);
        let html = "<span style='position: relative; display: inline;'>" + format(start_range) + "</span>";
        html += "<span id='caret' style='position: relative; display: inline;'>|</span>";
        html += "<span style='position: relative; display: inline;'>" + format(end_range) + "</span>";

        let div_mirror = document.createElement("div");
        div_mirror.style.position = "absolute";
        div_mirror.style.left = "0";
        div_mirror.style.top = "0";
        div_mirror.style.zIndex = "-20000";
        div_mirror.innerHTML = html;
        document.body.append(div_mirror);

        let flag: HTMLSpanElement = div_mirror.querySelector("#caret");
        let flagPos = flag.getBoundingClientRect();
        let textAreaPos = textArea.getBoundingClientRect();
        let bodyPos = document.body.getBoundingClientRect();
        let left = textAreaPos.left + flagPos.left - textArea.scrollLeft - bodyPos.left;
        let top = textAreaPos.top + flagPos.top - textArea.scrollTop - bodyPos.top + 25;

        div_mirror.remove();
        return [left, top];
    };


}
