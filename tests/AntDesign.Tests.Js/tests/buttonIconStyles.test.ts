import { expect } from 'chai';
import { readFileSync } from 'fs';
import { JSDOM } from 'jsdom';
import * as less from 'less';
import { resolve } from 'path';

// Path data from the built-in outline icons.
const iconPaths = {
  plus: [
    'M474 152m8 0l60 0q8 0 8 8l0 704q0 8-8 8l-60 0q-8 0-8-8l0-704q0-8 8-8Z',
    'M168 474m8 0l672 0q8 0 8 8l0 60q0 8-8 8l-672 0q-8 0-8-8l0-60q0-8 8-8Z'
  ],
  minus: [
    'M872 474H152c-4.4 0-8 3.6-8 8v60c0 4.4 3.6 8 8 8h720c4.4 0 8-3.6 8-8v-60c0-4.4-3.6-8-8-8z'
  ]
};
const icons = Object.keys(iconPaths);

function getPathShapeRendering(path: SVGPathElement): string {
  const { defaultView } = path.ownerDocument;

  // JSDOM does not compute inheritance for shape-rendering.
  for (let element: Element = path; element; element = element.parentElement) {
    const value = defaultView.getComputedStyle(element).getPropertyValue('shape-rendering').trim().toLowerCase();
    if (value && value !== 'inherit' && value !== 'unset') {
      return value === 'initial' ? 'auto' : value;
    }
  }

  return 'auto';
}

describe('Button icon styles', function () {
  this.timeout(15000);

  for (const theme of ['default', 'compact']) {
    describe(theme, () => {
      let dom: JSDOM;

      before(async () => {
        const suffix = theme === 'compact' ? '.compact' : '';
        const filename = resolve(__dirname, '../../../components', `ant-design-blazor${suffix}.less`);
        const { css } = await less.render(readFileSync(filename, 'utf8'), {
          filename,
          javascriptEnabled: true
        });

        const buttons = icons.map(icon => `
          <button class="ant-btn ant-btn-sm ant-btn-icon-only">
            <span class="anticon anticon-${icon}">
              <svg viewBox="0 0 1024 1024" width="1em" height="1em" fill="currentColor">
                ${iconPaths[icon].map(data => `<path d="${data}" />`).join('')}
              </svg>
            </span>
          </button>`).join('');
        dom = new JSDOM(`<!doctype html><style>${css}</style>${buttons}`);
      });

      after(() => dom?.window.close());

      for (const icon of icons) {
        it(`allows antialiasing on the ${icon} icon paths`, () => {
          const paths: NodeListOf<SVGPathElement> =
            dom.window.document.querySelectorAll(`.anticon-${icon} > svg > path`);
          expect(paths.length).to.be.greaterThan(0);

          // Without antialiasing, a stroke narrower than one device pixel can disappear.
          paths.forEach(path => expect(['auto', 'geometricprecision']).to.include(getPathShapeRendering(path)));
        });
      }
    });
  }
});
