// Read-only browser collector. No Node imports, framework access or DOM writes.
// The same function can be passed to Puppeteer or the in-app browser evaluate API.
export function captureDomStyles({ side, scopeSelector, rootSelector, semanticNodes = [], styleProperties = [], pseudoElements = [], pseudoStyleProperties = [] }) {
  const scope = document.querySelector(scopeSelector);
  if (!scope) throw new Error(`capture scope not found: ${scopeSelector}`);
  const roots = [...scope.querySelectorAll(rootSelector)];
  if (!roots.length) throw new Error(`capture roots not found: ${scopeSelector} ${rootSelector}`);
  const readStyle = element => {
    const computed = getComputedStyle(element);
    return Object.fromEntries(styleProperties.map(property => [property, computed.getPropertyValue(property.startsWith('--') ? property : property.replace(/[A-Z]/g, letter => `-${letter.toLowerCase()}`)).trim()]));
  };
  const readComputedStyle = element => {
    const styles = readStyle(element);
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
  for (const definition of semanticNodes) {
    const selector = definition[`${side}Selector`] ?? definition.selector;
    const nodes = definition.portal
      ? [...document.querySelectorAll(selector)].filter(element => {
        const tooltip = element.matches('.ant-tooltip') ? element : element.closest('.ant-tooltip');
        if (definition.portalVisible) {
          const style = tooltip && getComputedStyle(tooltip);
          if (!style || style.display === 'none' || style.visibility === 'hidden' || Number(style.opacity) === 0 || tooltip.classList.contains('ant-tooltip-hidden')) return false;
        }
        if (!definition.portalText) return true;
        const textNode = element.matches('[role="tooltip"]') ? element : tooltip?.querySelector('[role="tooltip"]');
        return textNode?.textContent.trim() === definition.portalText;
      })
      : roots.flatMap(root => selector === '$root' ? [root] : [...root.querySelectorAll(selector)]);
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
        const trigger = roots[0].getBoundingClientRect();
        return { relativeGeometry: { left: rect.left - trigger.left, top: rect.top - trigger.top, width: rect.width, height: rect.height } };
      })() : {}),
    }));
  }
  return {
    html: roots.map(root => root.outerHTML).join('\n'),
    semantic,
    computedStyles: Object.fromEntries(roots.map((element, index) => [`root[${index}]`, readComputedStyle(element)])),
    rootCount: roots.length,
    environment: { viewportWidth: innerWidth, viewportHeight: innerHeight, devicePixelRatio, zoom: getComputedStyle(document.documentElement).zoom },
  };
}
