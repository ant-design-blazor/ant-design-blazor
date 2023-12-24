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
    var customScript = document.createElement('script');
    customScript.setAttribute('src', '_content/AntDesign/js/ant-design-blazor.js');
    document.head.appendChild(customScript);

    var customStyle = document.createElement('link');
    customStyle.setAttribute('href', '_content/AntDesign/css/ant-design-blazor.css');
    customStyle.setAttribute('rel', 'stylesheet');
    document.head.appendChild(customStyle);
}