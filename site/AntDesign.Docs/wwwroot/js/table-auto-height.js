const observers = new WeakMap();
const initialColumnWidths = new WeakMap();

function observeAutoHeight(container) {
  const wrapper = container.querySelector('.ant-table-wrapper');
  if (!wrapper) return;

  const measure = () => {
    const section = wrapper.querySelector('.ant-table-container');
    const header = wrapper.querySelector('.ant-table-header');
    const body = wrapper.querySelector('.ant-table-body');
    const pagination = wrapper.querySelector('.ant-pagination');
    if (!section || !header || !body) return;

    const getHeight = element => {
      const styles = getComputedStyle(element);
      const marginTop = Number.parseFloat(styles.marginTop) || 0;
      const marginBottom = Number.parseFloat(styles.marginBottom) || 0;
      return element.getBoundingClientRect().height + marginTop + marginBottom;
    };

    const totalHeight = getHeight(wrapper);
    const headerHeight = getHeight(header);
    const paginationHeight = pagination ? getHeight(pagination) : 0;
    const scrollY = Math.max(0, Math.floor(totalHeight - headerHeight - paginationHeight));
    const sectionHeight = totalHeight - paginationHeight;
    const sectionHeightValue = `${sectionHeight}px`;
    const scrollYValue = `${scrollY}px`;

    if (section.style.height !== sectionHeightValue) section.style.height = sectionHeightValue;
    if (body.style.maxHeight !== scrollYValue) body.style.maxHeight = scrollYValue;
    body.style.removeProperty('height');

    const table = header.querySelector('table');
    if (!table) return;

    const tableWidth = wrapper.getBoundingClientRect().width;
    if (!tableWidth) return;
    const columns = [...header.querySelectorAll('colgroup col')];
    columns.forEach(column => {
      if (!initialColumnWidths.has(column)) {
        initialColumnWidths.set(column, column.style.width);
      }
      const rawWidth = initialColumnWidths.get(column).trim();
      if (!rawWidth.endsWith('%')) return;
      const percentage = Number.parseFloat(rawWidth);
      if (!Number.isFinite(percentage)) return;
      const width = `${Math.round(tableWidth * percentage / 100 * 1000) / 1000}px`;
      if (column.style.width !== width) column.style.width = width;
    });
  };

  const resizeObserver = new ResizeObserver(measure);
  resizeObserver.observe(wrapper);
  const mutationObserver = new MutationObserver(measure);
  mutationObserver.observe(wrapper, { childList: true, subtree: true });
  observers.set(container, { resizeObserver, mutationObserver });
  measure();
  requestAnimationFrame(measure);
}

function unobserveAutoHeight(container) {
  const observer = observers.get(container);
  observer?.resizeObserver.disconnect();
  observer?.mutationObserver.disconnect();
  observers.delete(container);
}

export { observeAutoHeight, unobserveAutoHeight };
