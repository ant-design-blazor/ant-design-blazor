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
  const interfaces = parseInterfaces(source);

  const visit = (name, visiting = new Set()) => {
    if (visiting.has(name)) return new Map();
    const nextVisiting = new Set(visiting).add(name);
    const definition = interfaces.get(name);
    if (!definition) return new Map();
    const definitions = new Map();
    for (const { name: parent, excluded } of definition.parents) {
      const inherited = visit(parent, nextVisiting);
      for (const excludedName of excluded) inherited.delete(excludedName);
      for (const [propertyName, token] of inherited) definitions.set(propertyName, token);
    }
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
    return definitions;
  };
  return [...visit('ComponentToken')].sort(([left], [right]) => left.localeCompare(right)).map(([, token]) => token);
}

export function extractExcludedComponentTokenNames(source) {
  const interfaces = parseInterfaces(source);
  const excluded = new Set();
  const visit = (name, visiting = new Set()) => {
    if (visiting.has(name)) return;
    const definition = interfaces.get(name);
    if (!definition) return;
    const nextVisiting = new Set(visiting).add(name);
    for (const parent of definition.parents) {
      parent.excluded.forEach(token => excluded.add(token));
      visit(parent.name, nextVisiting);
    }
  };
  visit('ComponentToken');
  return excluded;
}

function parseInterfaces(source) {
  const interfaces = new Map();
  for (const match of source.matchAll(/interface\s+([A-Za-z_$][\w$]*)(?:\s+extends\s+([^\{]+))?\s*\{([\s\S]*?)\}/g)) {
    interfaces.set(match[1], {
      parents: splitTopLevel(match[2] ?? '').map(parseParentType).filter(Boolean),
      body: match[3],
    });
  }
  return interfaces;
}

function splitTopLevel(value) {
  const parts = [];
  let depth = 0;
  let start = 0;
  for (let index = 0; index < value.length; index += 1) {
    if (value[index] === '<') depth += 1;
    else if (value[index] === '>') depth = Math.max(0, depth - 1);
    else if (value[index] === ',' && depth === 0) {
      parts.push(value.slice(start, index).trim());
      start = index + 1;
    }
  }
  parts.push(value.slice(start).trim());
  return parts.filter(Boolean);
}

function parseParentType(expression) {
  const source = expression.trim();
  const utility = source.match(/^(?:Exclude|Omit)\s*<([\s\S]*)>$/);
  if (utility) {
    const [base, ...excludedParts] = splitTopLevel(utility[1]);
    const name = base?.match(/^([A-Za-z_$][\w$]*)/)?.[1];
    const excluded = excludedParts.flatMap(part => [...part.matchAll(/['"]([^'"]+)['"]/g)].map(match => match[1]));
    return name ? { name, excluded } : null;
  }
  const name = source.match(/^([A-Za-z_$][\w$]*)/)?.[1];
  return name ? { name, excluded: [] } : null;
}
