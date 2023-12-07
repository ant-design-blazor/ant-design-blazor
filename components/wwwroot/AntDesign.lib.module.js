var beforeStartCalled = false;
var afterStartedCalled = false;

export function beforeWebStart() {
    if (beforeStartCalled) {
        return;
    }

    beforeStartCalled = true;
    var customScript = document.createElement('script');
    customScript.setAttribute('src', '_content/AntDesign/js/ant-design-blazor.js');
    document.head.appendChild(customScript);

    var customStyle = document.createElement('link');
    customStyle.setAttribute('href', '_content/AntDesign/css/ant-design-blazor.css');
    document.head.appendChild(customStyle);
}

export function beforeStart(options, extensions) {
    if (beforeStartCalled) {
        return;
    }

    beforeStartCalled = true;

    var customScript = document.createElement('script');
    customScript.setAttribute('src', '_content/AntDesign/js/ant-design-blazor.js');
    document.head.appendChild(customScript);

    var customStyle = document.createElement('link');
    customStyle.setAttribute('href', '_content/AntDesign/css/ant-design-blazor.css');
    document.head.appendChild(customStyle);
}