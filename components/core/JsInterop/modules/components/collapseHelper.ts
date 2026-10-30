type CollapseDotNet = {
  invokeMethodAsync(method: string, ...args: unknown[]): Promise<unknown>;
};

type CollapseRoot = HTMLElement & { __antCollapseDispose?: () => void };

const triggerSelector = '[data-collapse-trigger]';
const itemSelector = '.ant-collapse-item[data-collapse-key]';
const contentSelector = '.ant-collapse-panel';
const transitions = new WeakMap<HTMLElement, () => void>();

function getPanelItems(root: HTMLElement) {
  return Array.from(root.querySelectorAll<HTMLElement>(itemSelector))
    .filter(item => item.closest<HTMLElement>('[data-collapse-root]') === root);
}

function transitionTimeout(element: HTMLElement) {
  const parseTime = (value: string) => value.trim().endsWith('ms')
    ? Number.parseFloat(value)
    : Number.parseFloat(value) * 1000;
  const style = window.getComputedStyle(element);
  const durations = style.transitionDuration.split(',').map(parseTime);
  const delays = style.transitionDelay.split(',').map(parseTime);
  const count = Math.max(durations.length, delays.length);
  let longest = 0;
  for (let index = 0; index < count; index++) {
    longest = Math.max(longest, durations[index % durations.length] + delays[index % delays.length]);
  }
  return Math.max(50, longest + 50);
}

function setExpanded(item: HTMLElement, active: boolean) {
  item.classList.toggle('ant-collapse-item-active', active);
  item.querySelectorAll<HTMLElement>('[role="button"], [role="tab"]').forEach((trigger) => {
    trigger.setAttribute('aria-expanded', String(active));
  });

  const arrow = item.querySelector<SVGElement>('.ant-collapse-arrow svg');
  if (arrow) {
    const rtl = item.closest('.ant-collapse-rtl') !== null;
    arrow.style.transform = active ? `rotate(${rtl ? -90 : 90}deg)` : 'rotate(0deg)';
  }
}

function updateContent(item: HTMLElement, active: boolean, animate: boolean, finish: () => void) {
  const content = item.querySelector<HTMLElement>(contentSelector);
  if (!content) return;

  transitions.get(content)?.();

  const wasHidden = content.classList.contains('ant-collapse-panel-hidden');
  const currentHeight = content.getBoundingClientRect().height;
  const currentOpacity = window.getComputedStyle(content).opacity;
  content.classList.remove('ant-collapse-panel-hidden');
  if (active) {
    content.removeAttribute('hidden');
    content.removeAttribute('inert');
  }
  else content.setAttribute('inert', '');
  content.classList.toggle('ant-collapse-panel-active', active);
  content.classList.toggle('ant-collapse-panel-inactive', !active);

  const reduceMotion = window.matchMedia?.('(prefers-reduced-motion: reduce)').matches ?? false;
  if (!animate || reduceMotion) {
    content.style.removeProperty('height');
    content.style.removeProperty('opacity');
    if (!active) content.classList.add('ant-collapse-panel-hidden');
    else content.removeAttribute('hidden');
    finish();
    return;
  }

  content.style.height = `${active && wasHidden ? 0 : currentHeight}px`;
  content.style.opacity = active && wasHidden ? '0' : currentOpacity;
  // Force the collapsed/current size to be committed before setting the target.
  void content.offsetHeight;
  content.style.height = active ? `${content.scrollHeight}px` : '0px';
  content.style.opacity = active ? '1' : '0';

  let finished = false;
  let timeout = 0;
  const complete = (event?: TransitionEvent) => {
    if (finished || (event && (event.target !== content || event.propertyName !== 'height'))) return;
    finished = true;
    window.clearTimeout(timeout);
    content.removeEventListener('transitionend', complete);
    transitions.delete(content);
    content.style.removeProperty('height');
    content.style.removeProperty('opacity');
    if (!active) content.classList.add('ant-collapse-panel-hidden');
    finish();
  };
  content.addEventListener('transitionend', complete);
  timeout = window.setTimeout(() => complete(), transitionTimeout(content));
  transitions.set(content, () => {
    window.clearTimeout(timeout);
    content.removeEventListener('transitionend', complete);
    transitions.delete(content);
  });
}

export class collapseHelper {
  static attach(component: CollapseDotNet, root: CollapseRoot) {
    if (!root || root.__antCollapseDispose) return;
    let disposed = false;
    let syncQueue: Promise<unknown> = Promise.resolve();
    const invokeDotNet = (method: string, ...args: unknown[]) => {
      syncQueue = syncQueue
        .then(() => component.invokeMethodAsync(method, ...args))
        .catch(() => undefined);
      return syncQueue;
    };

    const waitForContent = async (item: HTMLElement) => {
      const deadline = performance.now() + 5000;
      while (!disposed && performance.now() < deadline) {
        const content = item.querySelector<HTMLElement>(contentSelector);
        if (content) return content;
        await new Promise<void>(resolve => requestAnimationFrame(() => resolve()));
      }
      return item.querySelector<HTMLElement>(contentSelector);
    };

    const toggleTrigger = (trigger: HTMLElement) => {
      if (trigger.getAttribute('aria-disabled') === 'true') return;
      const item = trigger.closest<HTMLElement>(itemSelector);
      const key = item?.dataset.collapseKey;
      if (!item || key == null) return;

      const active = !item.classList.contains('ant-collapse-item-active');
      const accordion = root.dataset.collapseAccordion === 'True' || root.dataset.collapseAccordion === 'true';
      let activeKeys: string[];
      try {
        activeKeys = JSON.parse(root.dataset.collapseActiveKeys || '[]');
      } catch {
        activeKeys = [];
      }
      if (accordion) {
        activeKeys = active ? [key] : [];
      } else if (active && !activeKeys.includes(key)) {
        activeKeys = [...activeKeys, key];
      } else if (!active) {
        activeKeys = activeKeys.filter(activeKey => activeKey !== key);
      }
      root.dataset.collapseActiveKeys = JSON.stringify(activeKeys);
      const touched = accordion && active
        ? getPanelItems(root)
        : [item];
      const panelsWaitingForContent: Array<{ item: HTMLElement; key: string; active: boolean }> = [];

      for (const panelItem of touched) {
        const panelKey = panelItem.dataset.collapseKey!;
        const nextActive = panelItem === item ? active : false;
        setExpanded(panelItem, nextActive);
        if (nextActive && !panelItem.querySelector(contentSelector)) {
          panelsWaitingForContent.push({ item: panelItem, key: panelKey, active: nextActive });
          continue;
        }
        updateContent(
          panelItem,
          nextActive,
          root.dataset.collapseAnimation === 'True' || root.dataset.collapseAnimation === 'true',
          () => {
            if (!disposed) invokeDotNet('SetPanelClientHidden', panelKey, !nextActive);
          },
        );
      }

      void (async () => {
        await invokeDotNet('ApplyClientActiveKeys', activeKeys, true);
        for (const pending of panelsWaitingForContent) {
          const content = await waitForContent(pending.item);
          if (!content || disposed) continue;
          updateContent(
            pending.item,
            pending.active,
            root.dataset.collapseAnimation === 'True' || root.dataset.collapseAnimation === 'true',
            () => {
              if (!disposed) invokeDotNet('SetPanelClientHidden', pending.key, !pending.active);
            },
          );
        }
      })();
    };

    const findTrigger = (event: Event) => {
      const target = event.target instanceof Element ? event.target : event.target instanceof Node ? event.target.parentElement : null;
      const trigger = target?.closest<HTMLElement>(triggerSelector);
      return trigger && root.contains(trigger) ? trigger : null;
    };

    const onClick = (event: MouseEvent) => {
      const trigger = findTrigger(event);
      if (!trigger) return;
      event.preventDefault();
      event.stopPropagation();
      toggleTrigger(trigger);
    };

    const onKeyDown = (event: KeyboardEvent) => {
      if ((event.key !== 'Enter' && event.key !== ' ') || event.repeat) return;
      const trigger = findTrigger(event);
      if (!trigger || trigger.getAttribute('aria-disabled') === 'true') return;
      event.preventDefault();
      event.stopPropagation();
      if (event.key === 'Enter') toggleTrigger(trigger);
    };

    const onKeyUp = (event: KeyboardEvent) => {
      if (event.key !== ' ') return;
      const trigger = findTrigger(event);
      if (!trigger || trigger.getAttribute('aria-disabled') === 'true') return;
      event.preventDefault();
      event.stopPropagation();
      toggleTrigger(trigger);
    };

    root.addEventListener('click', onClick);
    root.addEventListener('keydown', onKeyDown);
    root.addEventListener('keyup', onKeyUp);
    root.__antCollapseDispose = () => {
      disposed = true;
      root.removeEventListener('click', onClick);
      root.removeEventListener('keydown', onKeyDown);
      root.removeEventListener('keyup', onKeyUp);
      delete root.__antCollapseDispose;
    };
  }

  static detach(root: CollapseRoot) {
    root?.__antCollapseDispose?.();
  }
}
