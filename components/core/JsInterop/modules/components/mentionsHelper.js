var __awaiter = (this && this.__awaiter) || function (thisArg, _arguments, P, generator) {
    function adopt(value) { return value instanceof P ? value : new P(function (resolve) { resolve(value); }); }
    return new (P || (P = Promise))(function (resolve, reject) {
        function fulfilled(value) { try { step(generator.next(value)); } catch (e) { reject(e); } }
        function rejected(value) { try { step(generator["throw"](value)); } catch (e) { reject(e); } }
        function step(result) { result.done ? resolve(result.value) : adopt(result.value).then(fulfilled, rejected); }
        step((generator = generator.apply(thisArg, _arguments || [])).next());
    });
};
export class mentionsHelper {
}
mentionsHelper.setEditorKeyHandler = function (Mentions, textArea) {
    textArea.onkeydown = (ev) => __awaiter(this, void 0, void 0, function* () {
        let pop = document.querySelector("ul.ant-mentions-dropdown");
        //判断isPopShow不能用异步
        let isPopShow = pop != null && !pop.classList.contains("ant-mentions-dropdown-hidden");
        if (!isPopShow)
            return;
        if (ev.key == "ArrowUp") {
            ev.preventDefault();
            yield Mentions.invokeMethodAsync("UpOption");
        }
        else if (ev.key == "ArrowDown") {
            ev.preventDefault();
            yield Mentions.invokeMethodAsync("NextOption");
        }
        else if (ev.key == "Enter") {
            ev.preventDefault();
            yield Mentions.invokeMethodAsync("EnterOption");
        }
        //其他按键在c#中处理
    });
};
mentionsHelper.getProp = function (e, propName) {
    return e[propName];
};
mentionsHelper.getCursorXY = function (textArea) {
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
    let flag = div_mirror.querySelector("#caret");
    let flagPos = flag.getBoundingClientRect();
    let textAreaPos = textArea.getBoundingClientRect();
    let bodyPos = document.body.getBoundingClientRect();
    let left = textAreaPos.left + flagPos.left - textArea.scrollLeft - bodyPos.left;
    let top = textAreaPos.top + flagPos.top - textArea.scrollTop - bodyPos.top + 25;
    div_mirror.remove();
    return [left, top];
};
//# sourceMappingURL=mentionsHelper.js.map