import assert from 'node:assert/strict';
import test from 'node:test';
import { checkHomepageLayout } from '../src/homepage-layout-check.mjs';

const snapshot = () => ({
  requestedWidth: 1024, width: 1024, clientWidth: 1013, scrollWidth: 1013,
  header: { clientWidth: 1013, scrollWidth: 1013 }, navigationVisible: true,
  sections: ['preview', 'rich', 'framework'].map(name => ({ name, x: 0, right: 1013 })),
  actions: [{ x: 300, right: 450 }, { x: 466, right: 620 }],
  crowded: { top: 0, navTop: 0 },
});

test('uses actual document client width, not innerWidth or matching component styles, to gate overflow', () => {
  const observation = snapshot();
  observation.scrollWidth = 1098;
  observation.header.scrollWidth = 1098;
  const report = checkHomepageLayout([observation]);
  assert.equal(report.passed, false);
  assert.match(report.checks[0].errors.join('\n'), /Document overflows by 85px/);
  assert.match(report.checks[0].errors.join('\n'), /Header overflows by 85px/);
});

test('accepts a real vertical scrollbar and internal scrolling without requiring page overflow', () => {
  const observation = snapshot();
  observation.swatches = { clientWidth: 379, scrollWidth: 412 };
  assert.equal(checkHomepageLayout([observation]).passed, true);
});

test('rejects clipped navigation even when document overflow is zero', () => {
  const observation = snapshot();
  observation.navigationClipped = true;
  const report = checkHomepageLayout([observation]);
  assert.equal(report.passed, false);
  assert.match(report.checks[0].errors.join('\n'), /navigation items are clipped/);
});

test('rejects missing navigation, wrapped crowded actions, clipped hero actions and vw bleed', () => {
  const observation = snapshot();
  observation.navigationVisible = false;
  observation.crowded.top = 64;
  observation.actions[1].right = 1020;
  observation.sections[0] = { name: 'preview', x: -5.5, right: 1018.5 };
  const errors = checkHomepageLayout([observation]).checks[0].errors.join('\n');
  assert.match(errors, /No visible header navigation/);
  assert.match(errors, /second row/);
  assert.match(errors, /Hero action is clipped/);
  assert.match(errors, /Section escapes/);
});

test('does not certify incomplete observations or a viewport override applied to the wrong tab', () => {
  const observation = snapshot();
  observation.width = 672;
  delete observation.clientWidth;
  assert.equal(checkHomepageLayout([observation]).passed, false);
  assert.throws(() => checkHomepageLayout([]), /observation/);
  assert.throws(() => checkHomepageLayout([snapshot()], NaN), /tolerance/);
});
