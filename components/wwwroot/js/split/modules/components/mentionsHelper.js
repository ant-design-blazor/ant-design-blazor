import { state } from "../stateProvider";
export class mentionsHelper {
    static getCursorXY(element, objReference) {
        state.objReferenceDict["mentions"] = objReference;
        window.addEventListener("click", this.mentionsOnWindowClick);
        const offset = this.getOffset(element);
        return [offset.left, offset.top + offset.height + 14];
    }
    static getOffset(elem) {
        return (new InputCaret(elem)).getOffset();
    }
    static mentionsOnWindowClick(e) {
        let mentionsObj = state.objReferenceDict["mentions"];
        if (mentionsObj) {
            mentionsObj.invokeMethodAsync("CloseMentionsDropDown");
        }
        else {
            window.removeEventListener("click", this.mentionsOnWindowClick);
        }
    }
}
class InputCaret {
    constructor(inputor) {
        this.getPos = function () {
            return this.domInputor.selectionStart;
        };
        this.getPosition = function (pos) {
            var domInputor, end_range, format, html, mirror, start_range;
            domInputor = this.domInputor;
            format = function (value) {
                value = value.replace(/<|>|`|"|&/g, '?').replace(/\r\n|\r|\n/g, "<br/>");
                if (/firefox/i.test(navigator.userAgent)) {
                    value = value.replace(/\s/g, '&nbsp;');
                }
                return value;
            };
            if (!pos) {
                pos = this.getPos();
            }
            let inputorValue = domInputor.value;
            start_range = inputorValue.slice(0, pos);
            end_range = inputorValue.slice(pos);
            html = "<span style='position: relative; display: inline;'>" + format(start_range) + "</span>";
            html += "<span id='caret' style='position: relative; display: inline;'>|</span>";
            html += "<span style='position: relative; display: inline;'>" + format(end_range) + "</span>";
            mirror = new Mirror(domInputor);
            return mirror.create(html).rect();
        };
        this.getOffset = function (pos = null) {
            var offset, position, domInputor;
            domInputor = this.domInputor;
            var rect = domInputor.getBoundingClientRect();
            offset = {
                left: rect.left,
                top: rect.top
            };
            position = this.getPosition(pos);
            return offset = {
                left: offset.left + position.left - domInputor.scrollLeft,
                top: offset.top + position.top - domInputor.scrollTop,
                height: position.height
            };
        };
        this.domInputor = inputor;
    }
}
class Mirror {
    constructor(inputor) {
        this.create = function (html) {
            this.$mirror = document.createElement("div");
            //TODO: hard coded reference, try to make it more relative
            window.AntDesign.interop.styleHelper.css(this.$mirror, this.mirrorCss());
            this.$mirror.innerHTML = html;
            this.domInputor.parentElement.append(this.$mirror);
            return this;
        };
        this.mirrorCss = function () {
            var css, _this = this;
            css = {
                position: 'absolute',
                left: -9999,
                top: 0,
                zIndex: -20000
            };
            this.css_attr.push('width');
            this.css_attr.forEach((p) => {
                return css[p] = _this.domInputor.style[p]; //_this.$inputor.css(p);
            });
            return css;
        };
        this.rect = function () {
            var flag, pos, rect;
            flag = this.$mirror.querySelector("#caret");
            var oRect = flag.getBoundingClientRect();
            pos = {
                left: flag.offsetLeft,
                top: flag.offsetTop
            }; //$flag.position();
            rect = {
                left: pos.left,
                top: pos.top,
                height: oRect.height
            };
            this.$mirror.parentElement.removeChild(this.$mirror);
            return rect;
        };
        this.domInputor = inputor;
        this.css_attr = [];
    }
}
