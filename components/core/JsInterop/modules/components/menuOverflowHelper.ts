// Horizontal Menu's responsive layout. The measurements come from the actual
// menu items (including icons, padding, custom content and the rest indicator).
const states = new WeakMap<HTMLElement, any>();

export function fitMenuItems(widths: number[], available: number, restWidth: number) {
    if (!widths.every(width => Number.isFinite(width) && width >= 0)
        || !Number.isFinite(available) || !Number.isFinite(restWidth)) return 0;
    if (widths.reduce((sum, width) => sum + width, 0) <= available + .5) return widths.length;
    let used = restWidth;
    let count = 0;
    for (const width of widths) {
        if (used + width > available + .5) break;
        used += width;
        count++;
    }
    return count;
}

export function observe(root: HTMLElement, reference: any, enabled = true) {
    if (!enabled) { return unobserve(root); }
    if (!root || !root.isConnected) return;
    if (states.has(root)) { states.get(root).schedule(); return; }
    const state: any = { disposed: false, frame: 0, next: null, pending: null, last: '', observed: new Set<HTMLElement>() };
    const publish = ids => {
        state.next = ids;
        if (state.pending) return;
        state.pending = (async () => {
            while (state.next && !state.disposed) {
                const next = state.next;
                state.next = null;
                const signature = JSON.stringify(next);
                if (signature === state.last) continue;
                await reference.invokeMethodAsync('SetOverflowItems', next);
                state.last = signature;
            }
        })().catch(error => { if (!state.disposed) console.error(error); })
            .finally(() => { state.pending = null; });
    };
    const width = (item: HTMLElement) => {
        const style = getComputedStyle(item);
        return style.display === 'none' ? 0 : item.getBoundingClientRect().width
            + (parseFloat(style.marginLeft) || 0) + (parseFloat(style.marginRight) || 0);
    };
    const measure = () => {
        state.frame = 0;
        if (state.disposed || !root.isConnected || root.clientWidth <= 0) return;
        const children = Array.from(root.children) as HTMLElement[];
        const items = children.filter(item => item.hasAttribute('data-menu-overflow-item') && getComputedStyle(item).display !== 'none');
        const rest = children.find(item => item.hasAttribute('data-menu-overflow-rest'));
        if (!rest) return;
        const targets = new Set([root, rest, ...items]);
        state.observed.forEach(item => { if (!targets.has(item)) resize?.unobserve(item); });
        targets.forEach(item => { if (!state.observed.has(item)) resize?.observe(item); });
        state.observed = targets;
        const style = getComputedStyle(root);
        const available = root.clientWidth - (parseFloat(style.paddingLeft) || 0) - (parseFloat(style.paddingRight) || 0);
        const gap = parseFloat(style.columnGap) || 0;
        const widths = items.map(item => width(item) + gap);
        const count = fitMenuItems(widths, available + gap, width(rest) + gap);
        publish(items.slice(count).map(item => item.getAttribute('data-menu-overflow-item')));
    };
    const schedule = () => {
        if (!state.disposed && !state.frame) state.frame = requestAnimationFrame(measure);
    };
    state.schedule = schedule;
    const resize = typeof ResizeObserver === 'function' ? new ResizeObserver(schedule) : null;
    resize?.observe(root);
    const mutations = new MutationObserver(schedule);
    mutations.observe(root, { childList: true, characterData: true, subtree: true, attributes: true, attributeFilter: ['class', 'style', 'data-menu-overflow-item'] });
    window.addEventListener('resize', schedule);
    const fonts = (document as any).fonts;
    fonts?.addEventListener('loadingdone', schedule);
    fonts?.ready.then(schedule);
    state.stop = () => {
        state.disposed = true;
        if (state.frame) cancelAnimationFrame(state.frame);
        resize?.disconnect();
        mutations.disconnect();
        window.removeEventListener('resize', schedule);
        fonts?.removeEventListener('loadingdone', schedule);
    };
    states.set(root, state);
    schedule();
}

export async function unobserve(root: HTMLElement) {
    const state = states.get(root);
    if (!state) return;
    state.stop();
    await state.pending;
    states.delete(root);
}

export class menuOverflowHelper {
    static observe = observe;
    static unobserve = unobserve;
}
