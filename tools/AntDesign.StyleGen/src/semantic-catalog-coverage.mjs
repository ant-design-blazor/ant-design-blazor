/** Reads the synchronized React Semantic DOM catalog JSON. */
export function parseSemanticCatalog(source) {
  const catalogJson = typeof source === 'string' ? JSON.parse(source) : source;
  const components = new Map();
  for (const component of catalogJson.components ?? []) {
    components.set(component.name, new Set((component.nodes ?? []).map((node) => node.name)));
  }
  return components;
}

export function getSemanticCatalogCoverage(inventory, source, documentedSource = null) {
  // The DatePicker Dumi preview calls the popup node `popup.root`, while the
  // React semantic.md page names the same `.ant-picker-dropdown` node `popup`.
  const documentedPathAliases = new Map([['date-picker/popup.root', 'date-picker/popup']]);
  const catalog = parseSemanticCatalog(source);
  const entries = inventory.entries ?? [];
  const componentNames = [...new Set(entries.map(({ component }) => component))];
  const slots = entries.flatMap((entry) => (entry.classNames ?? []).map((slot) => ({ component: entry.component, slot })));
  const mappedSlots = slots.filter(({ component, slot }) => catalog.get(component)?.has(slot)).length;
  const definitions = entries.filter((entry) => catalog.has(entry.component)
    && (entry.classNames ?? []).every((slot) => catalog.get(entry.component).has(slot))).length;
  const dumiPaths = new Set(entries.flatMap((entry) => (entry.dumiSemanticSources ?? [])
    .flatMap((sourceEntry) => (sourceEntry.semanticPaths ?? []).map((semanticPath) => `${entry.component}/${semanticPath}`))));
  const mappedDumiPaths = [...dumiPaths].filter((path) => {
    const separator = path.indexOf('/');
    const component = path.slice(0, separator);
    const semanticPath = path.slice(separator + 1);
    return catalog.get(component)?.has(semanticPath)
      || documentedPathAliases.has(path) && catalog.get(component)?.has(documentedPathAliases.get(path).slice(separator + 1));
  }).length;
  const documentedPaths = new Set((documentedSource?.pages ?? []).flatMap((page) =>
    (page.parts ?? []).map((part) => `${page.component}/${part.name}`)));
  const mappedDocumentedPaths = [...documentedPaths].filter((path) => {
    const separator = path.indexOf('/');
    const component = path.slice(0, separator);
    const semanticPath = path.slice(separator + 1);
    return catalog.get(component)?.has(semanticPath);
  }).length;
  const documentedComponents = new Set([...documentedPaths].map((path) => path.slice(0, path.indexOf('/'))));
  return {
    catalogComponents: { total: componentNames.length, migrated: componentNames.filter((name) => catalog.has(name)).length },
    definitions: { total: entries.length, migrated: definitions },
    apiSlots: { total: slots.length, migrated: mappedSlots },
    dumiPaths: { total: dumiPaths.size, migrated: mappedDumiPaths },
    documentedComponents: {
      total: documentedComponents.size,
      migrated: [...documentedComponents].filter((name) => catalog.has(name)).length,
    },
    documentedSlots: { total: documentedPaths.size, migrated: mappedDocumentedPaths },
  };
}
