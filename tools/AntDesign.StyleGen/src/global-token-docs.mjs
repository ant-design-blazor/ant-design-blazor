import { readFile, writeFile } from 'node:fs/promises';

const BEGIN = '<!-- BEGIN GENERATED GLOBAL TOKEN REFERENCE -->';
const END = '<!-- END GENERATED GLOBAL TOKEN REFERENCE -->';

function kebabCase(value) {
  return String(value)
    .replace(/([a-z0-9])([A-Z])/g, '$1-$2')
    .replace(/([A-Z])([A-Z][a-z])/g, '$1-$2')
    .toLowerCase();
}

function escapeMarkdown(value) {
  return String(value ?? '').replace(/\|/g, '\\|').replace(/[\r\n]+/g, ' ').trim();
}

export function renderGlobalTokenReference(metadata, language) {
  if (!metadata || typeof metadata !== 'object' || !metadata.global || typeof metadata.global !== 'object') {
    throw new Error('React token metadata must contain a global token map');
  }
  const chinese = language.toLowerCase().startsWith('zh');
  const groups = new Map();
  for (const [name, token] of Object.entries(metadata.global)) {
    const source = token.source || 'other';
    (groups.get(source) ?? groups.set(source, []).get(source)).push({ ...token, name });
  }

  const sourceNames = chinese
    ? { seed: 'Seed（基础）', map: 'Map（派生）', alias: 'Alias（语义别名）', other: '其他' }
    : { seed: 'Seed (foundation)', map: 'Map (derived)', alias: 'Alias (semantic aliases)', other: 'Other' };
  const lines = [
    BEGIN,
    `<h2 id="global-token-reference">${chinese ? '全局 Token 参考' : 'Global Token Reference'}</h2>`,
    chinese
      ? `此表由 Ant Design React v6 的 \`components/version/token-meta.json\` 生成，共 ${Object.keys(metadata.global).length} 项。CSS 变量名按 Blazor v6 的变量格式展示。`
      : `Generated from Ant Design React v6 \`components/version/token-meta.json\`; ${Object.keys(metadata.global).length} tokens in total. CSS custom property names follow the Blazor v6 variable format.`,
  ];
  for (const source of ['seed', 'map', 'alias', ...groups.keys()].filter((value, index, all) => all.indexOf(value) === index)) {
    const tokens = groups.get(source);
    if (!tokens?.length) continue;
    lines.push('', `### ${sourceNames[source] ?? source}`, '', '| Token | ' + (chinese ? '说明' : 'Description') + ' | CSS ' + (chinese ? '变量' : 'variable') + ' | ' + (chinese ? '类型' : 'Type') + ' |', '| --- | --- | --- | --- |');
    for (const token of tokens.sort((left, right) => left.name.localeCompare(right.name))) {
      const description = chinese ? token.desc || token.name : token.descEn || token.nameEn || token.name;
      lines.push(`| \`${token.name}\` | ${escapeMarkdown(description)} | \`--ant-${kebabCase(token.name)}\` | \`${escapeMarkdown(token.type || 'unknown')}\` |`);
    }
  }
  lines.push(END);
  return lines.join('\n');
}

export async function syncGlobalTokenDocs({ metadataPath, englishPath, chinesePath }) {
  const metadata = JSON.parse(await readFile(metadataPath, 'utf8'));
  for (const [path, language] of [[englishPath, 'en-US'], [chinesePath, 'zh-CN']]) {
    const source = await readFile(path, 'utf8');
    const start = source.indexOf(BEGIN);
    const end = source.indexOf(END);
    const generated = renderGlobalTokenReference(metadata, language);
    const next = start >= 0 && end >= start
      ? `${source.slice(0, start)}${generated}${source.slice(end + END.length)}`
      : `${source.trimEnd()}\n\n${generated}\n`;
    await writeFile(path, next, 'utf8');
  }
  return Object.keys(metadata.global).length;
}
