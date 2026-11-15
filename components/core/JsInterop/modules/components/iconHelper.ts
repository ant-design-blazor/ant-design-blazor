export class iconHelper {
  private static resources = new Map<string, Promise<void>>();
  private static sprites = new Map<string, { element: Element; sprite: SVGSVGElement }>();
  private static pending: Array<{ element: Element; urls: string[] }> = [];
  private static flushTimer: number | null = null;
  private static flushPromise: Promise<void> | null = null;

  static createIconFromfontCN(scriptUrl: string | string[], element?: Element): void {
    // iconfont inserts its symbols at the start of the document. Match React's
    // reverse, sequential loading without mutating the caller's resource array.
    const urls = (Array.isArray(scriptUrl) ? scriptUrl.slice() : [scriptUrl])
      .filter(url => typeof url === 'string' && url.length > 0).reverse();
    if (!urls.length) return;

    // Server-side after-render callbacks can arrive in a different order from
    // sibling DOM nodes. Batch requests from the current render and process
    // them in document order so duplicate symbol IDs resolve like React's
    // page-level iconfont scripts.
    this.pending.push({ element, urls });

    // A running batch drains requests queued while a remote script is loading.
    // Starting another flush concurrently can make two load handlers claim the
    // same newly inserted SVG sprite and leave a later, untracked sprite ahead
    // of the document-ordered entries.
    if (this.flushPromise) return;

    if (this.flushTimer !== null) window.clearTimeout(this.flushTimer);
    // A short quiet window collects sibling callbacks without adding runtime
    // coordination attributes to the rendered icon DOM.
    this.flushTimer = window.setTimeout(() => { void this.flushPending(); }, 500);
  }

  private static async flushPending() {
    this.flushTimer = null;
    if (this.flushPromise) return this.flushPromise;

    const flushPromise = (async () => {
      while (this.pending.length) {
        const batch = this.pending.splice(0);
        batch.sort((left, right) => {
          if (!left.element || !right.element || left.element === right.element) return 0;
          const relation = left.element.compareDocumentPosition(right.element);
          if (relation & Node.DOCUMENT_POSITION_FOLLOWING) return -1;
          if (relation & Node.DOCUMENT_POSITION_PRECEDING) return 1;
          return 0;
        });

        for (const request of batch) {
          for (const url of request.urls) {
            await this.loadResource(url, request.element);
          }
        }
      }
    })();

    this.flushPromise = flushPromise;
    try {
      await flushPromise;
    } finally {
      if (this.flushPromise === flushPromise) this.flushPromise = null;
    }
  }

  private static loadResource(url: string, element: Element): Promise<void> {
    const cached = this.resources.get(url);
    if (cached) return cached;
    if (Array.from(document.querySelectorAll('script[data-namespace]'))
      .some(script => script.getAttribute('data-namespace') === url)) return Promise.resolve();

    const existingSprites = new Set(Array.from(document.body.querySelectorAll<SVGSVGElement>('svg'))
      .filter(sprite => sprite.querySelector('symbol[id]')));
    const loaded = new Promise<void>(resolve => {
      const script = document.createElement('script');
      script.setAttribute('src', url);
      script.setAttribute('data-namespace', url);
      const complete = () => {
        const sprite = Array.from(document.body.querySelectorAll<SVGSVGElement>('svg'))
          .find(candidate => !existingSprites.has(candidate) && candidate.querySelector('symbol[id]'));
        if (sprite) {
          this.sprites.set(url, { element, sprite });
          this.reorderSprites();
        }
        resolve();
      };
      script.addEventListener('load', complete, { once: true });
      // As in React, an unavailable resource must not prevent later resources.
      script.addEventListener('error', complete, { once: true });
      document.body.appendChild(script);
    });
    this.resources.set(url, loaded);
    return loaded;
  }

  private static reorderSprites() {
    const sprites = Array.from(this.sprites.values())
      .filter(entry => entry.sprite.isConnected)
      .sort((left, right) => {
        if (left.element !== right.element && left.element.isConnected && right.element.isConnected) {
          const relation = left.element.compareDocumentPosition(right.element);
          // React leaves earlier demos' sprites first when IDs are duplicated.
          if (relation & Node.DOCUMENT_POSITION_FOLLOWING) return -1;
          if (relation & Node.DOCUMENT_POSITION_PRECEDING) return 1;
        }
        const relation = left.sprite.compareDocumentPosition(right.sprite);
        if (relation & Node.DOCUMENT_POSITION_FOLLOWING) return -1;
        if (relation & Node.DOCUMENT_POSITION_PRECEDING) return 1;
        return 0;
      });
    const fragment = document.createDocumentFragment();
    sprites.forEach(entry => fragment.appendChild(entry.sprite));
    document.body.insertBefore(fragment, document.body.firstChild);
  }
}
