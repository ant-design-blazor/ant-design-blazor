// React v6 PreviewBanner/LuminousBg: client-side decoration, no render-tree mutations
// or recurring Blazor Server traffic. Keep colors/geometry in HomeBannerBackground.
const instances = new WeakMap();

export function mount(root) {
    if (!root || !root.isConnected || instances.has(root)) return;
    const motion = window.matchMedia('(prefers-reduced-motion: reduce)');
    const timers = new Set();
    const bubbles = Array.from(root.querySelectorAll('[data-desc="luminous-bubble"]'));
    const stop = () => {
        timers.forEach(timer => window.clearTimeout(timer));
        timers.clear();
    };
    const randomize = bubble => {
        if (!root.isConnected || motion.matches) return;
        const x = (Math.random() - .5) * 400 * Number(bubble.dataset.offsetX || 1);
        const y = (Math.random() - .5) * 400 * Number(bubble.dataset.offsetY || 1);
        const opacity = Math.random();
        bubble.style.setProperty('--bubble-x', `${x}px`);
        bubble.style.setProperty('--bubble-y', `${y}px`);
        bubble.style.setProperty('--bubble-light-opacity', String(.1 + opacity * .05));
        bubble.style.setProperty('--bubble-dark-opacity', String(.1 + opacity * .2));
        bubble.style.setProperty('--bubble-scale', String(1 + Math.random()));
        const timer = window.setTimeout(() => {
            timers.delete(timer);
            randomize(bubble);
        }, Math.random() * 2000 + 3000);
        timers.add(timer);
    };
    const change = () => {
        stop();
        if (!motion.matches) bubbles.forEach(randomize);
    };
    // Also clean up when a disconnected server circuit can no longer call DisposeAsync.
    const observer = new MutationObserver(() => { if (!root.isConnected) unmount(root); });
    instances.set(root, () => {
        stop();
        motion.removeEventListener('change', change);
        observer.disconnect();
    });
    observer.observe(document.body, { childList: true, subtree: true });
    motion.addEventListener('change', change);
    change();
}

export function unmount(root) {
    instances.get(root)?.();
    instances.delete(root);
}
