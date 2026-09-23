import test from 'node:test';
import assert from 'node:assert/strict';
import { compareMarkup, readReactSnapshot } from '../src/html-structure-compare.mjs';

test('compares React and Blazor markup as the same ordered DOM tree', () => {
  const result = compareMarkup(
    '<button class="ant-btn" type="button"><span class="ant-btn-content">Save</span></button>',
    '<button type="button" class="ant-btn" id="blazor-id"><span class="ant-btn-content">Save</span></button>',
  );
  assert.equal(result.equal, true);
});

test('reports the exact path and field for a structural mismatch', () => {
  const result = compareMarkup(
    '<button class="ant-btn"><span class="ant-btn-content">Save</span></button>',
    '<button class="ant-btn"><div class="ant-btn-content">Save</div></button>',
  );
  assert.equal(result.equal, false);
  assert.deepEqual(result.differences[0], { path: 'root/0/0', field: 'tag', expected: 'span', actual: 'div' });
});

test('extracts a React Jest snapshot by its exact test key', () => {
  const snapshot = 'exports[`Button renders basic 1`] = `\n<button>\n  Save\n</button>\n`;';
  assert.equal(readReactSnapshot(snapshot, 'Button renders basic 1'), '<button>\n  Save\n</button>');
});
