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

    if (!document.querySelector('[src="_content/AntDesign/js/ant-design-blazor.js"]')) {
        var customScript = document.createElement('script');
        customScript.setAttribute('src', '_content/AntDesign/js/ant-design-blazor.js');
        document.body.insertBefore(customScript, document.querySelector('[antblazor-js]') || document.querySelector('script'));
    }

    if (!document.querySelector('[href="_content/AntDesign/css/ant-design-blazor.css"]')){
        var customStyle = document.createElement('link');
        customStyle.setAttribute('href', `_content/AntDesign/css/ant-design-blazor.css`);
        customStyle.setAttribute('rel', 'stylesheet');
        document.head.insertBefore(customStyle, document.querySelector('[antblazor-css]') || document.querySelector('link'));
    }
}