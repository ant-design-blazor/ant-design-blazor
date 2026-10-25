import { createRequire } from 'node:module';
import { writeFile } from 'node:fs/promises';
import { resolve } from 'node:path';

// Build fixtures with the actual packages resolved by the pinned React checkout.
const [reactRoot, output] = process.argv.slice(2);
if (!reactRoot || !output) throw new Error('Usage: node color-palette-reference.mjs <react-root> <output.json>');
const require = createRequire(resolve(reactRoot, 'package.json'));
const { generate, presetPrimaryColors } = require('@ant-design/colors');
const { FastColor } = require('@ant-design/fast-color');
const named = require('@ant-design/fast-color/lib/presetColors').default;
const inputs = [...Object.values(presetPrimaryColors), ...Object.keys(named),
  '#000', '#fff', '#abc', '#1677ff80', '#1234', 'rgb(22,119,255)',
  'rgb(10% 30% 90% / 50%)', 'rgba(22, 119, 255, .25)',
  'hsl(210.5deg 50% 40%)', 'hsla(0, 0%, 25%, .8)',
  'hsv(359.75, 70%, 90%)', 'hsb(60.2 80% 55% / 20%)', '', 'not-a-color'];
for (let i = 0; i < 128; i++) inputs.push('#' + ((i * 1299721 + 407) & 0xffffff).toString(16).padStart(6, '0'));
const cases = [...new Set(inputs)].flatMap(color => [
  { color, dark: false, expected: generate(color) },
  { color, dark: true, expected: generate(color, { theme: 'dark' }) },
  { color, dark: true, backgroundColor: '#30243b', expected: generate(color, { theme: 'dark', backgroundColor: '#30243b' }) },
]);
await writeFile(output, JSON.stringify({
  upstream: { colors: require('@ant-design/colors/package.json').version, fastColor: require('@ant-design/fast-color/package.json').version }, cases,
  operations: [...new Set(inputs)].map(color => {
    const c = new FastColor(color);
    return { color, alpha: c.setA(.35).toRgbString(), lighten: c.lighten(19).toHexString(),
      darken: c.darken(15).toHexString(), mix: c.mix('#1677ff80', 37).toHexString() };
  }),
}, null, 2) + '\n');
console.log(`Recorded ${cases.length} full palettes (${cases.length * 10} swatches)`);
