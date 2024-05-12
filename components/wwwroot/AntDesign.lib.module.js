var beforeStartCalled = false;
var afterStartedCalled = false;

export function beforeWebStart() {
    loadScriptAndStyle();
}

export function beforeStart(options, extensions) {
    loadScriptAndStyle();
}

function loadScriptAndStyle() {
    if (beforeStartCalled) {
        return;
    }

    beforeStartCalled = true;

    if (!document.querySelector('[src$="_content/AntDesign/js/ant-design-blazor.js"]') && !document.querySelector('[no-antblazor-js]')) {
        var customScript = document.createElement('script');
        customScript.setAttribute('src', '_content/AntDesign/js/ant-design-blazor.js');
        const jsmark = document.querySelector('[antblazor-js]') || document.querySelector('script');

        if (jsmark) {
            jsmark.before(customScript);
        } else {
            document.body.appendChild(customScript);
        }
    }

    if (!document.querySelector('[href*="_content/AntDesign/css/ant-design-blazor"]') && !document.querySelector('[no-antblazor-css]')) {
        var customStyle = document.createElement('link');
        customStyle.setAttribute('href', `_content/AntDesign/css/ant-design-blazor.css`);
        customStyle.setAttribute('rel', 'stylesheet');

        const cssMark = document.querySelector('[antblazor-css]') || document.querySelector('link');
        if (cssMark) {
            cssMark.before(customStyle);
        } else {
            document.head.appendChild(customStyle);
        }
    }
}