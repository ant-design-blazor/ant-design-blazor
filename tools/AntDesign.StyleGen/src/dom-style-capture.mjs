// Read-only browser collector. No Node imports, framework access or DOM writes.
// The same function can be passed to Puppeteer or the in-app browser evaluate API.
export function captureDomStyles({ side, scopeSelector, rootSelector, semanticNodes = [], styleProperties = [], pseudoElements = [], pseudoStyleProperties = [], includeAntDesignTokens = false }) {
  const scope = document.querySelector(scopeSelector);
  if (!scope) throw new Error(`capture scope not found: ${scopeSelector}`);
  const roots = [...scope.querySelectorAll(rootSelector)];
  if (!roots.length) throw new Error(`capture roots not found: ${scopeSelector} ${rootSelector}`);
  const readStyle = (element, includeTokenVariables = false) => {
    const computed = getComputedStyle(element);
    const properties = new Set(styleProperties);
    if (includeTokenVariables && includeAntDesignTokens) {
      const tokenPrefixes = Array.isArray(includeAntDesignTokens)
        ? includeAntDesignTokens
        : ['--ant-'];
      for (let index = 0; index < computed.length; index += 1) {
        const property = computed.item(index);
        if (tokenPrefixes.some(prefix => property.startsWith(prefix))) properties.add(property);
      }
    }
    return Object.fromEntries([...properties].map(property => [property,
      computed.getPropertyValue(property.startsWith('--') ? property : property.replace(/[A-Z]/g, letter => `-${letter.toLowerCase()}`)).trim()]));
  };
  const readComputedStyle = (element, includeTokenVariables = false) => {
    const styles = readStyle(element, includeTokenVariables);
    for (const pseudo of pseudoElements) {
      const computed = getComputedStyle(element, pseudo);
      for (const property of pseudoStyleProperties) {
        const cssName = property.startsWith('--') ? property : property.replace(/[A-Z]/g, letter => `-${letter.toLowerCase()}`);
        styles[`${pseudo}.${property}`] = computed.getPropertyValue(cssName).trim();
      }
    }
    return styles;
  };
  const semantic = {};
  const semanticElements = {};
  for (const definition of semanticNodes) {
    let portalTrigger = null;
    const selector = definition[`${side}Selector`] ?? definition.selector;
    let nodes = definition.portalParentPath
      ? (semanticElements[definition.portalParentPath] ?? []).flatMap(parent =>
        selector === '$root' ? [parent] : [...parent.querySelectorAll(selector)])
      : definition.portal
      ? [...document.querySelectorAll(selector)].filter(element => {
        const tooltip = element.matches('.ant-tooltip') ? element : element.closest('.ant-tooltip');
        if (definition.portalVisible) {
          const portal = tooltip ?? element.closest('.ant-picker-dropdown, .ant-select-dropdown, .ant-cascader-dropdown, .ant-dropdown, .ant-popover, .ant-tooltip, .ant-menu-submenu');
          const style = portal && getComputedStyle(portal);
          const rect = portal?.getBoundingClientRect();
          if (!style || style.display === 'none' || style.visibility === 'hidden' || Number(style.opacity) === 0
            || portal.classList.contains('ant-tooltip-hidden') || portal.classList.contains('ant-picker-dropdown-hidden')
            || !rect || rect.width <= 0 || rect.height <= 0 || rect.bottom <= 0 || rect.right <= 0
            || rect.top >= innerHeight || rect.left >= innerWidth) return false;
        }
        if (!definition.portalText) return true;
        const textNode = element.matches('[role="tooltip"]') ? element : tooltip?.querySelector('[role="tooltip"]');
        return textNode?.textContent.trim() === definition.portalText;
      })
      : roots.flatMap(root => selector === '$root' ? [root] : [...root.querySelectorAll(selector)]);
    if (definition.portal && definition.portalMatch === 'nearest-trigger') {
      const triggerSelector = definition[`${side}TriggerSelector`] ?? definition.triggerSelector;
      const trigger = triggerSelector ? document.querySelector(triggerSelector) : roots[0];
      if (!trigger) throw new Error(`portal trigger not found: ${triggerSelector ?? rootSelector}`);
      portalTrigger = trigger;
      const triggerRect = trigger.getBoundingClientRect();
      const distanceToTrigger = element => {
        const rect = element.getBoundingClientRect();
        const horizontalGap = Math.max(triggerRect.left - rect.right, rect.left - triggerRect.right, 0);
        const verticalGap = Math.max(triggerRect.top - rect.bottom, rect.top - triggerRect.bottom, 0);
        return horizontalGap ** 2 + verticalGap ** 2;
      };
      nodes = nodes.sort((left, right) => distanceToTrigger(left) - distanceToTrigger(right)).slice(0, 1);
    }
    semanticElements[definition.path] = nodes;
    semantic[definition.path] = nodes.map(element => ({
      html: element.outerHTML,
      tag: element.tagName.toLowerCase(),
      computedStyle: readComputedStyle(element),
      ...(definition.verifySvgUse ? (() => {
        if (!element.matches('use')) throw new Error(`SVG use verification requires a use element: ${definition.path}`);
        const href = element.getAttribute('xlink:href') ?? element.getAttribute('href');
        const rect = element.getBoundingClientRect();
        return { svgUse: {
          href, width: rect.width, height: rect.height,
          definitions: [...document.querySelectorAll('symbol[id]')]
            .filter(symbol => `#${symbol.getAttribute('id')}` === href).map(symbol => symbol.outerHTML),
        } };
      })() : {}),
      ...(definition.portal ? (() => {
        const rect = element.getBoundingClientRect();
        const trigger = (portalTrigger ?? roots[0]).getBoundingClientRect();
        const relativeGeometry = { left: rect.left - trigger.left, top: rect.top - trigger.top, width: rect.width, height: rect.height };
        const viewportGeometry = { left: rect.left, top: rect.top, right: rect.right, bottom: rect.bottom, width: rect.width, height: rect.height };
        const triggerViewportGeometry = { left: trigger.left, top: trigger.top, right: trigger.right, bottom: trigger.bottom, width: trigger.width, height: trigger.height };
        let comparisonGeometry = relativeGeometry;
        if (definition.geometryAnchor?.mode === 'viewport') {
          comparisonGeometry = viewportGeometry;
        } else if (definition.geometryAnchor?.mode === 'pixel-snap-to-trigger') {
          comparisonGeometry = {
            left: rect.left - Math.floor(trigger.left),
            top: rect.top - Math.floor(trigger.bottom + (definition.geometryAnchor.verticalOffset ?? 0)),
            width: rect.width,
            height: rect.height,
          };
        } else if (definition.geometryParentSelector) {
          const parent = element.closest?.(definition.geometryParentSelector) ?? document.querySelector(definition.geometryParentSelector);
          if (parent) {
            const parentRect = parent.getBoundingClientRect();
            comparisonGeometry = {
              left: rect.left - parentRect.left,
              top: rect.top - parentRect.top,
              width: rect.width,
              height: rect.height,
            };
          }
        }
        return { relativeGeometry, comparisonGeometry, viewportGeometry, triggerViewportGeometry };
      })() : {}),
    }));
  }
  return {
    html: roots.map(root => root.outerHTML).join('\n'),
    semantic,
    computedStyles: Object.fromEntries(roots.map((element, index) => [`root[${index}]`, readComputedStyle(element, true)])),
    rootCount: roots.length,
    environment: { viewportWidth: innerWidth, viewportHeight: innerHeight, devicePixelRatio, zoom: getComputedStyle(document.documentElement).zoom },
  };
}
