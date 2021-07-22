export class iconHelper {
    static createIconFromfontCN(scriptUrl) {
        if (document.querySelector(`[data-namespace="${scriptUrl}"]`)) {
            return;
        }
        const script = document.createElement('script');
        script.setAttribute('src', scriptUrl);
        script.setAttribute('data-namespace', scriptUrl);
        document.body.appendChild(script);
    }
}
