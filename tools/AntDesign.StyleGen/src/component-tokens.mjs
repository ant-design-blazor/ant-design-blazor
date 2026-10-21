export function extractComponentTokenDefinitions(source) {
  const interfaces = new Map();
  for (const match of source.matchAll(/interface\s+([A-Za-z_$][\w$]*)(?:\s+extends\s+([^\{]+))?\s*\{([\s\S]*?)\}/g)) {
    interfaces.set(match[1], {
      parents: (match[2] ?? '').split(',').map((name) => name.trim().split(/[<\s]/)[0]).filter(Boolean),
      body: match[3],
    });
  }

  const definitions = new Map();
  const visit = (name, visited = new Set()) => {
    if (visited.has(name)) return;
    visited.add(name);
    const definition = interfaces.get(name);
    if (!definition) return;
    for (const parent of definition.parents) visit(parent, visited);
    const propertyPattern = /(?:\/\*\*([\s\S]*?)\*\/\s*)?^\s*([A-Za-z_$][\w$]*)\??\s*:\s*([^;\n]+);?/gm;
    for (const property of definition.body.matchAll(propertyPattern)) {
      const [, comment = '', propertyName, type] = property;
      const description = comment.match(/^\s*\*?\s*@desc\s+(.+?)\s*$/m)?.[1]?.trim();
      const descriptionEn = comment.match(/^\s*\*?\s*@descEN\s+(.+?)\s*$/m)?.[1]?.trim();
      const deprecated = /^\s*\*?\s*@deprecated\b/m.test(comment);
      const token = { name: propertyName, type: type.trim() };
      if (description) token.description = description;
      if (descriptionEn) token.descriptionEn = descriptionEn;
      if (deprecated) token.deprecated = true;
      const previous = definitions.get(propertyName);
      definitions.set(propertyName, { ...previous, ...token });
    }
  };
  visit('ComponentToken');
  return [...definitions].sort(([left], [right]) => left.localeCompare(right)).map(([, token]) => token);
}
