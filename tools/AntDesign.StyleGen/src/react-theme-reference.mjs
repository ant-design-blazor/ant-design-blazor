#!/usr/bin/env node
// Execute the pinned upstream implementation, rather than reimplementing it in
// the verifier. The generated fixture is consumed by the C# runtime tests.
import { execFileSync } from 'node:child_process';
import { createRequire } from 'node:module';
import { mkdir, writeFile } from 'node:fs/promises';
import { dirname, join, resolve } from 'node:path';
import { pathToFileURL } from 'node:url';

export async function generateReactThemeReference(upstream, output) {
  const root = resolve(upstream);
  const revision = execFileSync('git', ['rev-parse', 'HEAD'], { cwd: root, encoding: 'utf8' }).trim();
  const custom = {
    colorPrimary: '#722ed1', colorInfo: '#13c2c2', colorBgBase: '#f4f6fa',
    colorTextBase: '#182230', colorShadow: 'rgba(20, 40, 60, 0.4)',
    fontSize: 16, lineWidth: 2, borderRadius: 10, sizeUnit: 6, sizeStep: 5, controlHeight: 40,
  };
  const scenarios = ['default', 'dark', 'compact'].flatMap(algorithm => [
    { id: algorithm, algorithm, token: {} },
    { id: `${algorithm}-custom`, algorithm, token: custom },
    { id: `${algorithm}-accessible`, algorithm, token: { motion: false, focusOutline: false, borderRadius: 0 } },
    { id: `${algorithm}-overrides`, algorithm, token: {
      colorText: '#123456', colorBgContainer: '#f5f6f7', colorBorderSecondary: '#ddeeff',
      colorPrimaryBg: '#e4dcfa', fontSizeSM: 13, lineHeight: 2, sizeSM: 22,
      fontWeightStrong: 700, controlOutlineWidth: 5, boxShadow: '0 0 2px red',
    } },
  ]);
  const imports = [
    `import getDesignToken from ${JSON.stringify(join(root, 'components/theme/getDesignToken.ts'))};`,
    ...['default', 'dark', 'compact'].map(name => `import ${name}Algorithm from ${JSON.stringify(join(root, `components/theme/themes/${name}/index.ts`))};`),
  ].join('\n');
  const source = `${imports}\nconst algorithms={default:defaultAlgorithm,dark:darkAlgorithm,compact:compactAlgorithm};\nconsole.log(JSON.stringify(${JSON.stringify(scenarios)}.map(s=>({...s,expected:getDesignToken({algorithm:algorithms[s.algorithm],token:s.token})}))));`;
  const tsx = createRequire(join(root, 'package.json')).resolve('tsx/cli');
  const values = JSON.parse(execFileSync(process.execPath, [tsx, '-e', source], { cwd: root, encoding: 'utf8', maxBuffer: 8 * 1024 * 1024 }));
  const report = { schemaVersion: 1, source: { repository: 'ant-design/ant-design', revision, entry: 'components/theme/getDesignToken.ts' }, scenarios: values };
  if (output) {
    await mkdir(dirname(resolve(output)), { recursive: true });
    await writeFile(output, `${JSON.stringify(report, null, 2)}\n`);
  }
  return report;
}

if (process.argv[1] && import.meta.url === pathToFileURL(resolve(process.argv[1])).href) {
  const args = Object.fromEntries(process.argv.slice(2).reduce((pairs, value, index, all) => value.startsWith('--') ? [...pairs, [value.slice(2), all[index + 1]]] : pairs, []));
  if (!args['antd-path'] || !args.output) throw new Error('Pass --antd-path and --output');
  const result = await generateReactThemeReference(args['antd-path'], args.output);
  console.log(JSON.stringify({ source: result.source, scenarios: result.scenarios.map(s => ({ id: s.id, tokens: Object.keys(s.expected).length })) }, null, 2));
}
