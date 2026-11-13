export function beforeWebStart() {
    loadScriptAndStyle();
}

export function beforeStart(options, extensions) {
    loadScriptAndStyle();
}

function loadScriptAndStyle() {
    const interopJS = "_content/AntDesign.Charts/ant-design-charts-blazor.js";
    const cdnJS = "https://unpkg.com/@antv/g2plot@2.4.31/dist/g2plot.min.js";
    const localJS = "_content/AntDesign.Charts/g2plot.min.js";
    const cdnFlag = document.querySelector('[use-ant-design-charts-cdn]');

    if (!document.querySelector(`[src="${interopJS}"]`)) {
        const chartJS = cdnFlag ? cdnJS : localJS;
        const chartScript = document.createElement('script');
        chartScript.setAttribute('src', chartJS);

        const jsMark = document.querySelector("script");
        if (jsMark) {
            jsMark.before(chartScript);
        }
        else {
            document.body.appendChild(chartScript);
        }

        const interopScript = document.createElement('script');
        interopScript.setAttribute('src', interopJS);
        chartScript.after(interopScript);
    }
}