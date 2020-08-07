window.AntDesign.DocSearch = {
    instance: {},
    init: locale => {
        if (!docsearch) {
            return;
        }
        const lang = locale === 'zh-CN' ? 'cn' : 'en';
        window.AntDesign.DocSearch.instance = docsearch({
            apiKey: '93dc26e7c963c887f9b096b349dd47ce',
            indexName: 'ant-design-blazor-docs',
            appId: 'C9UTAZSOWW',
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