window.AntDesign.DocSearch = {
    instance: {},
    init: locale => {
        if (!docsearch) {
            return;
        }
        const lang = locale === 'zh-CN' ? 'cn' : 'en';
        window.AntDesign.DocSearch.instance = docsearch({
            apiKey: '872d29d519c3c2bd32a37398b6a85e0f',
            indexName: 'ant_design_blazor',
            appId: 'V29CUJY3QP',
            inputSelector: '#search-box input',
            algoliaOptions: { facetFilters: [`tags:${lang}`] },
            transformData(hits) {
                hits.forEach(hit => {
                    hit.url = hit.url.replace('ant-design-blazor.github.io', window.location.host);
                    hit.url = hit.url.replace('https:', window.location.protocol);
                });
                return hits;
            },
            debug: false
        });
    },
    localeChange: locale => {
        if (!docsearch || !window.AntDesign.DocSearch.instance) {
            return;
        }
        const lang = locale === 'zh-CN' ? 'cn' : 'en';
        window.AntDesign.DocSearch.instance.algoliaOptions.facetFilters = [`tags:${lang}`]
    }
};
