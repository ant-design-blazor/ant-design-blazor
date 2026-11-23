export function setThemeStylesheet(href) {
    const link = document.querySelector('#docs-theme-v6');
    if (!link) return Promise.resolve(false);

    const targetUrl = new URL(href, document.baseURI).href;
    if (link.href === targetUrl && link.sheet) return Promise.resolve(true);

    return new Promise(resolve => {
        let settled = false;
        const finish = loaded => {
            if (settled) return;
            settled = true;
            clearTimeout(timeout);
            link.removeEventListener('load', onLoad);
            link.removeEventListener('error', onError);
            resolve(loaded);
        };
        const onLoad = () => finish(true);
        const onError = () => finish(false);
        const timeout = setTimeout(() => finish(false), 10000);

        link.addEventListener('load', onLoad, { once: true });
        link.addEventListener('error', onError, { once: true });
        link.href = href;
    });
}

// Keep the stylesheet switch callable through IJSRuntime even if the
// importing component is re-rendered or disposed while a theme is loading.
window.setDocsThemeStylesheet = setThemeStylesheet;
