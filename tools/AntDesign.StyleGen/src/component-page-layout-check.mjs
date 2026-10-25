import { readFileSync } from 'node:fs';
import { resolve } from 'node:path';
import { pathToFileURL } from 'node:url';

/** Shared docs-shell invariants. Collect rendered dimensions from a browser. */
export function checkComponentPageLayout(observations) {
  if (!Array.isArray(observations) || !observations.length) throw new Error('Browser observations required');
  const checks = observations.map(o => {
    const errors = [];
    if (!Number.isFinite(o.width) || o.width !== o.requestedWidth) errors.push('Wrong viewport');
    if (!Number.isFinite(o.scrollWidth) || !Number.isFinite(o.clientWidth) || o.scrollWidth > o.clientWidth + 1) errors.push('Document overflows');
    if (!o.main || !Number.isFinite(o.main.width) || o.main.width <= 0) errors.push('Missing main content');
    else {
      if (o.main.x < -1 || o.main.right > o.clientWidth + 1) errors.push('Main content escapes viewport');
      if (o.width < 768 && (o.main.paddingLeft > 16 || o.main.paddingRight > 16)) errors.push('Mobile retains desktop padding');
      if (o.width >= 768 && o.sidebar?.visible && Math.abs((o.main.columnTop ?? o.main.top) - o.sidebar.top) > 2) errors.push('Main content wrapped below sidebar');
    }
    if (!o.demoCount) errors.push('No demos rendered');
    if (o.errors?.length) errors.push('Demo render errors');
    for (const box of o.boxes ?? []) if (box.right > o.main.right + 1 || box.x < o.main.x - 1) errors.push(`Demo escapes content: ${box.id}`);
    return { width: o.width, route: o.route, passed: !errors.length, errors };
  });
  return { scope: 'component-page-layout-only', passed: checks.every(x => x.passed), checks };
}

if (process.argv[1] && import.meta.url === pathToFileURL(resolve(process.argv[1])).href) {
  const report = checkComponentPageLayout(JSON.parse(readFileSync(process.argv[2], 'utf8')));
  console.log(JSON.stringify(report, null, 2));
  if (!report.passed) process.exitCode = 1;
}
