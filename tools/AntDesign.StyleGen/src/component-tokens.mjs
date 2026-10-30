// Shared by typed token properties and documentation metadata. Keep the React
// source type separately; docs must not present it as a C# property type.
export function csharpTokenType(type) {
  const source = String(type ?? 'unknown').replace(/\s+/g, ' ').trim();
  const members = source.includes('|') ? source.split('|').map(item => item.trim()) : [source];
  const mapped = [...new Set(members.map(member => {
    if (/^(string|CSSProperties\[|CSSObject\[|React\.CSSProperties)/.test(member)) return member === 'string' ? 'string' : 'OneOf<string, double>';
    if (/^(number|CSSNumericValue)/.test(member)) return 'double';
    if (member === 'boolean' || /^(true|false)$/.test(member)) return 'bool';
    return 'string';
  }))];
  return mapped.length === 1 ? mapped[0] : `OneOf<${mapped.join(', ')}>`;
}

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
