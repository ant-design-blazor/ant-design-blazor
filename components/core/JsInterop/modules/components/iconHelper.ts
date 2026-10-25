export class iconHelper {
  private static resources = new Map<string, Promise<void>>();

  static async createIconFromfontCN(scriptUrl: string | string[]) {
    // iconfont inserts its symbols at the start of the document. Match React's
    // reverse, sequential loading without mutating the caller's resource array.
    const urls = (Array.isArray(scriptUrl) ? scriptUrl.slice() : [scriptUrl])
      .filter(url => typeof url === 'string' && url.length > 0).reverse();
    for (const url of urls) {
      await this.loadResource(url);
    }
  }

  private static loadResource(url: string): Promise<void> {
    const cached = this.resources.get(url);
    if (cached) return cached;
    if (Array.from(document.querySelectorAll('script[data-namespace]'))
      .some(script => script.getAttribute('data-namespace') === url)) return Promise.resolve();

    const loaded = new Promise<void>(resolve => {
      const script = document.createElement('script');
      script.setAttribute('src', url);
      script.setAttribute('data-namespace', url);
      script.addEventListener('load', () => resolve(), { once: true });
      // As in React, an unavailable resource must not prevent later resources.
      script.addEventListener('error', () => resolve(), { once: true });
      document.body.appendChild(script);
    });
    this.resources.set(url, loaded);
    return loaded;
  }
}
