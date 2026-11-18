#!/usr/bin/env node

import { createRequire } from 'node:module';
import { readFile, writeFile, mkdir } from 'node:fs/promises';
import { dirname, resolve } from 'node:path';
import process from 'node:process';
import { compareDomSnapshots } from './dom-style-parity.mjs';
import { captureDomStyles } from './dom-style-capture.mjs';

const DEFAULT_STYLE_PROPERTIES = [
  'display', 'position', 'boxSizing', 'width', 'height', 'minHeight', 'maxHeight',
  'paddingBlock', 'paddingInline', 'marginBlock', 'gap', 'alignItems', 'lineHeight',
  'fontSize', 'fontWeight', 'color', 'backgroundColor', 'borderRadius',
  'borderTopWidth', 'borderRightWidth', 'borderBottomWidth', 'borderLeftWidth',
  '--ant-select-height', '--ant-select-multi-item-height', '--ant-select-multi-padding-vertical',
];
const DEFAULT_PSEUDO_STYLE_PROPERTIES = [
  'content', 'display', 'position', 'width', 'height', 'inset', 'transform',
  'backgroundColor', 'borderTopWidth', 'borderRightWidth', 'borderBottomWidth', 'borderLeftWidth',
];

function parseArgs(argv) {
  const values = {};
  for (let index = 0; index < argv.length; index += 1) {
    if (!argv[index].startsWith('--')) continue;
    values[argv[index].slice(2)] = argv[index + 1];
    index += 1;
  }
  return values;
}

function usage() {
  console.error('Usage: node dom-style-parity-runner.mjs --cases <manifest.json> --antd-path <react-repo> [--chrome <executable>] [--report <report.json>]');
}

function puppeteerFor(reactRoot) {
  const requireFromReact = createRequire(resolve(reactRoot, 'package.json'));
  return requireFromReact('puppeteer');
}

function safeError(error) {
  return String(error?.message ?? error).split('\n')[0];
}

async function navigate(page, url, side, timeoutMs) {
  const errors = [];
  page.on('pageerror', (error) => errors.push(`pageerror: ${safeError(error)}`));
  page.on('console', (message) => {
    if (message.type() === 'error') errors.push(`console: ${message.text().slice(0, 240)}`);
  });
  try {
    await page.goto(url, { waitUntil: 'domcontentloaded', timeout: timeoutMs });
    return { side, url, errors, loaded: true };
  } catch (error) {
    return { side, url, errors, loaded: false, blocker: safeError(error) };
  }
}

export async function scrollScopeAnchorIntoView(page, scopeSelector, rootSelector, timeoutMs = 20000, explicitAnchorSelector = null) {
  // Dumi mounts demos lazily as their preview containers approach the
  // viewport. For portal comparisons the capture scope is often `body`, so
  // use the component root's outer demo anchor to trigger that lazy mount.
  const hasDocumentScope = /^(?:body|html|:root)$/.test(scopeSelector);
  const anchorSource = explicitAnchorSelector ?? (hasDocumentScope ? rootSelector : scopeSelector);
  if (anchorSource) {
    const scopeAnchorSelector = anchorSource.split(/\s+/)[0];
    // Demo sections are hydrated lazily by the docs site. A fresh navigation
    // may not have created the section when waitForDemo first tries to scroll;
    // wait for its stable outer anchor before scrolling to trigger the demo.
    await page.waitForSelector(scopeAnchorSelector, { timeout: timeoutMs });
    await page.evaluate(selector => {
      // The docs page enables smooth scrolling globally. Disable it for the
      // automated page only so lazy demo mounting and subsequent geometry
      // alignment do not race an in-flight scroll animation.
      document.documentElement.style.scrollBehavior = 'auto';
      document.body.style.scrollBehavior = 'auto';
      document.querySelector(selector)?.scrollIntoView({ block: 'center', inline: 'nearest', behavior: 'instant' });
    }, scopeAnchorSelector);
  }
}

async function alignRootViewportY(page, rootSelector, targetY, timeoutMs) {
  await page.waitForSelector(rootSelector, { timeout: timeoutMs });
  await page.evaluate(() => {
    document.documentElement.style.scrollBehavior = 'auto';
    document.body.style.scrollBehavior = 'auto';
  });

  const waitForStableLayout = async () => page.waitForFunction(({ rootSelector }) => {
    const root = document.querySelector(rootSelector);
    if (!root) return false;
    const rect = root.getBoundingClientRect();
    const signature = `${rect.top}:${rect.width}:${rect.height}:${document.documentElement.scrollHeight}`;
    const state = window.__antdV6ParityLayoutStability ??= { signature: '', stablePolls: 0 };
    if (state.signature === signature) state.stablePolls += 1;
    else {
      state.signature = signature;
      state.stablePolls = 0;
    }
    return state.stablePolls >= 3;
  }, { timeout: timeoutMs, polling: 100 }, { rootSelector });

  for (let attempt = 0; attempt < 6; attempt += 1) {
    await waitForStableLayout();
    await page.$eval(rootSelector, (element, y) => {
      let scrollContainer = element.parentElement;
      while (scrollContainer && scrollContainer !== document.body) {
        const style = getComputedStyle(scrollContainer);
        const canScroll = /(auto|scroll|overlay)/.test(style.overflowY)
          && scrollContainer.scrollHeight > scrollContainer.clientHeight;
        if (canScroll) break;
        scrollContainer = scrollContainer.parentElement;
      }

      const top = element.getBoundingClientRect().top;
      if (scrollContainer && scrollContainer !== document.body) {
        scrollContainer.style.scrollBehavior = 'auto';
        scrollContainer.scrollTop += top - y;
      } else {
        window.scrollTo({ top: window.scrollY + top - y, left: window.scrollX, behavior: 'instant' });
      }
    }, targetY);
    await page.evaluate(() => new Promise(resolvePromise => requestAnimationFrame(() => requestAnimationFrame(resolvePromise))));
    try {
      await waitForStableLayout();
    } catch {
      continue;
    }
    const aligned = await page.$eval(rootSelector, (element, y) => Math.abs(element.getBoundingClientRect().top - y) < 0.5, targetY);
    if (aligned) return;
  }

  const finalTop = await page.$eval(rootSelector, element => element.getBoundingClientRect().top);
  throw new Error(`Could not align ${rootSelector} to viewport y=${targetY}; final top=${finalTop}`);
}

async function waitForDemo(page, sideSpec, timeoutMs, minimumRoots, { scroll = true } = {}) {
  const scopeSelector = sideSpec.scopeSelector;
  const rootSelector = sideSpec.rootSelector;
  // Responsive components often finish layout-dependent rendering from
  // requestAnimationFrame/ResizeObserver. Chrome can throttle those callbacks
  // while a Puppeteer page is in the background, so activate each side before
  // deciding that its DOM is ready to compare.
  await page.bringToFront();
  if (scroll) await scrollScopeAnchorIntoView(page, scopeSelector, rootSelector, timeoutMs, sideSpec.scrollAnchorSelector);
  await page.waitForSelector(scopeSelector, { timeout: timeoutMs });
  try {
    await page.waitForFunction(({ scopeSelector, rootSelector, skeletonSelector, minimumRoots }) => {
      const scope = document.querySelector(scopeSelector);
      // Earlier lazy demos can change the document height after the initial
      // scroll, pushing this demo back out of the viewport before its
      // IntersectionObserver callback runs. Keep the target visible while we
      // wait so the docs page can finish mounting the requested demo.
      if (scope && !/^(?:body|html|:root)$/.test(scopeSelector)) {
        const rect = scope.getBoundingClientRect();
        if (rect.bottom <= 0 || rect.top >= window.innerHeight) {
          scope.scrollIntoView({ block: 'center', inline: 'nearest', behavior: 'instant' });
        }
      }
      return Boolean(scope)
        && (!skeletonSelector || scope.querySelectorAll(skeletonSelector).length === 0)
        && scope.querySelectorAll(rootSelector).length >= minimumRoots;
    }, { timeout: timeoutMs, polling: 100 }, {
      scopeSelector,
      rootSelector,
      skeletonSelector: sideSpec.skeletonSelector ?? '.ant-skeleton',
      minimumRoots,
    });
  } catch (error) {
    const state = await page.evaluate(({ scopeSelector, rootSelector, skeletonSelector }) => {
      const scope = document.querySelector(scopeSelector);
      return {
        title: document.title,
        skeletons: scope && skeletonSelector ? scope.querySelectorAll(skeletonSelector).length : null,
        roots: scope ? scope.querySelectorAll(rootSelector).length : null,
        blazorStarted: Boolean(window.Blazor),
      };
    }, { scopeSelector, rootSelector, skeletonSelector: sideSpec.skeletonSelector ?? '.ant-skeleton' });
    throw new Error(`${safeError(error)} (scope=${scopeSelector}, roots=${state.roots}, skeletons=${state.skeletons}, title=${JSON.stringify(state.title)}, blazor=${state.blazorStarted})`);
  }
  await page.evaluate(async () => {
    if (document.fonts?.ready) await document.fonts.ready;
    await new Promise(requestAnimationFrame);
    await new Promise(requestAnimationFrame);
  });
  // A body-wide scope is common for portals, but scrolling body does not align
  // the component demo. Center the actual trigger so page layout/scroll offsets
  // do not masquerade as component popup-positioning differences.
  if (scroll) {
    await page.evaluate(({ rootSelector, scopeSelector }) => {
      document.documentElement.style.scrollBehavior = 'auto';
      document.body.style.scrollBehavior = 'auto';
      (document.querySelector(rootSelector) ?? document.querySelector(scopeSelector))
        ?.scrollIntoView({ block: 'center', inline: 'nearest', behavior: 'instant' });
    }, { rootSelector, scopeSelector });
  }
}

export async function waitForSvgUses(page, side, sideSpec, semanticNodes, timeoutMs) {
  const definitions = (semanticNodes ?? []).filter(node => node.verifySvgUse);
  if (!definitions.length) return;
  await page.waitForFunction(({ side, scopeSelector, rootSelector, definitions }) => {
    const scope = document.querySelector(scopeSelector);
    if (!scope) return false;
    const roots = [...scope.querySelectorAll(rootSelector)];
    const ready = definitions.every(definition => {
      const selector = definition[`${side}Selector`] ?? definition.selector;
      const nodes = definition.portal ? [...document.querySelectorAll(selector)]
        : roots.flatMap(root => selector === '$root' ? [root] : [...root.querySelectorAll(selector)]);
      return nodes.length >= Math.max(1, definition.minimumCount ?? 0) && nodes.every(element => {
        if (!element.matches('use')) return false;
        const href = element.getAttribute('xlink:href') ?? element.getAttribute('href');
        const rect = element.getBoundingClientRect();
        return rect.width > 0 && rect.height > 0 && [...document.querySelectorAll('symbol[id]')]
          .some(symbol => `#${symbol.getAttribute('id')}` === href);
      });
    });
    if (!ready) return false;

    // Iconfont sprites may append duplicate symbol definitions after the
    // first usable <use> appears. Wait for the referenced SVG symbol set to
    // stop changing before capture so symbol-order and glyph bounds reflect
    // the settled browser state rather than script-load timing.
    const signature = JSON.stringify(definitions.flatMap(definition => {
      const selector = definition[`${side}Selector`] ?? definition.selector;
      const nodes = definition.portal ? [...document.querySelectorAll(selector)]
        : roots.flatMap(root => selector === '$root' ? [root] : [...root.querySelectorAll(selector)]);
      return nodes.map(element => {
        const href = element.getAttribute('xlink:href') ?? element.getAttribute('href');
        return [href, [...document.querySelectorAll('symbol[id]')]
          .filter(symbol => `#${symbol.getAttribute('id')}` === href)
          .map(symbol => symbol.outerHTML)];
      });
    }));
    const stateKey = '__antdV6StyleGenSvgSymbolStability';
    const state = window[stateKey] ?? (window[stateKey] = { signature: '', stablePolls: 0 });
    state.stablePolls = state.signature === signature ? state.stablePolls + 1 : 0;
    state.signature = signature;
    return state.stablePolls >= 3;
  }, { timeout: timeoutMs, polling: 100 }, {
    side, scopeSelector: sideSpec.scopeSelector, rootSelector: sideSpec.rootSelector, definitions,
  });
}

async function clickAtCenter(page, selector, timeoutMs, preserveViewport = false) {
  await page.waitForSelector(selector, { timeout: timeoutMs });
  const point = await page.$eval(selector, (element, shouldScroll) => {
    if (shouldScroll) element.scrollIntoView({ block: 'center', inline: 'nearest', behavior: 'instant' });
    const rect = element.getBoundingClientRect();
    return { x: rect.left + rect.width / 2, y: rect.top + rect.height / 2 };
  }, !preserveViewport);
  // Send a real pointer sequence at the freshly measured center. Locator.click
  // can focus a readonly Select input without dispatching the pointerdown path
  // used by OverlayTrigger's immediate-open handler in interactive server docs.
  await page.mouse.click(point.x, point.y);
}

export async function applyAction(page, side, action, timeoutMs) {
  // CSS transitions and ResizeObserver callbacks are throttled in background
  // tabs. Activate the page before dispatching input so the resulting DOM and
  // computed styles are sampled from the same settled state.
  await page.bringToFront();
  const selector = action[`${side}Selector`] ?? action.selector;
  const getVisibleTarget = async () => {
    const candidates = await page.$$(selector);
    for (const candidate of candidates) {
      const visible = await candidate.evaluate((element) => {
        const style = getComputedStyle(element);
        const rect = element.getBoundingClientRect();
        return style.display !== 'none' && style.visibility !== 'hidden'
          && Number(style.opacity) > 0 && rect.width > 0 && rect.height > 0
          && rect.bottom > 0 && rect.right > 0 && rect.top < innerHeight && rect.left < innerWidth;
      });
      if (visible) return candidate;
    }
    throw new Error(`${side} no visible match for ${selector}`);
  };
  switch (action.type) {
    case 'click':
      if (action.force) {
        // Native radio inputs are visually hidden; activate the enclosing label so
        // both the browser's checked state and framework change handler run.
        await page.$eval(selector, (element) => (element.closest('label') ?? element).click());
      } else if (action.visible) {
        // Component documentation keeps previously opened portal nodes mounted
        // while hidden or offscreen. Resolve the first actually visible match
        // before clicking instead of timing out on an earlier stale portal.
        const target = await getVisibleTarget();
        await target.evaluate((element) => element.scrollIntoView({ block: 'center', inline: 'nearest', behavior: 'instant' }));
        await target.click({ timeout: timeoutMs });
      } else {
        // Long component docs often place the target thousands of pixels below
        // the viewport and use smooth scrolling. Puppeteer's locator can wait
        // indefinitely for that scroll to settle before clicking; move it into
        // view instantly so the action tests the component, not page scrolling.
        if (action.coordinateClick) {
          await clickAtCenter(page, selector, timeoutMs, action.preserveViewport === true);
        } else {
          await page.$eval(selector, (element) => element.scrollIntoView({ block: 'center', inline: 'nearest', behavior: 'instant' }));
          await page.locator(selector).click({ timeout: timeoutMs });
        }
      }
      if (action[`${side}CheckedValue`] !== undefined) {
        const expectedValue = action[`${side}CheckedValue`];
        const checkedSelector = action[`${side}CheckedSelector`] ?? action.checkedSelector ?? selector;
        await page.waitForFunction(({ selector, expectedValue }) => {
          const element = document.querySelector(selector);
          return element instanceof HTMLInputElement && element.checked && element.value === expectedValue;
        }, { timeout: timeoutMs }, { selector: checkedSelector, expectedValue });
      }
      break;
    case 'measureClickVisible': {
      const visibleSelector = action[`${side}VisibleSelector`] ?? action.visibleSelector;
      if (!visibleSelector) throw new Error(`${side}VisibleSelector is required for measureClickVisible`);
      await page.evaluate((targetSelector, popupSelector, timeout, portalMatch) => {
        window.__antDesignParityVisibilityPromise = new Promise((resolve) => {
          let startedAt;
          let rafId;
          let timeoutId;
          let observer;
          const finish = (elapsed) => {
            document.removeEventListener('pointerdown', onPointerDown, true);
            observer?.disconnect();
            cancelAnimationFrame(rafId);
            clearTimeout(timeoutId);
            resolve(elapsed);
          };
          const isPaintVisible = () => {
            let candidates = [...document.querySelectorAll(popupSelector)];
            if (portalMatch === 'aria-controls') {
              const trigger = document.querySelector(targetSelector);
              // The click target can itself be the combobox input before
              // Blazor has rendered aria-controls/aria-owns. Keep that input
              // as the association anchor so immediate-open popup timing is
              // measured during the server round trip instead of returning a
              // false null for an unassociated portal.
              const input = trigger?.matches('[role="combobox"], [aria-controls], [aria-owns]')
                ? trigger
                : trigger?.querySelector('[aria-controls], [aria-owns]');
              const popupId = input?.getAttribute('aria-controls') || input?.getAttribute('aria-owns');
              const controlledNode = popupId && document.getElementById(popupId);
              const controlledPopup = controlledNode?.closest(popupSelector);
              if (controlledPopup) {
                candidates = [controlledPopup];
              } else if (input?.getAttribute('aria-expanded') === 'true') {
                // Some Blazor Server overlays are positioned and revealed on
                // pointerdown before the server round trip writes
                // aria-controls. Associate the popup geometrically in that
                // interval, but still require the popup itself to be visible;
                // the open class can be applied before its first painted frame.
                const triggerRoot = input.closest('.ant-select') ?? input;
                const triggerRect = triggerRoot.getBoundingClientRect();
                const isVisible = (element) => {
                  const style = getComputedStyle(element);
                  const rect = element.getBoundingClientRect();
                  return style.display !== 'none' && style.visibility !== 'hidden'
                    && Number(style.opacity) > 0 && rect.width > 0 && rect.height > 0;
                };
                candidates = candidates
                  .filter(isVisible)
                  .map(element => ({ element, rect: element.getBoundingClientRect() }))
                  .filter(({ rect }) => rect.right > triggerRect.left && rect.left < triggerRect.right)
                  .map(({ element, rect }) => ({
                    element,
                    verticalDistance: Math.min(
                      Math.abs(rect.top - triggerRect.bottom),
                      Math.abs(rect.bottom - triggerRect.top),
                    ),
                  }))
                  .filter(candidate => candidate.verticalDistance <= 24)
                  .sort((a, b) => a.verticalDistance - b.verticalDistance)
                  .slice(0, 1)
                  .map(candidate => candidate.element);
              } else {
                candidates = [];
              }
            }
            return candidates.some((element) => {
            const style = getComputedStyle(element);
            const rect = element.getBoundingClientRect();
            return style.display !== 'none' && style.visibility !== 'hidden'
              && Number(style.opacity) > 0 && rect.width > 0 && rect.height > 0;
            });
          };
          const check = () => {
            if (startedAt !== undefined && isPaintVisible()) {
              finish(performance.now() - startedAt);
              return;
            }
            rafId = requestAnimationFrame(check);
          };
          const onPointerDown = (event) => {
            const target = event.target;
            if (!(target instanceof Element) || !target.closest(targetSelector)) return;
            startedAt = performance.now();
            observer = new MutationObserver(check);
            observer.observe(document.documentElement, { subtree: true, attributes: true, childList: true });
            check();
          };
          document.addEventListener('pointerdown', onPointerDown, true);
          timeoutId = setTimeout(() => finish(null), timeout);
        });
      }, selector, visibleSelector, timeoutMs, action.portalMatch ?? action[`${side}PortalMatch`]);
      await clickAtCenter(page, selector, timeoutMs);
      const elapsed = await page.evaluate(() => window.__antDesignParityVisibilityPromise);
      if (elapsed === null && action.requireVisible !== false) {
        const state = await page.evaluate(() => {
          const trigger = document.querySelector('.ant-select input[role="combobox"]');
          const root = trigger?.closest('.ant-select');
          const rect = trigger?.getBoundingClientRect();
          return {
            expanded: trigger?.getAttribute('aria-expanded') ?? null,
            focused: document.activeElement === trigger,
            rootClass: root?.className ?? null,
            triggerRect: rect?.toJSON() ?? null,
            hitGrid: rect ? [0.1, 0.5, 0.9].map((x) => [0.2, 0.5, 0.8].map((y) => {
              const hit = document.elementFromPoint(rect.left + rect.width * x, rect.top + rect.height * y);
              return hit === trigger || trigger.contains(hit) ? 'target' : hit?.className?.toString?.().slice(0, 80) ?? hit?.tagName ?? null;
            })) : null,
            visiblePopups: [...document.querySelectorAll('.ant-select-dropdown')].filter((element) => {
              const style = getComputedStyle(element);
              const rect = element.getBoundingClientRect();
              return style.display !== 'none' && style.visibility !== 'hidden'
                && Number(style.opacity) > 0 && rect.width > 0 && rect.height > 0;
            }).length,
          };
        });
        throw new Error(`${side} popup did not become visible after click; state=${JSON.stringify(state)}`);
      }
      return elapsed;
    }
    case 'hover':
      await page.locator(selector).hover({ timeout: timeoutMs });
      break;
    case 'waitForSelector':
      try {
        await page.waitForSelector(selector, {
          ...(action.state === 'hidden' ? { hidden: true } : { visible: true }),
          timeout: timeoutMs,
        });
      } catch (error) {
        const selectState = selector.includes('ant-select')
          ? await page.evaluate(() => {
            const trigger = document.querySelector('.ant-select input[role="combobox"]');
            const root = trigger?.closest('.ant-select');
            const popup = [...document.querySelectorAll('.ant-select-dropdown')].find((element) => {
              const style = getComputedStyle(element);
              const rect = element.getBoundingClientRect();
              return style.display !== 'none' && style.visibility !== 'hidden'
                && Number(style.opacity) > 0 && rect.width > 0 && rect.height > 0;
            });
            return {
              expanded: trigger?.getAttribute('aria-expanded') ?? null,
              focused: document.activeElement === trigger,
              rootClass: root?.className ?? null,
              popupVisible: Boolean(popup),
            };
          })
          : null;
        throw new Error(`${side} ${selector}: ${safeError(error)}${selectState ? `; state=${JSON.stringify(selectState)}` : ''}`);
      }
      break;
    case 'waitForSelectOptions': {
      const visibleLabels = action.visibleLabels ?? [];
      const hiddenLabels = action.hiddenLabels ?? [];
      await page.waitForFunction(({ triggerSelector, visibleLabels, hiddenLabels }) => {
        const trigger = document.querySelector(triggerSelector);
        const input = trigger?.querySelector('input[aria-controls], input[aria-owns]');
        const listId = input?.getAttribute('aria-controls') || input?.getAttribute('aria-owns');
        const list = listId && document.getElementById(listId);
        const popup = list?.closest('.ant-select-dropdown');
        if (!popup) return false;
        const popupStyle = getComputedStyle(popup);
        if (popupStyle.display === 'none' || popupStyle.visibility === 'hidden' || Number(popupStyle.opacity) === 0) return false;
        const options = [...popup.querySelectorAll('.ant-select-item-option')];
        const isVisible = option => {
          const style = getComputedStyle(option);
          const rect = option.getBoundingClientRect();
          return style.display !== 'none' && style.visibility !== 'hidden' && rect.width > 0 && rect.height > 0;
        };
        const labelOf = option => option.getAttribute('title')?.trim() || option.textContent.trim();
        return visibleLabels.every(label => options.some(option => labelOf(option) === label && isVisible(option)))
          && hiddenLabels.every(label => options.every(option => labelOf(option) !== label || !isVisible(option)));
      }, { timeout: timeoutMs, polling: 100 }, {
        triggerSelector: action[`${side}Selector`] ?? action.triggerSelector ?? selector,
        visibleLabels,
        hiddenLabels,
      });
      break;
    }
    case 'waitForPortal': {
      const triggerSelector = action[`${side}TriggerSelector`] ?? action.triggerSelector;
      const portalSelector = action[`${side}PortalSelector`] ?? action.portalSelector;
      if (!triggerSelector || !portalSelector) throw new Error(`${side} triggerSelector and portalSelector are required for waitForPortal`);
      await page.waitForFunction(({ triggerSelector, portalSelector, maxDistance }) => {
        const trigger = document.querySelector(triggerSelector);
        if (!trigger) return false;
        const triggerRect = trigger.getBoundingClientRect();
        return [...document.querySelectorAll(portalSelector)].some(portal => {
          const style = getComputedStyle(portal);
          const rect = portal.getBoundingClientRect();
          if (style.display === 'none' || style.visibility === 'hidden' || Number(style.opacity) === 0
            || rect.width <= 0 || rect.height <= 0 || rect.bottom <= 0 || rect.right <= 0
            || rect.top >= innerHeight || rect.left >= innerWidth) return false;
          const horizontalGap = Math.max(triggerRect.left - rect.right, rect.left - triggerRect.right, 0);
          const verticalGap = Math.max(triggerRect.top - rect.bottom, rect.top - triggerRect.bottom, 0);
          return Math.hypot(horizontalGap, verticalGap) <= maxDistance;
        });
      }, { timeout: timeoutMs }, {
        triggerSelector,
        portalSelector,
        maxDistance: action.maxDistance ?? 100,
      });
      break;
    }
    case 'focus':
      await page.waitForSelector(selector, { timeout: timeoutMs });
      await page.focus(selector);
      break;
    case 'fill':
      if (action.visible) await (await getVisibleTarget()).fill(action.value ?? '', { timeout: timeoutMs });
      else await page.locator(selector).fill(action.value ?? '', { timeout: timeoutMs });
      break;
    case 'typeText':
      await page.waitForSelector(selector, { timeout: timeoutMs });
      await page.focus(selector);
      await page.keyboard.type(action.value ?? '', { delay: action.delayMs ?? 0 });
      break;
    case 'press':
      if (action.visible) await (await getVisibleTarget()).press(action.key, { timeout: timeoutMs });
      else {
        await page.waitForSelector(selector, { timeout: timeoutMs });
        await page.focus(selector);
        await page.keyboard.press(action.key);
      }
      break;
    case 'drag': {
      const fromSelector = action[`${side}FromSelector`] ?? action.fromSelector;
      const toSelector = action[`${side}ToSelector`] ?? action.toSelector;
      if (!fromSelector || !toSelector) throw new Error(`${side} fromSelector and toSelector are required for drag`);
      await page.waitForSelector(fromSelector, { timeout: timeoutMs });
      await page.waitForSelector(toSelector, { timeout: timeoutMs });
      await page.$eval(fromSelector, element => element.scrollIntoView({ block: 'center', inline: 'nearest', behavior: 'instant' }));
      await page.$eval(toSelector, element => element.scrollIntoView({ block: 'center', inline: 'nearest', behavior: 'instant' }));
      const points = await page.evaluate(({ fromSelector, toSelector }) => {
        const center = selector => {
          const element = document.querySelector(selector);
          if (!element) throw new Error(`drag target not found: ${selector}`);
          const rect = element.getBoundingClientRect();
          return { x: rect.left + rect.width / 2, y: rect.top + rect.height / 2 };
        };
        return { from: center(fromSelector), to: center(toSelector) };
      }, { fromSelector, toSelector });
      await page.mouse.move(points.from.x, points.from.y);
      await page.mouse.down();
      await page.mouse.move(points.to.x, points.to.y, { steps: action.steps ?? 8 });
      if (action.durationMs) await new Promise(resolvePromise => setTimeout(resolvePromise, action.durationMs));
      await page.mouse.up();
      const expectedOrder = action[`${side}ExpectedOrder`] ?? action.expectedOrder;
      if (expectedOrder) {
        const headerSelector = action[`${side}HeaderSelector`] ?? action.headerSelector ?? 'thead th';
        await page.waitForFunction(({ headerSelector, expectedOrder }) => {
          const actualOrder = [...document.querySelectorAll(headerSelector)].map(element => element.textContent.trim());
          return JSON.stringify(actualOrder) === JSON.stringify(expectedOrder);
        }, { timeout: timeoutMs }, { headerSelector, expectedOrder });
      }
      break;
    }
    case 'wait':
      await new Promise((resolvePromise) => setTimeout(resolvePromise, action.milliseconds ?? 0));
      break;
    case 'viewport':
      await page.setViewport({
        width: action.width,
        height: action.height,
        deviceScaleFactor: action.deviceScaleFactor ?? 1,
      });
      break;
    default:
      throw new Error(`unsupported action type: ${action.type}`);
  }
}

async function hoverAndVerify(page, selector, timeoutMs) {
  await page.locator(selector).hover({ timeout: timeoutMs });
  const hovered = () => page.waitForFunction((target) => document.querySelector(target)?.matches(':hover') === true,
    { timeout: Math.min(timeoutMs, 2000), polling: 50 }, selector);
  try {
    await hovered();
  } catch {
    // Some multi-page browser runs reactivate a page without restoring its
    // pointer state. Move to the live target rect explicitly, then verify the
    // actual CSS :hover state before waiting for its portal.
    const rect = await page.$eval(selector, (element) => {
      const { x, y, width, height } = element.getBoundingClientRect();
      return { x, y, width, height };
    });
    await page.mouse.move(rect.x + rect.width / 2, rect.y + rect.height / 2);
    await hovered();
  }
}

export function getFinalHoverAction(actions = []) {
  const finalStateAction = [...actions].reverse().find(({ type }) => !['wait', 'waitForSelector', 'waitForPortal'].includes(type));
  return finalStateAction?.type === 'hover' ? finalStateAction : null;
}

export function getPortalWaitTargets(semanticNodes = [], side) {
  const content = semanticNodes.find(({ path, portal, portalVisible, portalText }) =>
    path === 'content' && portal && portalVisible && portalText);
  return content ? [{
    selector: content[`${side}Selector`] ?? content.selector,
    text: content.portalText,
  }] : [];
}

async function waitForVisiblePortals(page, semanticNodes, side, timeoutMs, triggerSelector) {
  const targets = getPortalWaitTargets(semanticNodes, side);
  if (targets.length === 0) return;

  try {
    await page.waitForFunction((portalTargets) => portalTargets.every(({ selector, text }) => {
      const nodes = [...document.querySelectorAll(selector)];
      return nodes.some((element) => {
        const tooltipRoot = element.matches('.ant-tooltip') ? element : element.closest('.ant-tooltip');
        const style = tooltipRoot && getComputedStyle(tooltipRoot);
        const content = element.matches('[role="tooltip"]')
          ? element
          : tooltipRoot?.querySelector('[role="tooltip"]');
        return Boolean(style && style.display !== 'none' && style.visibility !== 'hidden'
          && Number(style.opacity) !== 0 && !tooltipRoot.classList.contains('ant-tooltip-hidden'))
          && (content?.textContent.trim() ?? tooltipRoot?.textContent.trim()) === text;
      });
    }), { timeout: timeoutMs, polling: 100 }, targets);
  } catch (error) {
    const state = await page.evaluate(({ portalTargets, triggerSelector: selector }) => ({
      trigger: (() => {
        const element = document.querySelector(selector);
        return element && { html: element.outerHTML, hovered: element.matches(':hover') };
      })(),
      portals: portalTargets.map(({ selector: portalSelector }) => ({
        selector: portalSelector,
        matches: [...document.querySelectorAll(portalSelector)].map((element) => {
          const root = element.matches('.ant-tooltip') ? element : element.closest('.ant-tooltip');
          const style = root && getComputedStyle(root);
          return { className: root?.className, display: style?.display, visibility: style?.visibility,
            text: element.textContent.trim().slice(0, 80), tooltipText: root?.querySelector('[role="tooltip"]')?.textContent.trim() };
        }).slice(-5),
      })),
    }), { portalTargets: targets, triggerSelector });
    throw new Error(`${safeError(error)} (${JSON.stringify(state)})`);
  }
}

// Read-only collector, also usable through the in-app browser's evaluate API.
export async function capture(page, side, sideSpec, semanticNodes, styleProperties, pseudoElements = [], pseudoStyleProperties = [], includeAntDesignTokens = false) {
  return page.evaluate(captureDomStyles, {
    side,
    scopeSelector: sideSpec.scopeSelector,
    rootSelector: sideSpec.rootSelector,
    semanticNodes,
    styleProperties,
    pseudoElements,
    pseudoStyleProperties,
    includeAntDesignTokens,
  });
}

async function runGroup(reactBrowser, blazorBrowser, group, config) {
  const reactPage = await reactBrowser.newPage();
  const blazorPage = await blazorBrowser.newPage();
  let reactPageClosed = false;
  let blazorPageClosed = false;
  await Promise.all([
    reactPage.setViewport(config.viewport),
    blazorPage.setViewport(config.viewport),
  ]);
  if (config.colorScheme) {
    await Promise.all([
      reactPage.emulateMediaFeatures([{ name: 'prefers-color-scheme', value: config.colorScheme }]),
      blazorPage.emulateMediaFeatures([{ name: 'prefers-color-scheme', value: config.colorScheme }]),
    ]);
  }
  const navigationTimeoutMs = config.navigationTimeoutMs ?? 60000;
  const reactNavigation = await navigate(reactPage, group.react.url, 'react', navigationTimeoutMs);
  const blazorNavigation = await navigate(blazorPage, group.blazor.url, 'blazor', navigationTimeoutMs);
  const result = { id: group.id, component: group.component, scenarios: [] };
  for (const [side, page, sideSpec] of [
    ['react', reactPage, group.react],
    ['blazor', blazorPage, group.blazor],
  ]) {
    for (const action of sideSpec.beforeWaitActions ?? []) {
      await applyAction(page, side, action, config.timeoutMs);
    }
  }
  const minimumRoots = Math.max(1, ...group.scenarios.map((scenario) => scenario.minimumRoots ?? 1));
  let readinessBlocker = [reactNavigation, blazorNavigation]
    .filter((navigation) => !navigation.loaded)
    .map((navigation) => `${navigation.side}: ${navigation.blocker}`)
    .join('; ');
  if (!readinessBlocker) {
    try {
      await waitForDemo(reactPage, group.react, config.timeoutMs, minimumRoots);
    } catch (error) {
      readinessBlocker = `react: ${safeError(error)}`;
    }
  if (!readinessBlocker) {
    try {
      await waitForDemo(blazorPage, group.blazor, config.timeoutMs, minimumRoots);
      } catch (error) {
        readinessBlocker = `blazor: ${safeError(error)}`;
      }
    }
  }

  if (!readinessBlocker && group.postMountSettleMs > 0) {
    // Interactive component setup (including JavaScript event bindings) can
    // finish after the first demo roots appear. Let it settle before timing a
    // click-to-visible interaction.
    await new Promise(resolvePromise => setTimeout(resolvePromise, group.postMountSettleMs));
    await waitForDemo(blazorPage, group.blazor, config.timeoutMs, minimumRoots, { scroll: false });
  }

  for (const scenario of group.scenarios) {
    const scenarioResult = { id: scenario.id, status: 'failed', actions: [] };
    let activeStage = 'scenario setup';
    if (readinessBlocker) {
      scenarioResult.status = 'blocked';
      scenarioResult.blocker = readinessBlocker;
      result.scenarios.push(scenarioResult);
      continue;
    }
    try {
      await Promise.all([
        reactPage.setViewport(config.viewport),
        blazorPage.setViewport(config.viewport),
      ]);
      if (group.matchScopeWidth) {
        const sourceSide = group.matchScopeWidth;
        const targetSide = sourceSide === 'react' ? 'blazor' : 'react';
        const sourcePage = sourceSide === 'react' ? reactPage : blazorPage;
        const targetPage = targetSide === 'react' ? reactPage : blazorPage;
        const sourceSpec = group[sourceSide];
        const targetSpec = group[targetSide];
        await sourcePage.bringToFront();
        await scrollScopeAnchorIntoView(sourcePage, sourceSpec.scopeSelector, sourceSpec.rootSelector, config.timeoutMs, sourceSpec.scrollAnchorSelector);
        await sourcePage.waitForSelector(sourceSpec.scopeSelector, { timeout: config.timeoutMs });
        const sourceWidth = await sourcePage.$eval(sourceSpec.scopeSelector,
          (element) => element.getBoundingClientRect().width);
        await targetPage.bringToFront();
        await scrollScopeAnchorIntoView(targetPage, targetSpec.scopeSelector, targetSpec.rootSelector, config.timeoutMs, targetSpec.scrollAnchorSelector);
        await targetPage.waitForSelector(targetSpec.scopeSelector, { timeout: config.timeoutMs });
        await targetPage.$eval(targetSpec.scopeSelector, (element, width) => {
          element.style.width = `${width}px`;
        }, sourceWidth);
      }
      if (Number.isFinite(group.alignRootViewportY)) {
        await reactPage.bringToFront();
        await alignRootViewportY(reactPage, group.react.rootSelector, group.alignRootViewportY, config.timeoutMs);
        await blazorPage.bringToFront();
        await alignRootViewportY(blazorPage, group.blazor.rootSelector, group.alignRootViewportY, config.timeoutMs);
      }
      const finalHover = getFinalHoverAction(scenario.actions);
      const actions = scenario.actions ?? [];
      const runSideActions = async (side, page) => {
        // The previous side may have been brought to the front to capture its
        // popup before it blurs. Activate this page before sending clicks or
        // keyboard input so browser focus-dependent state is exercised in the
        // same foreground context on both implementations.
        await page.bringToFront();
        if (group.singleBrowserContext) {
          await page.waitForFunction(() => document.visibilityState === 'visible', { timeout: config.timeoutMs });
          // Blazor Server can need one event-loop turn after tab activation to
          // resume its browser-side event dispatch before the next pointer input.
          await new Promise(resolvePromise => setTimeout(resolvePromise, 250));
        }
        for (const action of actions) {
          activeStage = `${side} ${action.type} action`;
          const durationMs = await applyAction(page, side, action, config.timeoutMs);
          if (action.type === 'measureClickVisible') {
            scenarioResult.timings ??= {};
            scenarioResult.timings[action.id ?? action.type] ??= { unit: 'ms' };
            scenarioResult.timings[action.id ?? action.type][side] = durationMs;
          }
          if (action.stableForMs) {
            await new Promise((resolvePromise) => setTimeout(resolvePromise, action.stableForMs));
          }
        }
      };
      const captureSide = async (side, page, sideSpec) => {
        // Capture each app while it is active. React's CSS-in-JS size changes
        // transition for 200ms; background capture can leave computed boxes stale.
        activeStage = `${side} final hover and portal readiness`;
        await page.bringToFront();
        // Locator clicks leave the pointer at each page's trigger coordinates.
        // Different documentation layouts can place those triggers differently,
        // which makes the final popup option acquire a different :hover/active
        // state even though the scenario did not request a hover comparison.
        // Start every capture from the same neutral pointer state; explicit
        // hover scenarios re-enter their target below.
        // A visible portal may be bound to the trigger's focused/open state.
        // Moving the pointer to the page corner after a popup action can cause
        // some components to blur or close before capture, so keep the pointer
        // on the trigger for open-popup scenarios.
        const preserveOpenPopup = actions.some(action => action.type === 'measureClickVisible');
        if (!preserveOpenPopup) await page.mouse.move(0, 0);
        await waitForDemo(page, sideSpec, config.timeoutMs, minimumRoots, { scroll: false });
        await new Promise((resolvePromise) => setTimeout(resolvePromise, 800));
        await waitForDemo(page, sideSpec, config.timeoutMs, minimumRoots, { scroll: false });
        if (finalHover) {
          const selector = finalHover[`${side}Selector`] ?? finalHover.selector;
          // Re-enter hover from a neutral pointer location; background pages
          // may retain coordinates without retaining :hover.
          await page.mouse.move(0, 0);
          await new Promise((resolvePromise) => setTimeout(resolvePromise, 250));
          await hoverAndVerify(page, selector, config.timeoutMs);
          await waitForVisiblePortals(page, group.semanticNodes, side, config.timeoutMs, selector);
          scenarioResult.finalHoverState ??= {};
          scenarioResult.finalHoverState[side] = await page.$eval(selector,
            (element) => ({ html: element.outerHTML, hovered: element.matches(':hover') }));
          await new Promise((resolvePromise) => setTimeout(resolvePromise, 800));
        }
        activeStage = `${side} SVG glyph readiness`;
        await waitForSvgUses(page, side, sideSpec, group.semanticNodes, config.timeoutMs);
        activeStage = `${side} DOM/CSS capture`;
        return capture(page, side, sideSpec, group.semanticNodes,
          group.styleProperties ?? DEFAULT_STYLE_PROPERTIES,
          group.pseudoElements ?? ['::before', '::after'],
          group.pseudoStyleProperties ?? DEFAULT_PSEUDO_STYLE_PROPERTIES,
          group.includeAntDesignTokens ?? false);
      };

      const alignTargetForCapture = async (page, sideSpec) => {
        if (!Number.isFinite(group.alignRootViewportY)) return;
        await alignRootViewportY(page, sideSpec.rootSelector, group.alignRootViewportY, config.timeoutMs);
        if (actions.some(action => action.type === 'measureClickVisible')) {
          const point = await page.$eval(sideSpec.rootSelector, (element) => {
            const rect = element.getBoundingClientRect();
            return { x: rect.left + rect.width / 2, y: rect.top + rect.height / 2 };
          });
          await page.mouse.move(point.x, point.y);
        }
      };

      let react;
      let blazor;
      if (group.captureBlazorBeforeReact) {
        // Keep the Blazor Server circuit active while its browser-side events
        // run and its state is captured. React is client-only and can safely
        // be activated after the Blazor snapshot is complete.
        await runSideActions('blazor', blazorPage);
        scenarioResult.actions = actions.map((action) => action.id ?? action.type);
        await alignTargetForCapture(blazorPage, group.blazor);
        blazor = await captureSide('blazor', blazorPage, group.blazor);
        if (scenario === group.scenarios.at(-1)) {
          await blazorPage.close();
          blazorPageClosed = true;
        }
        await runSideActions('react', reactPage);
        await alignTargetForCapture(reactPage, group.react);
        react = await captureSide('react', reactPage, group.react);
      } else if (group.captureReactBeforeBlazor) {
        // Some popup components close on blur. Finish and snapshot React before
        // activating Blazor so the portal remains in the intended open state.
        // Close React's page before interacting with Blazor: Chromium can
        // throttle a background interactive-server page while React's popup
        // is being captured, leaving its first pointer/focus callbacks queued.
        await runSideActions('react', reactPage);
        scenarioResult.actions = actions.map((action) => action.id ?? action.type);
        await alignTargetForCapture(reactPage, group.react);
        react = await captureSide('react', reactPage, group.react);
        if (scenario === group.scenarios.at(-1)) {
          await reactPage.close();
          reactPageClosed = true;
        }
        await runSideActions('blazor', blazorPage);
        await alignTargetForCapture(blazorPage, group.blazor);
        blazor = await captureSide('blazor', blazorPage, group.blazor);
      } else {
        for (const action of actions) {
          activeStage = `react ${action.type} action`;
          const reactDurationMs = await applyAction(reactPage, 'react', action, config.timeoutMs);
          activeStage = `blazor ${action.type} action`;
          const blazorDurationMs = await applyAction(blazorPage, 'blazor', action, config.timeoutMs);
          if (action.type === 'measureClickVisible') {
            scenarioResult.timings ??= {};
            scenarioResult.timings[action.id ?? action.type] = {
              unit: 'ms',
              react: reactDurationMs,
              blazor: blazorDurationMs,
              delta: Number.isFinite(reactDurationMs) && Number.isFinite(blazorDurationMs)
                ? blazorDurationMs - reactDurationMs
                : null,
            };
          }
          if (action.stableForMs) {
            await new Promise((resolvePromise) => setTimeout(resolvePromise, action.stableForMs));
          }
          scenarioResult.actions.push(action.id ?? action.type);
        }
        await alignTargetForCapture(reactPage, group.react);
        await alignTargetForCapture(blazorPage, group.blazor);
        react = await captureSide('react', reactPage, group.react);
        blazor = await captureSide('blazor', blazorPage, group.blazor);
      }
      const comparisonPolicy = group.comparisonPolicy ?? config.comparisonPolicy;
      const comparison = compareDomSnapshots(react, blazor, {
        ...comparisonPolicy,
        geometryTolerancePx: group.geometryTolerancePx ?? config.comparisonPolicy.geometryTolerancePx ?? 0,
        computedStylePixelToleranceByProperty: group.computedStylePixelToleranceByProperty
          ?? comparisonPolicy.computedStylePixelToleranceByProperty,
        semanticMinimumCounts: scenario.semanticMinimumCounts
          ?? Object.fromEntries((group.semanticNodes ?? []).map(({ path, minimumCount }) => [path, minimumCount ?? 0])),
        svgUsePaths: (group.semanticNodes ?? []).filter(node => node.verifySvgUse).map(node => node.path),
      });
      scenarioResult.status = comparison.equal ? 'passed' : 'differences';
      scenarioResult.rootCount = { react: react.rootCount, blazor: blazor.rootCount };
      scenarioResult.comparison = comparison;
      scenarioResult.samples = { react, blazor };
    } catch (error) {
      scenarioResult.status = 'blocked';
      scenarioResult.blocker = `${activeStage}: ${safeError(error)}`;
    }
    result.scenarios.push(scenarioResult);
  }

  result.navigationErrors = { react: reactNavigation.errors, blazor: blazorNavigation.errors };
  await Promise.allSettled([
    ...(reactPageClosed ? [] : [reactPage.close()]),
    ...(blazorPageClosed ? [] : [blazorPage.close()]),
  ]);
  return result;
}

export async function runDomStyleParity(manifest, { puppeteer, executablePath } = {}) {
  validateParityManifest(manifest);
  const config = {
    ...manifest.config,
    timeoutMs: manifest.config?.timeoutMs ?? 20000,
    navigationTimeoutMs: manifest.config?.navigationTimeoutMs ?? 60000,
    viewport: manifest.config?.viewport ?? { width: 1280, height: 900, deviceScaleFactor: 1 },
    comparisonPolicy: manifest.config?.comparisonPolicy ?? {},
  };
  const browserType = puppeteer ?? puppeteerFor(manifest.reactRoot);
  const launchOptions = {
    ...(executablePath ? { executablePath } : {}),
    headless: true,
    args: ['--no-sandbox', '--disable-dev-shm-usage'],
  };
  const reactBrowser = await browserType.launch(launchOptions);
  let blazorBrowser;
  try {
    const singleBrowserContext = manifest.groups.every(group => group.singleBrowserContext === true);
    if (singleBrowserContext) {
      // Some Blazor Server environments drop the first interactive event when
      // React and Blazor run in separate Chromium processes. These manifests
      // capture Blazor first, so a shared browser keeps the circuit active and
      // does not background it before the interaction snapshot is taken.
      blazorBrowser = reactBrowser;
    } else {
      // Keep interactive Blazor Server in an independent browser process for
      // the default path; Chromium may throttle its circuit when React is active.
      blazorBrowser = await browserType.launch(launchOptions);
    }
    const groups = [];
    for (const group of manifest.groups) groups.push(await runGroup(reactBrowser, blazorBrowser, group, config));
    const scenarios = groups.flatMap((group) => group.scenarios);
    return {
      schemaVersion: 1,
      generatedAt: new Date().toISOString(),
      upstream: manifest.upstream ?? null,
      summary: {
        groups: groups.length,
        scenarios: scenarios.length,
        passed: scenarios.filter(({ status }) => status === 'passed').length,
        differences: scenarios.filter(({ status }) => status === 'differences').length,
        blocked: scenarios.filter(({ status }) => status === 'blocked').length,
        visualStylePassed: scenarios.filter(({ comparison }) => comparison?.visualStyles?.equal).length,
        visualStyleDifferences: scenarios.filter(({ comparison }) => comparison && !comparison.visualStyles?.equal).length,
      },
      groups,
    };
  } finally {
    if (blazorBrowser === reactBrowser) {
      await reactBrowser.close();
    } else {
      await Promise.allSettled([
        reactBrowser.close(),
        ...(blazorBrowser ? [blazorBrowser.close()] : []),
      ]);
    }
  }
}

export function validateParityManifest(manifest) {
  if (manifest?.schemaVersion !== 1 || !Array.isArray(manifest.groups) || manifest.groups.length === 0) {
    throw new Error('scenario manifest must be schemaVersion 1 and contain at least one group');
  }
  for (const group of manifest.groups) {
    if (!group.id || !group.react?.url || !group.blazor?.url) {
      throw new Error('each group requires id and React/Blazor URLs');
    }
    if (group.singleBrowserContext !== undefined && typeof group.singleBrowserContext !== 'boolean') {
      throw new Error(`${group.id}.singleBrowserContext must be a boolean`);
    }
    for (const side of ['react', 'blazor']) {
      if (!group[side].scopeSelector || !group[side].rootSelector) {
        throw new Error(`${group.id}.${side} requires scopeSelector and rootSelector`);
      }
      for (const action of group[side].beforeWaitActions ?? []) {
        if (!['click', 'hover', 'focus', 'fill', 'typeText', 'press', 'wait'].includes(action.type)) {
          throw new Error(`${group.id}.${side} has unsupported beforeWaitAction ${action.type}`);
        }
        if (!action.selector && !action[`${side}Selector`]) {
          throw new Error(`${group.id}.${side} beforeWaitAction requires a selector`);
        }
      }
    }
    if (!Array.isArray(group.scenarios) || group.scenarios.length === 0) {
      throw new Error(`${group.id} must contain at least one scenario`);
    }
    if (group.matchScopeWidth !== undefined && !['react', 'blazor'].includes(group.matchScopeWidth)) {
      throw new Error(`${group.id}.matchScopeWidth must be react or blazor`);
    }
    if (group.alignRootViewportY !== undefined && !Number.isFinite(group.alignRootViewportY)) {
      throw new Error(`${group.id}.alignRootViewportY must be a finite viewport coordinate`);
    }
    const scenarioIds = new Set();
    for (const scenario of group.scenarios) {
      if (!scenario.id || scenarioIds.has(scenario.id)) throw new Error(`${group.id} has a missing or duplicate scenario id`);
      scenarioIds.add(scenario.id);
      if (scenario.semanticMinimumCounts !== undefined) {
        if (!scenario.semanticMinimumCounts || typeof scenario.semanticMinimumCounts !== 'object' || Array.isArray(scenario.semanticMinimumCounts)) {
          throw new Error(`${group.id}.${scenario.id}.semanticMinimumCounts must be an object`);
        }
        for (const [path, count] of Object.entries(scenario.semanticMinimumCounts)) {
          if (!path || !Number.isInteger(count) || count < 0) {
            throw new Error(`${group.id}.${scenario.id} has an invalid semantic minimum count for ${path}`);
          }
        }
      }
      for (const action of scenario.actions ?? []) {
        if (!['click', 'measureClickVisible', 'hover', 'focus', 'fill', 'typeText', 'press', 'drag', 'wait', 'waitForSelector', 'waitForSelectOptions', 'waitForPortal', 'viewport'].includes(action.type)) throw new Error(`${group.id}.${scenario.id} has unsupported action ${action.type}`);
        const hasActionSelector = side => action.selector || action[`${side}Selector`]
          || (action.type === 'drag' && (action[`${side}FromSelector`] ?? action.fromSelector)
            && (action[`${side}ToSelector`] ?? action.toSelector))
          || (action.type === 'waitForPortal' && action[`${side}TriggerSelector`]);
        if (!['wait', 'viewport'].includes(action.type) && !hasActionSelector('react')) throw new Error(`${group.id}.${scenario.id} has an action without a React selector`);
        if (!['wait', 'viewport'].includes(action.type) && !hasActionSelector('blazor')) throw new Error(`${group.id}.${scenario.id} has an action without a Blazor selector`);
        if (action.type === 'measureClickVisible' && !(action.visibleSelector || action.reactVisibleSelector || action.blazorVisibleSelector)) {
          throw new Error(`${group.id}.${scenario.id} measureClickVisible requires a visible selector`);
        }
      }
    }
    if (!Array.isArray(group.semanticNodes) || group.semanticNodes.length === 0) {
      throw new Error(`${group.id} must declare Semantic DOM paths`);
    }
    for (const node of group.semanticNodes) {
      if (!node.path) throw new Error(`${group.id} has a Semantic DOM node without a path`);
      for (const side of ['react', 'blazor']) {
        if (!(node[`${side}Selector`] ?? node.selector)) {
          throw new Error(`${group.id}.${node.path} requires a ${side} selector`);
        }
      }
      if (node.minimumCount !== undefined && (!Number.isInteger(node.minimumCount) || node.minimumCount < 0)) {
        throw new Error(`${group.id}.${node.path} has an invalid minimumCount`);
      }
    }
  }
  return manifest;
}

if (import.meta.url === `file://${process.argv[1]}`) {
  const args = parseArgs(process.argv.slice(2));
  if (!args.cases || !args['antd-path']) {
    usage();
    process.exit(2);
  }
  const manifestPath = resolve(args.cases);
  const manifest = JSON.parse(await readFile(manifestPath, 'utf8'));
  manifest.reactRoot = resolve(args['antd-path']);
  manifest.config ??= {};
  manifest.config.timeoutMs ??= 20000;
  if (args.timeout) manifest.config.timeoutMs = Number(args.timeout);
  manifest.config.viewport ??= { width: 1280, height: 900, deviceScaleFactor: 1 };
  const report = await runDomStyleParity(manifest, { executablePath: args.chrome ?? process.env.CHROME_BIN });
  const reportPath = args.report ? resolve(args.report) : null;
  if (reportPath) {
    await mkdir(dirname(reportPath), { recursive: true });
    await writeFile(reportPath, `${JSON.stringify(report, null, 2)}\n`, 'utf8');
  }
  console.log(JSON.stringify({ report: reportPath, ...report.summary }, null, 2));
  if (report.summary.differences || report.summary.blocked) process.exitCode = 1;
}
