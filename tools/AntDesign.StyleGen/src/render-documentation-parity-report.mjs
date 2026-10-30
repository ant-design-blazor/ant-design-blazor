#!/usr/bin/env node

import { readFile, writeFile } from 'node:fs/promises';
import { resolve } from 'node:path';

const [, , inputArg, outputArg, interactionArg, manifestArg] = process.argv;
if (!inputArg || !outputArg) {
  console.error('Usage: node render-documentation-parity-report.mjs <inventory.json> <report.md> [interaction-report.json] [interaction-manifest.json]');
  process.exit(2);
}

const inputPath = resolve(inputArg);
const outputPath = resolve(outputArg);
const report = JSON.parse(await readFile(inputPath, 'utf8'));
const interactionReport = interactionArg ? JSON.parse(await readFile(resolve(interactionArg), 'utf8')) : null;
const interactionManifest = manifestArg ? JSON.parse(await readFile(resolve(manifestArg), 'utf8')) : null;
const { summary, components } = report;
const lines = [
  '# React / Blazor 文档运行时对齐扫描',
  '',
  `扫描时间：${report.generatedAt}`,
  '',
  '## 范围与判定',
  '',
  `- React: ${report.bases.react}；Blazor: ${report.bases.blazor}。React demo 名称作为缺漏基准，Blazor 独有 demo 不抵扣缺漏。`,
  '- 静态扫描在 viewport 1280×900、light theme 下逐组件进入文档页，滚动触发延迟加载，采集 demo 当前可见的 Ant Design 节点 HTML、选定 computed CSS 属性与几何位置；display:none、visibility:hidden 等关闭态节点不参与可见样式比较，另记隐藏节点数。',
  '- HTML 快照不一致仅标记为候选；HTML 中可能包含运行时包装、语义差异或无视觉影响的属性。CSS/几何差异按同名 Ant 类节点匹配，1px 内视为相同。',
  '- 下方“交互状态”来自 DOM/style manifests 的实际浏览器操作；未覆盖的 demo 不能据此判为交互样式已对齐。',
  '',
  '## 汇总',
  '',
  '| 指标 | 数量 |',
  '|---|---:|',
  `| React / Blazor 组件路由 | ${summary.reactRoutes} / ${summary.blazorRoutes} |`,
  `| 可比组件 | ${summary.matchedComponents} |`,
  `| React / Blazor demo | ${summary.reactDemos} / ${summary.blazorDemos} |`,
  `| 名称匹配 demo | ${summary.matchedDemos} |`,
  `| React 有、Blazor 缺失 | ${summary.missingReactDemos} |`,
  `| 仅 Blazor 有（不计缺漏） | ${summary.extraBlazorDemos} |`,
  `| HTML 首屏快照候选差异 | ${summary.runtimeHtmlDifferences} / ${summary.matchedDemos} |`,
  `| computed CSS 候选差异 | ${summary.runtimeStyleDifferences} / ${summary.matchedDemos} |`,
  `| 几何候选差异（>1px） | ${summary.runtimeGeometryDifferences} / ${summary.matchedDemos} |`,
  `| 页面采集阻塞 | ${summary.blockedPages} |`,
  '',
  '## 组件路由差异',
  '',
];

if (summary.missingBlazorRoutes.length) {
  lines.push(`- React 有、Blazor 缺失：${summary.missingBlazorRoutes.map((route) => `\`${route.name}\` (${route.path})`).join(', ')}。`);
} else {
  lines.push('- 没有缺失的组件路由。');
}
for (const component of report.missingReactRouteDemos ?? []) {
  const demos = component.demos.map((name) => `\`${name}\``).join(', ');
  const unavailable = component.blockers?.length ? `；采集信息：${component.blockers.join('; ')}` : '';
  lines.push(`- **${component.name}** 整个组件路由缺失；React 文档 demo：${demos || '当前无法采集'}${unavailable}。`);
}
if (summary.extraBlazorRoutes.length) {
  lines.push(`- Blazor 独有：${summary.extraBlazorRoutes.map((route) => `\`${route.name}\` (${route.path})`).join(', ')}。`);
}
if (summary.excludedReactRoutes.length) {
  lines.push(`- 按扫描配置排除：${summary.excludedReactRoutes.map((route) => `\`${route.name}\``).join(', ')}。`);
}

lines.push(
  '',
  '## React 有但 Blazor 缺失的 demo',
  '',
  '仅按 React demo 名称计数；Blazor 额外 demo 不列为缺漏。',
  '',
  '| 组件 | 缺失 React demo |',
  '|---|---|',
);

for (const component of components) {
  const missing = component.demos.missingReactNames;
  if (missing.length) lines.push(`| ${component.name} | ${missing.map((name) => `\`${name}\``).join(', ')} |`);
}

lines.push('', '## 首屏 HTML 快照候选差异', '', '以下名称的采集快照不完全相同，尚未人工确认每项都是实际缺陷。', '', '| 组件 | 候选 demo |', '|---|---|');
for (const component of components) {
  const different = component.demos.matchedDemos
    .filter((demo) => !demo.runtime.html.equal)
    .map((demo) => demo.react.name);
  if (different.length) lines.push(`| ${component.name} | ${different.map((name) => `\`${name}\``).join(', ')} |`);
}

lines.push('', '## 首屏 CSS 与几何候选差异', '', '| 组件 | CSS 属性候选差异 | 几何候选差异（>1px） |', '|---|---|---|');
for (const component of components) {
  const styles = component.demos.matchedDemos
    .filter((demo) => !demo.runtime.computedStyles.equal)
    .map((demo) => demo.react.name);
  const geometry = component.demos.matchedDemos
    .filter((demo) => !demo.runtime.geometry.equal)
    .map((demo) => demo.react.name);
  if (styles.length || geometry.length) {
    lines.push(`| ${component.name} | ${styles.map((name) => `\`${name}\``).join(', ') || '—'} | ${geometry.map((name) => `\`${name}\``).join(', ') || '—'} |`);
  }
}

lines.push('', '## 页面错误或采集阻塞', '', '| 组件 | React 页面 | Blazor 页面 |', '|---|---|---|');
for (const component of components) {
  if (component.blockers.react.length || component.blockers.blazor.length) {
    lines.push(`| ${component.name} | ${component.blockers.react.join('; ') || '—'} | ${component.blockers.blazor.join('; ') || '—'} |`);
  }
}

lines.push(
  '',
  '## 交互状态验收覆盖',
  '',
  '运行 DOM/style manifests 中的场景，对点击、输入、悬停、弹层和语义节点进行 React / Blazor 对称操作后比较 DOM、computed style 与几何。该覆盖不是全站所有 demo 的交互穷举；功能型 demo 需按控件行为补充 scenario。',
  '',
);

if (interactionReport) {
  const result = interactionReport.summary;
  lines.push(
    `- 本次运行：${result.groups} 组、${result.scenarios} 个场景；DOM、CSS 与几何全部相同 ${result.passed}，至少一项有差异 ${result.differences}，阻塞 ${result.blocked}。`,
    `- 视觉样式通过 ${result.visualStylePassed} 个场景；存在视觉样式差异 ${result.visualStyleDifferences} 个。`,
    '',
    '| 组件 / 场景 | 视觉样式差异样本 |',
    '|---|---|',
  );
  for (const group of interactionReport.groups) {
    for (const scenario of group.scenarios) {
      if (scenario.comparison?.visualStyles?.equal !== false) continue;
      const comparison = scenario.comparison;
      const samples = [];
      for (const difference of comparison.computedStyles?.differences ?? []) {
        samples.push(`${difference.path}.${difference.property}: ${difference.expected} → ${difference.actual}`);
      }
      for (const semantic of comparison.semantic ?? []) {
        for (const difference of semantic.computedStyles?.differences ?? []) {
          samples.push(`${semantic.path}/${difference.path}.${difference.property}: ${difference.expected} → ${difference.actual}`);
        }
      }
      for (const difference of comparison.visualStyles?.semanticDifferences ?? []) {
        const value = difference.expected !== undefined || difference.actual !== undefined
          ? `${difference.expected} → ${difference.actual}`
          : `${difference.difference}`;
        samples.push(`${difference.path}[${difference.index}].${difference.field ?? difference.property}: ${value}`);
      }
      lines.push(`| ${group.component} / ${scenario.id} | ${samples.slice(0, 5).map((sample) => `\`${sample}\``).join('<br>') || '视觉比较器判为不同，未返回属性样本'} |`);
    }
  }
  lines.push('');

  if (interactionManifest) {
    const covered = new Map();
    const normalize = (value = '') => value.replace(/^components-/i, '').replace(/[^a-z0-9]/gi, '').toLowerCase();
    const componentByRoute = new Map(components.map((component) => [normalize(component.reactRoute.split('/').filter(Boolean).at(-1)), component]));
    for (const group of interactionManifest.groups) {
      const scope = group.react?.scopeSelector ?? '';
      const demoId = scope.match(/#([^\s]+-demo-([^\s]+))/)?.[2];
      if (!demoId) continue;
      const fullId = scope.match(/#([^\s]+)/)?.[1] ?? '';
      const routeSlug = normalize(fullId.split('-demo-')[0]);
      const component = componentByRoute.get(routeSlug);
      if (!component) continue;
      const demoName = normalize(demoId);
      if (!component.demos.matchedDemos.some((demo) => normalize(demo.react.name) === demoName)) continue;
      if (!covered.has(component.name)) covered.set(component.name, new Set());
      covered.get(component.name).add(demoName);
    }
    const coveredCount = [...covered.values()].reduce((total, names) => total + names.size, 0);
    lines.push(
      `- 按 manifest 中能关联到 React demo 锚点的场景，交互后状态覆盖 ${coveredCount}/${summary.matchedDemos} 个同名 demo；其余 ${summary.matchedDemos - coveredCount} 个仍未完成逐 demo 的功能状态比对。以 body 为 scope 的弹层场景没有 demo 锚点，不计入此数。`,
      '',
      '| 组件 | 已有交互状态覆盖 | 尚未覆盖的同名 demo |',
      '|---|---|---|',
    );
    for (const component of components) {
      const matched = component.demos.matchedDemos.map((demo) => demo.react.name);
      if (!matched.length) continue;
      const names = covered.get(component.name) ?? new Set();
      const done = matched.filter((name) => names.has(normalize(name)));
      const pending = matched.filter((name) => !names.has(normalize(name)));
      lines.push(`| ${component.name} | ${done.map((name) => `\`${name}\``).join(', ') || '—'} | ${pending.map((name) => `\`${name}\``).join(', ') || '—'} |`);
    }
    lines.push('');
  }
}

lines.push(`首屏机器报告：\`${inputArg}\`。`);
if (interactionArg) lines.push(`交互机器报告：\`${interactionArg}\`.`);
if (manifestArg) lines.push(`交互场景清单：\`${manifestArg}\`.`);
lines.push('');

await writeFile(outputPath, lines.join('\n'), 'utf8');
console.log(outputPath);
