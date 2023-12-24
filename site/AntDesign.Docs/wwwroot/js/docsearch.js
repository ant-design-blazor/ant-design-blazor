window.DocSearch = {
  init: function (locale) {
    if (!docsearch) {
      return;
    }
    const lang = locale === 'zh-CN' ? 'cn' : 'en';
    docsearch({
      apiKey: '93dc26e7c963c887f9b096b349dd47ce',
      appId: 'C9UTAZSOWW',
      indexName: 'ant-design-blazor-docs',
      container: '#docsearch',
      placeholder: lang == 'cn' ? "全文搜索" : "Search...",
      searchParameters: { facetFilters: ['tags:' + lang] },
      transformItems: function (hits) {
        hits.forEach(function (hit) {
          hit.url = hit.url.replace('antblazor.com', window.location.host);
          hit.url = hit.url.replace('https:', window.location.protocol);
        });
        return hits;
      },
      debug: false,
      transformSearchClient: function (searchClient) {
        return {
          ...searchClient,
          search: debounce(searchClient.search, 2000)
        };
      },
      getMissingResultsUrl: function ({ query }) {
        return `https://github.com/ant-design-blazor/ant-design-blazor/issues/new?title=${query}`;
      },
    })
  }
};

function debounce(func, wait = 100) {
  let lastTimeout = null;
  return function (...args) {
    const that = this;
    return new Promise((resolve, reject) => {
      if (lastTimeout) {
        clearTimeout(lastTimeout);
      }
      lastTimeout = setTimeout(() => {
        lastTimeout = null;
        Promise.resolve(func.apply(that, args)).then(resolve).catch(reject);
      }, wait);
    });
  };
}
