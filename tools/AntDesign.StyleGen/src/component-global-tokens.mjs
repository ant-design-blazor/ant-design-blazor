const normalizeComponent = value => value.replace(/[^a-z0-9]/gi, '').toLowerCase();

export function collectComponentGlobalTokens(tokenData, metadata) {
  return Object.fromEntries(Object.entries(tokenData).filter(([, value]) => Array.isArray(value?.global))
    .map(([component, value]) => [normalizeComponent(component), [...new Set(value.global)]
      .filter(name => Object.hasOwn(metadata.global ?? {}, name))
      .sort((left, right) => {
        const color = Number(!left.toLowerCase().includes('color')) - Number(!right.toLowerCase().includes('color'));
        return color || (left < right ? -1 : left > right ? 1 : 0);
      })])
    .sort(([left], [right]) => left.localeCompare(right)));
}

export function renderComponentGlobalTokenCatalog(tokenData, metadata) {
  const entries = collectComponentGlobalTokens(tokenData, metadata);
  return [
    '    public static class GeneratedComponentGlobalTokens',
    '    {',
    '        public static IReadOnlyDictionary<string, IReadOnlyList<string>> All { get; } =',
    '            new ReadOnlyDictionary<string, IReadOnlyList<string>>(new Dictionary<string, IReadOnlyList<string>>(System.StringComparer.OrdinalIgnoreCase)',
    '            {',
    ...Object.entries(entries).map(([component, names]) => `                [${JSON.stringify(component)}] = new string[] { ${names.map(name => JSON.stringify(name)).join(', ')} },`),
    '            });',
    '    }',
    '',
  ].join('\n');
}
