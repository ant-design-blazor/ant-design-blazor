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

    // Versioned application references must count as already loaded too.
    // Otherwise the initializer inserts an unversioned (possibly cached) copy
    // which overwrites the newer window.AntDesign interop object.
    const hasMainScript = Array.from(document.querySelectorAll('script[src]')).some(script => {
        try {
            return new URL(script.getAttribute('src'), document.baseURI).pathname.endsWith('/_content/AntDesign/js/ant-design-blazor.js');
        } catch { return false; }
    });
    if (!hasMainScript && !document.querySelector('[no-antblazor-js]')) {
        var customScript = document.createElement('script');
        customScript.setAttribute('src', '_content/AntDesign/js/ant-design-blazor.js');
        const jsmark = document.querySelector('[antblazor-js]') || document.querySelector('script');

        if (jsmark) {
            jsmark.before(customScript);
        } else {
            document.body.appendChild(customScript);
        }
    }

    if (!document.querySelector('[href*="_content/AntDesign/css/ant-design-blazor"], [href*="_content/AntDesign/css/antd-v6/"]') && !document.querySelector('[no-antblazor-css]')) {
        var customStyle = document.createElement('link');
        customStyle.setAttribute('href', `_content/AntDesign/css/antd-v6/light.css`);
        customStyle.setAttribute('rel', 'stylesheet');

        const cssMark = document.querySelector('[antblazor-css]') || document.querySelector('link');
        if (cssMark) {
            cssMark.before(customStyle);
        } else {
            document.head.appendChild(customStyle);
        }
    }

    if (!document.querySelector('[href$="_content/AntDesign/css/avatar-responsive.css"]')) {
        var avatarResponsiveStyle = document.createElement('link');
        avatarResponsiveStyle.setAttribute('href', '_content/AntDesign/css/avatar-responsive.css');
        avatarResponsiveStyle.setAttribute('rel', 'stylesheet');
        document.head.appendChild(avatarResponsiveStyle);
    }
}
