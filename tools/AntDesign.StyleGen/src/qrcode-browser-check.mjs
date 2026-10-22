#!/usr/bin/env node
import { createRequire } from 'node:module';
import { mkdir, mkdtemp, readFile, readdir, writeFile } from 'node:fs/promises';
import { basename, join, resolve } from 'node:path';
import { compareReactDemoNames, scrollToLoadAllDemos, waitForSettledPage } from './documentation-parity-inventory.mjs';

const args = Object.fromEntries(process.argv.slice(2).reduce((pairs, value, index, values) =>
  value.startsWith('--') ? [...pairs, [value.slice(2), values[index + 1]]] : pairs, []));
if (!args['antd-path'] || !args.output || !args.chrome) throw new Error('Pass --antd-path, --output, and --chrome');
const upstream = resolve(args['antd-path']);
const output = resolve(args.output);
const require = createRequire(join(upstream, 'package.json'));
await mkdir(output, { recursive: true });
let browser;
const report = { checks: {}, downloads: [], errors: [] };
const check = (name, passed) => {
  report.checks[name] = Boolean(passed);
  if (!passed) throw new Error(`QRCode browser check failed: ${name}`);
};
try {
  browser = await require('puppeteer').launch({ executablePath: args.chrome, headless: true, args: ['--no-sandbox'] });
  const page = await browser.newPage();
  await page.setViewport({ width: 1280, height: 900, deviceScaleFactor: 1 });
  page.on('pageerror', error => report.errors.push(error.message));
  await page.goto(args.url ?? 'http://127.0.0.1:5540/en-US/components/qrcode', { waitUntil: 'domcontentloaded' });
  await page.waitForSelector('#components-qrcode-demo-base canvas');
  await page.waitForFunction(() => document.querySelector('#components-qrcode-demo-base canvas')?.width === 160 * devicePixelRatio);
  await waitForSettledPage(page);
  await scrollToLoadAllDemos(page);
  await waitForSettledPage(page);
  const demoNames = await page.$$eval('.code-box-demo', nodes => nodes.map(node => ({ name: node.closest('[id]').id.split('-demo-')[1] })));
  const reactNames = (await readdir(join(upstream, 'components/qr-code/demo'))).filter(name => name.endsWith('.md')).map(name => ({ name: name.slice(0, -3) }));
  const comparison = compareReactDemoNames(reactNames, demoNames);
  report.demos = { reactPublic: reactNames.length, blazor: demoNames.length, missing: comparison.missingReact };
  check('all-public-react-demos', comparison.missingReact.length === 0);

  const clickText = async (scope, selector, text) => {
    for (const handle of await page.$$(`${scope} ${selector}`)) {
      if ((await handle.evaluate(node => node.textContent.trim())) === text) { await handle.click(); return; }
    }
    throw new Error(`Missing ${text} in ${scope}`);
  };
  const sizeScope = '#components-qrcode-demo-customSize';
  await clickText(sizeScope, 'button', 'Larger');
  await page.waitForFunction(scope => document.querySelector(`${scope} .ant-qrcode`).getBoundingClientRect().width === 170, {}, sizeScope);
  check('interactive-size', true);

  const levelScope = '#components-qrcode-demo-errorlevel';
  const beforeLevel = await page.$eval(`${levelScope} canvas`, canvas => canvas.toDataURL());
  await clickText(levelScope, 'label', 'H');
  await page.waitForFunction(({ scope, before }) => document.querySelector(`${scope} canvas`).toDataURL() !== before, {}, { scope: levelScope, before: beforeLevel });
  check('interactive-error-level', true);

  const cdp = await browser.target().createCDPSession();
  const downloadDirectory = await mkdtemp(join(output, 'downloads-'));
  await cdp.send('Browser.setDownloadBehavior', { behavior: 'allow', downloadPath: downloadDirectory, eventsEnabled: true });
  const downloadScope = '#components-qrcode-demo-download';
  for (const type of ['canvas', 'svg']) {
    try {
    if (type === 'svg') await clickText(downloadScope, 'label', 'svg');
    await page.waitForSelector(`${downloadScope} .ant-qrcode > ${type}`);
    const result = new Promise((resolveDownload, reject) => {
      let guid;
      let fileName;
      const cleanup = () => { clearTimeout(timeout); cdp.off('Browser.downloadWillBegin', begin); cdp.off('Browser.downloadProgress', progress); };
      const begin = event => { guid = event.guid; fileName = basename(event.suggestedFilename); };
      const progress = event => {
        if (event.guid !== guid) return;
        if (event.state === 'completed') { cleanup(); resolveDownload(fileName); }
        if (event.state === 'canceled') { cleanup(); reject(new Error(`Canceled ${type} download`)); }
      };
      const timeout = setTimeout(() => { cleanup(); reject(new Error(`Timed out waiting for ${type} download`)); }, 15000);
      cdp.on('Browser.downloadWillBegin', begin);
      cdp.on('Browser.downloadProgress', progress);
    });
    await clickText(downloadScope, 'button', 'Download');
    const fileName = await result;
    const filePath = join(downloadDirectory, fileName);
    const content = await readFile(filePath);
    check(`${type}-download`, type === 'canvas' ? content.subarray(1, 4).toString() === 'PNG' : content.toString().includes('<svg'));
    report.downloads.push({ type, fileName, filePath, bytes: content.length });
    } catch (error) {
      report.checks[`${type}-download`] = false;
      report.downloads.push({ type, error: error.message });
      process.exitCode = 1;
    }
  }

  await page.locator('#components-qrcode-demo-Popover button').hover();
  await page.waitForSelector('.ant-popover .ant-qrcode-borderless', { visible: true });
  check('popover', true);
  check('semantic-cover-and-renderer', await page.$eval('.semantic-dom-preview .ant-qrcode', node => Boolean(node.querySelector('.ant-qrcode-cover') && node.querySelector('canvas'))));
  const rows = await page.$$eval('.global-token-table tbody tr', nodes => nodes.map(node => [...node.cells].map(cell => cell.textContent.trim())));
  check('global-token-rows', rows.length === 12 && rows.every(row => row[4] && row[4] !== '—'));
  report.globalTokens = rows;
  await page.$eval('#components-qrcode-demo-customSize', node => node.scrollIntoView({ block: 'center' }));
  await page.screenshot({ path: join(output, 'qrcode-browser-check.png') });
} catch (error) {
  report.failure = error.message;
  process.exitCode = 1;
} finally {
  await browser?.close();
  console.log(JSON.stringify(report, null, 2));
  await writeFile(join(output, 'browser-check.json'), `${JSON.stringify(report, null, 2)}\n`);
}
