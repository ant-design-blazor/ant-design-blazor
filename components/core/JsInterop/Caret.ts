//extract from https://github.com/ichord/Caret.js

class InputCaret {
    domInputor: any;

    constructor(inputor) {
        this.domInputor = inputor;
    }

    getPos = function () {
        return this.domInputor.selectionStart;
    };

    getPosition = function (pos) {
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

    getOffset = function (pos = null) {
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

}

class Mirror {
    domInputor: any;
    css_attr: any;
    constructor(inputor) {
        this.domInputor = inputor;
        this.css_attr = [];
    }

    create = function (html) {
        this.$mirror = document.createElement("div");

        (<any>window).AntDesign.interop.css(this.$mirror, this.mirrorCss());

        this.$mirror.innerHTML = html;
        this.domInputor.parentElement.append(this.$mirror);

        return this;
    };

    mirrorCss = function () {
        var css, _this = this;
        css = {
            position: 'absolute',
            left: -9999,
            top: 0,
            zIndex: -20000
        };

        this.css_attr.push('width');

        this.css_attr.forEach((p) => {
            return css[p] = _this.domInputor.style[p]//_this.$inputor.css(p);
        })
        
        return css;
    };

    rect = function () {
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

        this.$mirror.parentElement.removeChild(this.$mirror)
        return rect;
    };

}

function getOffset(elem) {
    return (new InputCaret(elem)).getOffset();
}

export default getOffset;