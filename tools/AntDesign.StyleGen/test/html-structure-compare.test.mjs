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

test('parses isolated table row fragments in a table context', () => {
  const result = compareMarkup(
    '<tr><td>32</td><td>New York No. 1 Lake Park</td></tr>',
    '<tr><td>32        </td><td>\n    New York No. 1 Lake Park    </td></tr>',
  );

  assert.equal(result.equal, true);
  assert.deepEqual(result.expected[0].children.map(({ tag }) => tag), ['td', 'td']);
  assert.deepEqual(result.actual[0].children.map(({ tag }) => tag), ['td', 'td']);
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
