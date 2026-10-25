const states = new WeakMap();

// The transparent mask has pointer-events:none. Observe the original outside
// click in capture phase instead of intercepting it or replaying a synthetic
// click, so links, focus, defaults and framework handlers run exactly once.
export function setOpen(host, open, receiver) {
  if (!host?.isConnected) return;
  let state = states.get(host);
  if (!state) {
    state = { document: host.ownerDocument, open: false, receiver };
    state.click = event => {
      if (event.composedPath().includes(host)) return;
      setOpen(host, false, state.receiver);
      state.receiver.invokeMethodAsync('CloseFromOutside').catch(() => unmount(host));
    };
    states.set(host, state);
  }
  state.receiver = receiver;
  if (state.open === open) return;
  state.open = open;
  if (open) state.document.addEventListener('click', state.click, true);
  else state.document.removeEventListener('click', state.click, true);
}

export function unmount(host) {
  const state = states.get(host);
  if (!state) return;
  state.document.removeEventListener('click', state.click, true);
  states.delete(host);
}
