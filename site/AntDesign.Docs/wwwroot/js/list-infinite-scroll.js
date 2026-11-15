const observers = new WeakMap();
const scrollHandlers = new WeakMap();
const virtualScrollHandlers = new WeakMap();

export function observeInfiniteScroll(container, sentinel, dotNetReference) {
    if (!container || !sentinel || !dotNetReference || !('IntersectionObserver' in window)) {
        return;
    }

    observers.get(sentinel)?.disconnect();

    const observer = new IntersectionObserver(entries => {
        if (entries.some(entry => entry.isIntersecting)) {
            dotNetReference.invokeMethodAsync('LoadNextPageAsync');
        }
    }, {
        root: container,
        rootMargin: '0px 0px 100px 0px',
    });

    observers.set(sentinel, observer);
    observer.observe(sentinel);
}

export function disconnectInfiniteScroll(sentinel) {
    observers.get(sentinel)?.disconnect();
    observers.delete(sentinel);
}

export function observeScrollToEnd(container, dotNetReference, containerHeight) {
    if (!container || !dotNetReference) {
        return;
    }

    disconnectScrollToEnd(container);

    const handler = () => {
        if (Math.abs(container.scrollHeight - container.scrollTop - containerHeight) <= 1) {
            dotNetReference.invokeMethodAsync('LoadMoreFromScroll');
        }
    };

    scrollHandlers.set(container, handler);
    container.addEventListener('scroll', handler, { passive: true });
}

export function observeVirtualList(container, dotNetReference) {
    if (!container || !dotNetReference) {
        return;
    }

    disconnectScrollToEnd(container);

    let userScrollPending = false;
    let scrollRequestPending = false;
    let latestScrollTop = container.scrollTop;
    let shouldLoadMore = false;
    const onUserScrollIntent = () => {
        userScrollPending = true;
    };
    const handler = () => {
        latestScrollTop = container.scrollTop;
        const isAtBottom = Math.abs(container.scrollHeight - container.scrollTop - container.clientHeight) <= 1;
        if (userScrollPending && isAtBottom) {
            userScrollPending = false;
            shouldLoadMore = true;
        }

        if (!scrollRequestPending) {
            scrollRequestPending = true;
            setTimeout(() => {
                scrollRequestPending = false;
                const loadMore = shouldLoadMore;
                shouldLoadMore = false;
                dotNetReference.invokeMethodAsync('OnVirtualScroll', latestScrollTop, loadMore);
            }, 50);
        }
    };

    virtualScrollHandlers.set(container, { handler, onUserScrollIntent });
    container.addEventListener('wheel', onUserScrollIntent, { passive: true });
    container.addEventListener('touchmove', onUserScrollIntent, { passive: true });
    container.addEventListener('keydown', onUserScrollIntent);
    container.addEventListener('scroll', handler, { passive: true });
}

export function disconnectScrollToEnd(container) {
    const handler = scrollHandlers.get(container);
    if (handler) {
        container.removeEventListener('scroll', handler);
        scrollHandlers.delete(container);
    }

    const virtualHandlers = virtualScrollHandlers.get(container);
    if (virtualHandlers) {
        container.removeEventListener('wheel', virtualHandlers.onUserScrollIntent);
        container.removeEventListener('touchmove', virtualHandlers.onUserScrollIntent);
        container.removeEventListener('keydown', virtualHandlers.onUserScrollIntent);
        container.removeEventListener('scroll', virtualHandlers.handler);
        virtualScrollHandlers.delete(container);
    }
}
