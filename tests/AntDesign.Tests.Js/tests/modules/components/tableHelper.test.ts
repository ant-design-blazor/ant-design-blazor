import { tableHelper } from '../../../../../components/core/JsInterop/modules/components/tableHelper';
import { expect } from 'chai';
import * as domInit from '../../domInit';

describe('tableHelper.scrollTo', () => {
  function createScrollableTable() {
    const holder = document.createElement('div');
    holder.style.overflowY = 'auto';
    Object.defineProperties(holder, {
      clientHeight: { configurable: true, value: 100 },
      scrollHeight: { configurable: true, value: 800 },
      scrollTop: { configurable: true, writable: true, value: 40 }
    });
    holder.getBoundingClientRect = () => ({ top: 100, bottom: 200 } as DOMRect);
    const row = document.createElement('div');
    row.setAttribute('data-row-key', 'row-1');
    row.getBoundingClientRect = () => ({ top: 220, bottom: 250, height: 30 } as DOMRect);
    holder.appendChild(row);
    const calls: number[] = [];
    (holder as any).scrollTo = ({ top }: ScrollToOptions) => calls.push(top ?? 0);
    // Referencing the helper keeps its jsdom global initialization in this isolated test file.
    domInit.Guid.newGuid();
    document.body.appendChild(holder);
    return { holder, calls };
  }

  it('aligns a keyed row at the requested viewport offset', () => {
    const { holder, calls } = createScrollableTable();

    try {
      tableHelper.scrollTo(holder, 'row-1', undefined, undefined, 10, 'nearest', false, 55);
      expect(calls).to.deep.equal([150]);
    } finally {
      holder.remove();
    }
  });

  it('uses the virtual row index when the target is outside the rendered window', () => {
    const { holder, calls } = createScrollableTable();
    holder.replaceChildren();

    try {
      tableHelper.scrollTo(holder, undefined, 20, undefined, undefined, 'start', true, 55);
      expect(calls).to.deep.equal([1100]);
    } finally {
      holder.remove();
    }
  });
});
