const observers = new WeakMap();

export function observe(element, receiver) {
  if (!element?.isConnected) return;
  unobserve(element);

  if (!('IntersectionObserver' in window)) {
    receiver.invokeMethodAsync('RenderDemoWhenVisible').catch(() => {});
    return;
  }

  const observer = new IntersectionObserver(entries => {
    if (!entries.some(entry => entry.isIntersecting)) return;
    observer.disconnect();
    observers.delete(element);
    receiver.invokeMethodAsync('RenderDemoWhenVisible').catch(() => {});
  }, { rootMargin: '300px 0px' });

  observers.set(element, observer);
  observer.observe(element);
}

export function unobserve(element) {
  const observer = element && observers.get(element);
  if (!observer) return;
  observer.disconnect();
  observers.delete(element);
}
