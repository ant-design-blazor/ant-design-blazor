import { manipulationHelper } from '../dom/manipulationHelper'

export class tableHelper {

  static scrollTo(holderRef: HTMLElement, key: string | undefined, index: number | undefined, top: number | undefined,
    offset: number | undefined, align: string, isVirtual: boolean, itemHeight: number) {
    if (!holderRef) return;

    let scrollElement: HTMLElement | null = null;
    let ancestor: HTMLElement | null = holderRef;
    while (ancestor && ancestor !== document.body) {
      const style = getComputedStyle(ancestor);
      if (/(auto|scroll)/.test(style.overflowY)
        && ancestor.scrollHeight > ancestor.clientHeight) {
        scrollElement = ancestor;
        break;
      }
      ancestor = ancestor.parentElement;
    }

    const scrollTarget = (scrollElement || document.scrollingElement || document.documentElement) as HTMLElement;
    const isWindow = scrollTarget === document.scrollingElement || scrollTarget === document.documentElement;
    const currentTop = isWindow ? window.scrollY : scrollTarget.scrollTop;
    const viewportTop = isWindow ? 0 : scrollTarget.getBoundingClientRect().top + scrollTarget.clientTop;
    const viewportHeight = isWindow ? window.innerHeight : scrollTarget.clientHeight;
    const rows = Array.from(holderRef.querySelectorAll('[data-row-key]')) as HTMLElement[];

    if (top != null) {
      this.setVerticalScroll(scrollTarget, isWindow, top);
      return;
    }

    let row = key != null ? rows.find(candidate => candidate.getAttribute('data-row-key') === String(key)) : null;
    if (!row && index != null) {
      const startIndex = isVirtual ? (holderRef as any).virtualWindowBinding?.startIndex ?? 0 : 0;
      row = rows[index - startIndex] || null;
    }

    let targetTop: number;
    if (row) {
      const rowRect = row.getBoundingClientRect();
      const rowTop = rowRect.top - viewportTop;
      const rowBottom = rowRect.bottom - viewportTop;
      if (offset != null) {
        targetTop = currentTop + rowTop - offset;
      } else if (align === 'start') {
        targetTop = currentTop + rowTop;
      } else if (align === 'center') {
        targetTop = currentTop + rowTop - (viewportHeight - rowRect.height) / 2;
      } else if (align === 'end') {
        targetTop = currentTop + rowBottom - viewportHeight;
      } else if (rowTop < 0) {
        targetTop = currentTop + rowTop;
      } else if (rowBottom > viewportHeight) {
        targetTop = currentTop + rowBottom - viewportHeight;
      } else {
        return;
      }
    } else if (isVirtual && index != null) {
      const estimatedRowTop = Math.max(0, index * itemHeight);
      if (offset != null || align === 'start') targetTop = estimatedRowTop - (offset ?? 0);
      else if (align === 'center') targetTop = estimatedRowTop - (viewportHeight - itemHeight) / 2;
      else if (align === 'end') targetTop = estimatedRowTop + itemHeight - viewportHeight;
      else targetTop = estimatedRowTop + itemHeight - viewportHeight;
    } else {
      return;
    }

    this.setVerticalScroll(scrollTarget, isWindow, Math.max(0, targetTop));
  }

  private static setVerticalScroll(scrollTarget: HTMLElement, isWindow: boolean, top: number) {
    if (isWindow) window.scrollTo({ top, behavior: 'auto' });
    else scrollTarget.scrollTo({ top, behavior: 'auto' });
  }

  static bindVirtualWindow(holderRef, dotNetRef, itemCount, itemHeight, startIndex, endIndex) {
    if (!holderRef || !dotNetRef || !Number.isFinite(itemCount) || !Number.isFinite(itemHeight) || itemHeight <= 0) {
      return;
    }

    let binding = holderRef.virtualWindowBinding;
    if (!binding) {
      binding = {
        dotNetRef,
        itemCount,
        itemHeight,
        startIndex,
        endIndex,
        requestSequence: 0,
        pendingKey: null,
        scrollbarTimer: null,
        touchStart: null,
        updateWindow: null,
        onWheel: null,
        onTouchStart: null,
        onTouchMove: null,
        onTouchEnd: null,
        onScrollbarPointerDown: null,
        onScrollbarPointerMove: null,
        onScrollbarPointerUp: null,
        scrollbarDrag: null
      };

      binding.updateWindow = event => {
        tableHelper.updateVirtualScrollbars(holderRef, binding, Boolean(event));
        const visibleStart = Math.max(0, Math.floor(holderRef.scrollTop / binding.itemHeight));
        const visibleEnd = Math.min(binding.itemCount, Math.ceil((holderRef.scrollTop + holderRef.clientHeight) / binding.itemHeight));
        if (visibleStart >= binding.startIndex && visibleEnd <= binding.endIndex) {
          return;
        }

        const nextStart = visibleStart;
        const nextEnd = Math.min(binding.itemCount, Math.max(nextStart + 1, visibleEnd + 2));
        const key = `${nextStart}:${nextEnd}`;
        if (key === binding.pendingKey || (nextStart === binding.startIndex && nextEnd === binding.endIndex)) {
          return;
        }

        binding.pendingKey = key;
        const sequence = ++binding.requestSequence;
        binding.dotNetRef.invokeMethodAsync('OnVirtualWindowChanged', nextStart, nextEnd, sequence)
          .catch(() => {
            if (binding.pendingKey === key) binding.pendingKey = null;
          });
      };

      binding.onWheel = event => {
        const deltaX = event.shiftKey && !event.deltaX ? event.deltaY : event.deltaX;
        const nextLeft = holderRef.scrollLeft + deltaX;
        const nextTop = holderRef.scrollTop + event.deltaY;
        const canScrollX = nextLeft !== holderRef.scrollLeft;
        const canScrollY = nextTop !== holderRef.scrollTop;
        if (canScrollX) holderRef.scrollLeft = nextLeft;
        if (canScrollY) holderRef.scrollTop = nextTop;
        if (canScrollX || canScrollY) event.preventDefault();
      };

      binding.onTouchStart = event => {
        const touch = event.touches?.[0];
        if (touch) binding.touchStart = { x: touch.clientX, y: touch.clientY, left: holderRef.scrollLeft, top: holderRef.scrollTop };
      };
      binding.onTouchMove = event => {
        const touch = event.touches?.[0];
        if (!touch || !binding.touchStart) return;
        const nextLeft = binding.touchStart.left + binding.touchStart.x - touch.clientX;
        const nextTop = binding.touchStart.top + binding.touchStart.y - touch.clientY;
        const canScrollX = nextLeft !== holderRef.scrollLeft;
        const canScrollY = nextTop !== holderRef.scrollTop;
        if (canScrollX) holderRef.scrollLeft = nextLeft;
        if (canScrollY) holderRef.scrollTop = nextTop;
        if (canScrollX || canScrollY) event.preventDefault();
      };
      binding.onTouchEnd = () => { binding.touchStart = null; };

      binding.onScrollbarPointerDown = event => {
        const thumb = event.target?.closest('.ant-table-tbody-virtual-scrollbar-thumb');
        const track = thumb?.parentElement;
        if (!thumb || !track) return;
        const horizontal = track.classList.contains('ant-table-tbody-virtual-scrollbar-horizontal');
        const bounds = track.getBoundingClientRect();
        binding.scrollbarDrag = {
          horizontal,
          pointer: horizontal ? event.clientX : event.clientY,
          scroll: horizontal ? holderRef.scrollLeft : holderRef.scrollTop,
          trackSize: horizontal ? bounds.width : bounds.height,
          thumbSize: horizontal ? thumb.getBoundingClientRect().width : thumb.getBoundingClientRect().height,
          thumb
        };
        thumb.classList.add('ant-table-tbody-virtual-scrollbar-thumb-moving');
        document.addEventListener('pointermove', binding.onScrollbarPointerMove, { passive: true });
        document.addEventListener('pointerup', binding.onScrollbarPointerUp, { passive: true });
        document.addEventListener('pointercancel', binding.onScrollbarPointerUp, { passive: true });
        event.preventDefault();
      };
      binding.onScrollbarPointerMove = event => {
        const drag = binding.scrollbarDrag;
        if (!drag) return;
        const pointer = drag.horizontal ? event.clientX : event.clientY;
        const trackRange = Math.max(1, drag.trackSize - drag.thumbSize);
        const scrollRange = drag.horizontal
          ? Math.max(0, (holderRef.querySelector('.ant-table-tbody-virtual-holder-inner')?.scrollWidth || holderRef.clientWidth) - holderRef.clientWidth)
          : Math.max(0, holderRef.scrollHeight - holderRef.clientHeight);
        const nextScroll = drag.scroll + (pointer - drag.pointer) / trackRange * scrollRange;
        if (drag.horizontal) holderRef.scrollLeft = nextScroll;
        else holderRef.scrollTop = nextScroll;
      };
      binding.onScrollbarPointerUp = () => {
        binding.scrollbarDrag?.thumb.classList.remove('ant-table-tbody-virtual-scrollbar-thumb-moving');
        binding.scrollbarDrag = null;
        document.removeEventListener('pointermove', binding.onScrollbarPointerMove);
        document.removeEventListener('pointerup', binding.onScrollbarPointerUp);
        document.removeEventListener('pointercancel', binding.onScrollbarPointerUp);
      };

      holderRef.addEventListener('scroll', binding.updateWindow, { passive: true });
      holderRef.addEventListener('wheel', binding.onWheel, { passive: false });
      holderRef.addEventListener('touchstart', binding.onTouchStart, { passive: true });
      holderRef.addEventListener('touchmove', binding.onTouchMove, { passive: false });
      holderRef.addEventListener('touchend', binding.onTouchEnd, { passive: true });
      holderRef.parentElement?.addEventListener('pointerdown', binding.onScrollbarPointerDown);
      holderRef.virtualWindowBinding = binding;
    }

    binding.dotNetRef = dotNetRef;
    binding.itemCount = itemCount;
    binding.itemHeight = itemHeight;
    binding.startIndex = startIndex;
    binding.endIndex = endIndex;
    binding.pendingKey = null;
    tableHelper.updateVirtualScrollbars(holderRef, binding, false);
    binding.updateWindow();
  }

  static updateVirtualScrollbars(holderRef, binding, showVertical) {
    const wrapper = holderRef?.parentElement;
    const vertical = wrapper?.querySelector('.ant-table-tbody-virtual-scrollbar-vertical');
    const horizontal = wrapper?.querySelector('.ant-table-tbody-virtual-scrollbar-horizontal');
    const inner = holderRef?.querySelector('.ant-table-tbody-virtual-holder-inner');

    if (vertical) {
      const viewport = holderRef.clientHeight;
      const range = Math.max(holderRef.scrollHeight, binding.itemCount * binding.itemHeight);
      const thumbSize = Math.floor(Math.max(20, viewport * viewport / Math.max(1, range)));
      const thumbRange = Math.max(0, viewport - thumbSize);
      const scrollRange = Math.max(1, range - viewport);
      const thumbTop = Math.round(holderRef.scrollTop / scrollRange * thumbRange * 10000) / 10000;
      const thumb = vertical.firstElementChild as HTMLElement;
      thumb.style.height = `${thumbSize}px`;
      thumb.style.top = `${thumbTop}px`;

      if (showVertical) {
        vertical.classList.add('ant-table-tbody-virtual-scrollbar-visible');
        clearTimeout(binding.scrollbarTimer);
        binding.scrollbarTimer = setTimeout(() => vertical.classList.remove('ant-table-tbody-virtual-scrollbar-visible'), 3000);
      }
    }

    if (horizontal && inner) {
      const viewport = holderRef.clientWidth;
      const range = Math.max(viewport, inner.scrollWidth);
      const thumbSize = Math.floor(Math.max(20, viewport * viewport / Math.max(1, range)));
      const thumbRange = Math.max(0, viewport - thumbSize);
      const scrollRange = Math.max(1, range - viewport);
      const thumbLeft = Math.round(holderRef.scrollLeft / scrollRange * thumbRange * 10000) / 10000;
      const thumb = horizontal.firstElementChild as HTMLElement;
      thumb.style.width = `${thumbSize}px`;
      thumb.style.left = `${thumbLeft}px`;
    }
  }

  static unbindVirtualWindow(holderRef) {
    const binding = holderRef?.virtualWindowBinding;
    if (!binding) return;
    holderRef.removeEventListener('scroll', binding.updateWindow);
    holderRef.removeEventListener('wheel', binding.onWheel);
    holderRef.removeEventListener('touchstart', binding.onTouchStart);
    holderRef.removeEventListener('touchmove', binding.onTouchMove);
    holderRef.removeEventListener('touchend', binding.onTouchEnd);
    holderRef.parentElement?.removeEventListener('pointerdown', binding.onScrollbarPointerDown);
    binding.onScrollbarPointerUp();
    clearTimeout(binding.scrollbarTimer);
    delete holderRef.virtualWindowBinding;
  }

  static syncVirtualRowSpanOverlays(bodyRef) {
    const inner = bodyRef?.querySelector('.ant-table-tbody-virtual-holder-inner') as HTMLElement;
    if (!inner) {
      return;
    }

    const rows = Array.from(inner.querySelectorAll(':scope > .ant-table-row:not(.ant-table-row-extra)[data-row-key]')) as HTMLElement[];
    const innerTop = inner.getBoundingClientRect().top;
    const rowItems = rows.map(row => {
      const cells = Array.from(row.querySelectorAll(':scope > .ant-table-cell')) as HTMLElement[];
      const spanCells = cells.map((cell, cellIndex) => {
        const marker = Number.parseInt(cell.getAttribute('data-virtual-rowspan') || '0', 10);
        if (marker > 1) {
          bodyRef.virtualRowSpanCells = bodyRef.virtualRowSpanCells || new WeakMap();
          bodyRef.virtualRowSpanCells.set(cell, marker);
          cell.removeAttribute('data-virtual-rowspan');
        }
        return {
          cell,
          cellIndex,
          rowSpan: marker > 1
            ? marker
            : (bodyRef.virtualRowSpanCells?.get(cell) || Number.parseInt(cell.getAttribute('rowspan') || '1', 10) || 1)
        };
      }).filter(item => item.rowSpan > 1);

      return {
        row,
        key: row.getAttribute('data-row-key'),
        top: Math.round((row.getBoundingClientRect().top - innerTop) * 100) / 100,
        spanCells
      };
    });
    const signature = JSON.stringify(rowItems.map(item => [item.key, item.top, item.spanCells.map(cell => [cell.cellIndex, cell.rowSpan])]));
    const existingRows = Array.from(inner.querySelectorAll(':scope > .ant-table-row-extra')) as HTMLElement[];

    if (bodyRef.virtualRowSpanSignature === signature) {
      // Blazor may reconcile a source span cell after the overlay was created.
      // Move its content back into the existing overlay without rebuilding rows.
      rowItems.forEach(item => item.spanCells.forEach(span => {
        const clone = existingRows.find(row => row.getAttribute('data-row-key') === item.key);
        const cloneCell = clone?.querySelectorAll(':scope > .ant-table-cell')[span.cellIndex] as HTMLElement;
        if (cloneCell && span.cell.textContent?.trim() && cloneCell.innerHTML !== span.cell.innerHTML) {
          cloneCell.innerHTML = span.cell.innerHTML;
          span.cell.innerHTML = '';
        }
      }));
      return;
    }

    existingRows.forEach(row => row.remove());
    bodyRef.virtualRowSpanSignature = signature;

    const rowHeights = rows.map(row => row.getBoundingClientRect().height).filter(height => height > 0);
    const estimatedRowHeight = rowHeights.length
      ? rowHeights.reduce((total, height) => total + height, 0) / rowHeights.length
      : 55;

    rowItems.forEach((item, rowIndex) => {
      if (!item.spanCells.length) {
        return;
      }

      const overlay = item.row.cloneNode(true) as HTMLElement;
      overlay.classList.add('ant-table-row-extra');
      overlay.style.cssText = `top: ${item.top}px; width: ${item.row.getBoundingClientRect().width}px; position: absolute; pointer-events: none;`;
      [overlay, ...Array.from(overlay.querySelectorAll('*'))].forEach(element => {
        Array.from(element.attributes).forEach(attribute => {
          if (attribute.name.startsWith('_bl_') || attribute.name === 'id') {
            element.removeAttribute(attribute.name);
          }
        });
      });

      const overlayCells = Array.from(overlay.querySelectorAll(':scope > .ant-table-cell')) as HTMLElement[];
      const spans = new Map(item.spanCells.map(span => [span.cellIndex, span]));
      overlayCells.forEach((cell, cellIndex) => {
        const span = spans.get(cellIndex);
        if (!span) {
          cell.style.visibility = 'hidden';
          cell.innerHTML = '';
          return;
        }

        cell.removeAttribute('data-virtual-rowspan');
        cell.setAttribute('rowspan', String(span.rowSpan));
        let height = 0;
        for (let offset = 0; offset < span.rowSpan; offset += 1) {
          height += rows[rowIndex + offset]?.getBoundingClientRect().height || estimatedRowHeight;
        }
        cell.style.removeProperty('visibility');
        cell.style.height = `${Math.round(height * 1000) / 1000}px`;
      });

      inner.appendChild(overlay);
      item.spanCells.forEach(span => span.cell.innerHTML = '');
    });
  }

  static bindTableRowHover(wrapperRef) {
    if (!wrapperRef || wrapperRef.tableRowHoverCleanup) {
      return;
    }

    let hoveredRow = null;
    const getOwnedRow = (target) => {
      const row = target instanceof Element ? target.closest('.ant-table-tbody > tr.ant-table-row') : null;
      return row?.closest('.ant-table-wrapper') === wrapperRef ? row : null;
    };
    const setHoveredRow = (row) => {
      if (hoveredRow === row) {
        return;
      }
      hoveredRow?.querySelectorAll(':scope > td').forEach(cell => cell.classList.remove('ant-table-cell-row-hover'));
      hoveredRow = row;
      hoveredRow?.querySelectorAll(':scope > td').forEach(cell => cell.classList.add('ant-table-cell-row-hover'));
    };
    const onPointerOver = event => setHoveredRow(getOwnedRow(event.target));
    const onPointerOut = event => {
      const nextRow = getOwnedRow(event.relatedTarget);
      if (!nextRow) {
        setHoveredRow(null);
      }
    };

    wrapperRef.addEventListener('pointerover', onPointerOver);
    wrapperRef.addEventListener('pointerout', onPointerOut);
    wrapperRef.tableRowHoverCleanup = () => {
      wrapperRef.removeEventListener('pointerover', onPointerOver);
      wrapperRef.removeEventListener('pointerout', onPointerOut);
      setHoveredRow(null);
      delete wrapperRef.tableRowHoverCleanup;
    };
  }

  static unbindTableRowHover(wrapperRef) {
    wrapperRef?.tableRowHoverCleanup?.();
  }

  static isHidden(element) {
    if (element) {
      const computedStyle = getComputedStyle(element);
      if (computedStyle.display === "none" || computedStyle.visibility === "hidden") {
        return true;
      }
      if (element.parentElement && tableHelper.isHidden(element.parentElement)) {
        return true;
      }
    }
    return false;
  }

  static isIgnore(element, style: CSSStyleDeclaration, left: number, right: number) {
    const previousPosition = style.getPropertyValue('position');
    const rect = element.getBoundingClientRect();
    if (rect.left > right || rect.right < left) {
      return true;
    }

    if (previousPosition === 'absolute') {
      return true;
    }
    return false;
  }
  static getTotalHeightAbove(rootElement, element): number {
    const currentRect = element.getBoundingClientRect();
    const parentRect = rootElement.getBoundingClientRect();
    return currentRect.top - parentRect.top;   
  }
  static getTotalHeightBelow(rootElement, element, marginBottom: number, left: number, right: number): number {
    let totalHeight = 0;
    let currentElement = element.nextElementSibling;

    let rowGap = 0;
    let parentBorderBottom = 0;
    let parentPaddingBottom = 0;
    const parentElement = element.parentElement;
    if (parentElement) {
      const parentComputedStyle = getComputedStyle(parentElement);
      if (parentComputedStyle.rowGap && parentComputedStyle.rowGap.endsWith('px')) {
        rowGap = manipulationHelper.parseNumericValue(parentComputedStyle.rowGap);        
      }
      if (parentComputedStyle.borderStyle != 'none' && parentComputedStyle.borderBottom) {        
        parentBorderBottom = manipulationHelper.parseNumericValue(parentComputedStyle.borderBottom);        
      }
      if (parentComputedStyle.paddingBottom) {
        parentPaddingBottom = manipulationHelper.parseNumericValue(parentComputedStyle.paddingBottom);
      }
    }
    let previousElement = element;
    let previousComputedStyle = getComputedStyle(element);

    while (currentElement) {
      const currentComputedStyle = getComputedStyle(currentElement);
      const hasSameZIndex = previousComputedStyle.getPropertyValue('z-index') == currentComputedStyle.getPropertyValue('z-index');
      if (!tableHelper.isIgnore(currentElement, currentComputedStyle, left, right) && hasSameZIndex) {
        totalHeight += currentElement.offsetHeight + Math.max(manipulationHelper.parseNumericValue(currentElement, 'marginTop'), manipulationHelper.parseNumericValue(previousElement, 'marginBottom')) + rowGap;

        previousElement = currentElement;
        previousComputedStyle = currentComputedStyle;
      }
      currentElement = currentElement.nextElementSibling;
   
    }
    if (!tableHelper.isIgnore(previousElement, previousComputedStyle, left, right)) {
      marginBottom = Math.max(manipulationHelper.parseNumericValue(previousElement, 'marginBottom'), marginBottom);
      if (parentBorderBottom > 0.1 || parentPaddingBottom>0.1) {
        totalHeight += marginBottom;
        marginBottom = 0;
      }
    }    

    if (parentElement) {     
      const parentComputedStyle = getComputedStyle(parentElement);
      if (!tableHelper.isIgnore(parentElement, parentComputedStyle, left, right)) {

        totalHeight += parentPaddingBottom;   
        totalHeight += parentBorderBottom;
        
      }
      if (parentElement === rootElement) {
        return totalHeight;
      }
      
      totalHeight += tableHelper.getTotalHeightBelow(rootElement, parentElement, marginBottom, left, right);
    }
    return totalHeight;
  }
  static parseHeightValue(value) {
    // 去除单位并转换为数字
    return parseFloat(value);
  }
  static getCssHeight(element,height) {
    if (element) {
      if (height !== 'auto' && height !== '0px' && height !== '') {
        // 将 'px' 以外的单位转换为像素
        if (height.endsWith('px')) {
          return tableHelper.parseHeightValue(height);
        } else if (height.endsWith('vh')) {
          return (tableHelper.parseHeightValue(height) / 100) * window.innerHeight;
        } else if (height.endsWith('%')) {
          // 获取父元素的高度
          const parentElement = element.parentElement;
          if (parentElement) {
            const parentComputedStyle = window.getComputedStyle(parentElement);
            const parentHeight = parentComputedStyle.height;
            return (tableHelper.parseHeightValue(height) / 100) * tableHelper.parseHeightValue(parentHeight);
          }
          return 0;
        }
        return height;
      }
    }
    return 0;
  }


  static getNumericHeight(element) {
    if (element && element.style && element.style.height) {
      const computedStyle = window.getComputedStyle(element);
      const height = tableHelper.getCssHeight(element, computedStyle.height);
      if (height > 0) {
        return height;
      }          
    }
    return 0;
  }
  static getContainer(element) {
    if (element) {     
      const height = tableHelper.getNumericHeight(element);    
      if (height > 0) {    
        return element;          
      }  
      const parent = this.getContainer(element.parentElement);
      if (parent != null) {
        return parent;
      }         
    }
    return null;
  }
  static setBodyHeight(bodyRef) {

    let container = tableHelper.getContainer(bodyRef.parentElement);
    if (!container) {
      container = document.body;
    }
    const rect = bodyRef.getBoundingClientRect();
    if (tableHelper.isHidden(bodyRef)) {
      return;
    }
    // 计算上面元素的总高度
    const heightAbove = tableHelper.getTotalHeightAbove(container, bodyRef);
    //console.log('heightAbove:' + heightAbove);

    // 计算下面元素的总高度
    const heightBelow = tableHelper.getTotalHeightBelow(container, bodyRef,0, rect.left, rect.right);
    //console.log('heightBelow:' + heightBelow);

    // 计算视口高度并减去滚动条的宽度
    const viewportHeight = container.clientHeight;
    //console.log('viewportHeight:' + viewportHeight);

    // 设置目标元素的高度

    const heightStyle = `${viewportHeight - heightAbove - heightBelow}px`;
    if (heightStyle !== bodyRef.style.height) {
      bodyRef.style.height = heightStyle;
    }

  }
  static bindTableScroll(wrapperRef, bodyRef, tableRef, headerRef, scrollX, scrollY, resizable, autoHeight) {
    const syncHeaderColumnWidths = () => {
      if (!scrollY || !bodyRef || !headerRef) {
        return;
      }

      const bodyColumns = bodyRef.querySelectorAll('colgroup col');
      const headerColumns = headerRef.querySelectorAll('colgroup col');
      const summaryColumns = wrapperRef.querySelectorAll('.ant-table-container > .ant-table-summary colgroup col');
      if (bodyRef.classList.contains('ant-table-tbody-virtual')
        || bodyRef.classList.contains('ant-table-tbody-virtual-holder')) {
        const headerTable = headerRef.querySelector('table');
        const headerCells = headerRef.querySelectorAll('.ant-table-thead th:not(.ant-table-cell-scrollbar)');
        const virtualRows = bodyRef.querySelectorAll('.ant-table-row');
        const innerHolder = bodyRef.querySelector('.ant-table-tbody-virtual-holder-inner');
        if (!headerTable || !headerColumns.length) {
          return;
        }

        const virtualContent = innerHolder?.parentElement;
        if (innerHolder && virtualContent) {
          const contentHeight = Math.ceil(innerHolder.scrollHeight);
          const contentHeightValue = `${contentHeight}px`;
          const currentContentHeight = Number.parseFloat(virtualContent.style.height);
          if (contentHeight > 0 && (!Number.isFinite(currentContentHeight) || contentHeight > currentContentHeight)) {
            virtualContent.style.height = contentHeightValue;
          }
        }

        const measuredTableWidth = headerTable.getBoundingClientRect().width;
        if (!measuredTableWidth) return;
        const containerWidth = headerRef.parentElement?.getBoundingClientRect().width || measuredTableWidth;
        const tableWidth = !scrollX ? Math.max(containerWidth, 1) : measuredTableWidth;
        const widthValue = `${Math.round(tableWidth * 1000) / 1000}px`;
        if (!scrollX && headerTable.style.width !== widthValue) {
          headerTable.style.width = widthValue;
        }
        const columnCount = Math.min(headerColumns.length, headerCells.length);
        let columnWidths;
        if (!scrollX) {
          // rc-table's virtual table supplies x=1 when scroll.x is omitted.
          // useWidthColumns then fills unconfigured columns from the available
          // container width: floor each average width and give the remainder
          // to the final column. Preserve the widths that were present before
          // this helper started assigning measured widths.
          if (!bodyRef.virtualInitialHeaderColumnWidths) {
            bodyRef.virtualInitialHeaderColumnWidths = new WeakMap();
          }
          const configuredWidths = [];
          for (let index = 0; index < headerColumns.length; index++) {
            const column = headerColumns[index];
            if (!bodyRef.virtualInitialHeaderColumnWidths.has(column)) {
              bodyRef.virtualInitialHeaderColumnWidths.set(column, column.style.width);
            }
            configuredWidths.push(bodyRef.virtualInitialHeaderColumnWidths.get(column));
          }
          let totalWidth = 0;
          let missingWidthCount = 0;
          const parseColumnWidth = value => {
            if (!value) return null;
            const normalized = value.trim();
            if (normalized.endsWith('%')) {
              const percentage = Number.parseFloat(normalized);
              return Number.isFinite(percentage) ? tableWidth * percentage / 100 : null;
            }
            const numericWidth = Number.parseFloat(normalized);
            return Number.isFinite(numericWidth) ? numericWidth : null;
          };
          const widths = Array.from({ length: columnCount }, (_, index) => {
            const configuredWidth = parseColumnWidth(configuredWidths[index]);
            if (configuredWidth != null && configuredWidth > 0) {
              totalWidth += configuredWidth;
              return configuredWidth;
            }
            missingWidthCount += 1;
            return null;
          });
          const maxFitWidth = Math.max(tableWidth, 1);
          let remainingWidth = Math.max(maxFitWidth - totalWidth, missingWidthCount);
          let remainingCount = missingWidthCount;
          const averageWidth = missingWidthCount ? remainingWidth / missingWidthCount : 0;
          let realTotal = 0;
          columnWidths = widths.map(width => {
            const resolvedWidth = width != null
              ? width
              : remainingCount === 1 ? remainingWidth : Math.floor(averageWidth);
            if (width == null) {
              remainingWidth -= Math.floor(averageWidth);
              remainingCount -= 1;
            }
            realTotal += resolvedWidth;
            return resolvedWidth;
          });
          if (realTotal < maxFitWidth && realTotal > 0) {
            const scale = maxFitWidth / realTotal;
            remainingWidth = maxFitWidth;
            columnWidths = columnWidths.map((width, index) => {
              const scaledWidth = index === columnWidths.length - 1
                ? remainingWidth
                : Math.floor(width * scale);
              remainingWidth -= scaledWidth;
              return scaledWidth;
            });
          }
        } else {
          columnWidths = Array.from({ length: columnCount }, (_, index) => headerColumns[index].getBoundingClientRect().width);
        }
        const contentWidth = columnWidths.reduce((total, width) => total + width, 0);

        columnWidths.forEach((width, index) => {
          const columnWidth = `${Math.round(width * 1000) / 1000}px`;
          if (headerColumns[index].style.width !== columnWidth) {
            headerColumns[index].style.width = columnWidth;
          }
        });

        const virtualTable = wrapperRef.matches('.ant-table') ? wrapperRef : wrapperRef.querySelector('.ant-table');
        if (virtualTable) {
          const hasHorizontalOverflow = contentWidth > bodyRef.clientWidth + 1;
          virtualTable.classList.toggle('ant-table-fix-start-shadow-show', hasHorizontalOverflow && bodyRef.scrollLeft > 0);
          virtualTable.classList.toggle('ant-table-fix-end-shadow-show', hasHorizontalOverflow && bodyRef.scrollLeft + bodyRef.clientWidth < contentWidth - 1);
          virtualTable.querySelectorAll('.ant-table-cell-fix-start-shadow').forEach(cell => {
            cell.classList.toggle('ant-table-cell-fix-start-shadow-show', hasHorizontalOverflow && bodyRef.scrollLeft > 0);
          });
          virtualTable.querySelectorAll('.ant-table-cell-fix-end-shadow').forEach(cell => {
            cell.classList.toggle('ant-table-cell-fix-end-shadow-show', hasHorizontalOverflow && bodyRef.scrollLeft + bodyRef.clientWidth < contentWidth - 1);
          });
        }

        const syncFixedOffset = (cell, index, span = 1) => {
          if (cell.classList.contains('ant-table-cell-fix-start')) {
            const offset = columnWidths.slice(0, index).reduce((total, width) => total + width, 0);
            cell.style.insetInlineStart = `${Math.round(offset * 1000) / 1000}px`;
          }
          if (cell.classList.contains('ant-table-cell-fix-end')) {
            const offset = columnWidths.slice(index + span).reduce((total, width) => total + width, 0);
            cell.style.insetInlineEnd = `${Math.round(offset * 1000) / 1000}px`;
          }
        };

        headerCells.forEach((cell, index) => syncFixedOffset(cell, index));

        virtualRows.forEach(row => {
          if (row.style.width !== widthValue) {
            row.style.width = widthValue;
          }
          row.querySelectorAll('.ant-table-cell[data-virtual-column]').forEach(cell => {
            const index = Number.parseInt(cell.getAttribute('data-virtual-column'), 10);
            const span = Math.max(1, Number.parseInt(cell.getAttribute('colspan') || '1', 10) || 1);
            const width = columnWidths.slice(index, index + span).reduce((total, value) => total + value, 0);
            if (!width) return;
            const columnWidth = `${Math.round(width * 1000) / 1000}px`;
            const cellFlex = `0 0 ${columnWidth}`;
            const mergedWidth = columnWidths.slice(index + 1, index + span).reduce((total, value) => total + value, 0);
            const marginRight = `${Math.round(mergedWidth * -1000) / 1000}px`;
            if (cell.style.flex !== cellFlex) cell.style.flex = cellFlex;
            if (cell.style.width !== columnWidth) cell.style.width = columnWidth;
            if (cell.style.marginRight !== marginRight) cell.style.marginRight = marginRight;
            if (cell.style.pointerEvents !== 'auto') cell.style.pointerEvents = 'auto';
            syncFixedOffset(cell, index, span);
            cell.removeAttribute('data-virtual-column');
          });
        });
        tableHelper.syncVirtualRowSpanOverlays(bodyRef);
        if (!bodyRef.virtualHeaderClassObserver && typeof MutationObserver !== 'undefined') {
          bodyRef.virtualHeaderClassObserver = new MutationObserver(() => syncHeaderColumnWidths());
          bodyRef.virtualHeaderClassObserver.observe(headerRef, {
            attributes: true,
            attributeFilter: ['class'],
            subtree: true
          });
        }
        if (innerHolder && !bodyRef.virtualRowObserver && typeof MutationObserver !== 'undefined') {
          let updateScheduled = false;
          bodyRef.virtualRowObserver = new MutationObserver(() => {
            if (updateScheduled) return;
            updateScheduled = true;
            requestAnimationFrame(() => {
              updateScheduled = false;
              syncHeaderColumnWidths();
            });
          });
          bodyRef.virtualRowObserver.observe(innerHolder, { attributes: true, attributeFilter: ['style'], childList: true, subtree: true });
        }
        return;
      }

      if (scrollX && bodyColumns.length >= headerColumns.length) {
        for (let index = 0; index < headerColumns.length; index++) {
          const bodyColumn = bodyColumns[index];
          if (!bodyColumn) {
            continue;
          }

          // The body table's auto layout has already resolved fluid columns
          // from their cell content. Measure those columns directly instead
          // of deriving widths from the header table's current (possibly stale)
          // width; the latter causes a feedback loop that progressively
          // shrinks fluid header columns.
          const width = bodyColumn.getBoundingClientRect().width;
          if (width <= 0) {
            continue;
          }
          const widthValue = `${Math.round(width * 1000) / 1000}px`;
          if (headerColumns[index].style.width !== widthValue) {
            headerColumns[index].style.width = widthValue;
          }
          if (summaryColumns[index] && summaryColumns[index].style.width !== widthValue) {
            summaryColumns[index].style.width = widthValue;
          }
        }
        return;
      }

      if (scrollX) {
        const headerTable = headerRef.querySelector('table');
        if (!headerTable) return;

        const fluidColumns = [];
        let explicitWidth = 0;
        for (let index = 0; index < headerColumns.length; index++) {
          const bodyColumn = bodyColumns[index];
          const width = bodyColumn?.style.width
            ? bodyColumn.getBoundingClientRect().width
            : 0;
          if (width > 0) {
            explicitWidth += width;
            const widthValue = `${Math.round(width * 1000) / 1000}px`;
            if (headerColumns[index].style.width !== widthValue) {
              headerColumns[index].style.width = widthValue;
            }
            if (summaryColumns[index] && summaryColumns[index].style.width !== widthValue) {
              summaryColumns[index].style.width = widthValue;
            }
            continue;
          }

          const naturalWidth = headerColumns[index].getBoundingClientRect().width;
          fluidColumns.push([index, naturalWidth]);
        }

        const availableWidth = Math.max(headerTable.getBoundingClientRect().width - explicitWidth, 0);
        const naturalWidthTotal = fluidColumns.reduce((total, [, width]) => total + width, 0);
        fluidColumns.forEach(([index, naturalWidth]) => {
          const width = naturalWidthTotal > 0
            ? availableWidth * naturalWidth / naturalWidthTotal
            : availableWidth / fluidColumns.length;
          const widthValue = `${Math.round(width * 1000) / 1000}px`;
          if (headerColumns[index].style.width !== widthValue) {
            headerColumns[index].style.width = widthValue;
          }
          if (summaryColumns[index] && summaryColumns[index].style.width !== widthValue) {
            summaryColumns[index].style.width = widthValue;
          }
        });

        return;
      }

      // With a fixed header and no horizontal scrolling, the body viewport is
      // narrower than the header by the vertical scrollbar width. Size fluid
      // header columns from the full header table width while preserving the
      // explicit column widths.
      const headerCells = headerRef.querySelectorAll('.ant-table-thead th:not(.ant-table-cell-scrollbar)');
      const headerTable = headerRef.querySelector('table');
      if (!headerTable) {
        return;
      }

      const fluidColumns = [];
      let explicitWidth = 0;
      const count = Math.min(headerColumns.length, headerCells.length);
      for (let index = 0; index < headerColumns.length; index++) {
        const column = headerColumns[index];
        // The header's fluid column can retain a width assigned during its
        // initial render. Determine which columns are explicitly sized from
        // the body colgroup, where React leaves fluid columns widthless.
        const bodyColumn = bodyColumns[index];
        const bodyColumnWidth = bodyColumn?.style.width;
        if (bodyColumnWidth) {
          // Percentage widths are already resolved by the browser on the body
          // table. Use that measured width so the sticky header receives the
          // same pixel sizing as rc-table after its layout pass.
          const measuredWidth = bodyColumn.getBoundingClientRect().width || Number.parseFloat(bodyColumnWidth) || 0;
          explicitWidth += measuredWidth;
          const widthValue = `${Math.round(measuredWidth * 1000) / 1000}px`;
          if (column.style.width !== widthValue) {
            column.style.width = widthValue;
          }
          continue;
        }

        if (index < count) {
          const naturalWidth = headerCells[index].getBoundingClientRect().width;
          if (naturalWidth > 0) {
            fluidColumns.push([column, naturalWidth, index]);
          }
        }
      }

      const tableContainerWidth = headerRef.parentElement?.getBoundingClientRect().width ?? headerRef.getBoundingClientRect().width;
      const availableWidth = Math.max(tableContainerWidth - explicitWidth, 0);
      const naturalWidthTotal = fluidColumns.reduce((total, [, width]) => total + width, 0);
      fluidColumns.forEach(([column, naturalWidth, columnIndex]) => {
        const width = naturalWidthTotal > 0 ? availableWidth * naturalWidth / naturalWidthTotal : availableWidth / fluidColumns.length;
        const widthValue = `${Math.round(width * 1000) / 1000}px`;
        if (column.style.width !== widthValue) {
          column.style.width = widthValue;
        }
        if (summaryColumns[columnIndex] && summaryColumns[columnIndex].style.width !== widthValue) {
          summaryColumns[columnIndex].style.width = widthValue;
        }
      });
    };

    bodyRef.bindScroll = () => {
      if (scrollX) {
        tableHelper.SetScrollPositionClassName(bodyRef, wrapperRef);
      }
      if (scrollY) {
        headerRef.scrollLeft = bodyRef.scrollLeft;
        const summaryRef = wrapperRef.querySelector('.ant-table-container > .ant-table-summary');
        if (summaryRef) {
          summaryRef.scrollLeft = bodyRef.scrollLeft;
        }
      }
      syncHeaderColumnWidths();
      if (autoHeight) {
        tableHelper.setBodyHeight(bodyRef);
      }

    }

    if (scrollY && typeof ResizeObserver !== 'undefined') {
      bodyRef.columnWidthObserver = new ResizeObserver(() => syncHeaderColumnWidths());
      const bodyTable = bodyRef.querySelector('.ant-table-body table');
      if (bodyTable) {
        bodyRef.columnWidthObserver.observe(bodyTable);
        bodyTable.querySelectorAll('colgroup col').forEach(column => bodyRef.columnWidthObserver.observe(column));
      }
      const headerTable = headerRef.querySelector('table');
      if (headerTable) {
        bodyRef.columnWidthObserver.observe(headerTable);
        headerTable.querySelectorAll('.ant-table-thead th').forEach(cell => bodyRef.columnWidthObserver.observe(cell));
      }
      requestAnimationFrame(syncHeaderColumnWidths);
    }

    bodyRef.addEventListener && bodyRef.addEventListener('scroll', bodyRef.bindScroll);
    window.addEventListener('resize', bodyRef.bindScroll);
    // Initialize fixed-column shadows and synchronized widths as soon as the
    // table has been rendered. A delayed update leaves the initial DOM without
    // the same scroll-position classes React renders on first paint.
    bodyRef.bindScroll();
    requestAnimationFrame(() => bodyRef.bindScroll());

    if (resizable) {
      tableHelper.enableColumnResizing(headerRef, tableRef, scrollY);
    }
    if (autoHeight) {
      bodyRef.observer = new MutationObserver(mutations => {
        if (mutations) {         
          tableHelper.setBodyHeight(bodyRef);
        }
      });
      const config = { childList: true, subtree: true, attributes: true, attributeFilter: ['display', 'visibility','aria-selected']};
      const target = document.body; // 要观察变动的 DOM 节点
      bodyRef.observer.observe(target, config);
      tableHelper.setBodyHeight(bodyRef);
    }

    if (scrollX) {
      const stickyRef = wrapperRef.querySelector('.ant-table-sticky-scroll');
      const stickyBarRef = wrapperRef.querySelector('.ant-table-sticky-scroll-bar');
      if (stickyRef && stickyBarRef) {
        tableHelper.enableStickyScroll(wrapperRef, bodyRef, stickyRef, stickyBarRef);
      }
    }
  }

  static unbindTableScroll(bodyRef) {
    if (bodyRef) {
      bodyRef.removeEventListener && bodyRef.removeEventListener('scroll', bodyRef.bindScroll);
      window.removeEventListener('resize', bodyRef.bindScroll);
      if (bodyRef.observer) {
        bodyRef.observer.disconnect();
      }
      if (bodyRef.columnWidthObserver) {
        bodyRef.columnWidthObserver.disconnect();
      }
      if (bodyRef.virtualHeaderClassObserver) {
        bodyRef.virtualHeaderClassObserver.disconnect();
        bodyRef.virtualHeaderClassObserver = null;
      }

      const wrapperRef = bodyRef.closest('.ant-table-wrapper');
      if (wrapperRef) {
        const stickyRef = wrapperRef.querySelector('.ant-table-sticky-scroll');
        if (stickyRef && stickyRef.cleanup) {
          stickyRef.cleanup();
        }
      }
    }
  }
  
  static SetScrollPositionClassName(bodyRef, wrapperRef) {

    const scrollLeft = bodyRef.scrollLeft;
    const scrollWidth = bodyRef.scrollWidth;
    const clientWidth = bodyRef.clientWidth;

    let pingLeft = false;
    let pingRight = false;

    if ((scrollWidth == clientWidth && scrollWidth != 0)) {
      pingLeft = false;
      pingRight = false;
    }
    else if (scrollLeft == 0) {
      pingLeft = false;
      pingRight = true;
    }
    else if (Math.abs(scrollWidth - (scrollLeft + clientWidth)) <= 1) {
      pingRight = false;
      pingLeft = true;
    }
    else {
      pingLeft = true;
      pingRight = true;
    }

    wrapperRef.classList.remove("ant-table-ping-left", "ant-table-ping-right");
    wrapperRef.classList.toggle("ant-table-fix-start-shadow-show", pingLeft);
    wrapperRef.classList.toggle("ant-table-fix-end-shadow-show", pingRight);
    wrapperRef.querySelectorAll(".ant-table-cell-fix-start-shadow").forEach(cell =>
      cell.classList.toggle("ant-table-cell-fix-start-shadow-show", pingLeft));
    wrapperRef.querySelectorAll(".ant-table-cell-fix-end-shadow").forEach(cell =>
      cell.classList.toggle("ant-table-cell-fix-end-shadow-show", pingRight));
  }

  static enableColumnResizing(headerElement, tableElement, scrollY) {

    const cols = tableElement.querySelectorAll('col');
    const ths = scrollY ?
      headerElement.querySelectorAll('.ant-table-thead th') :
      tableElement.tHead.querySelectorAll('.ant-table-thead th');
    const headerCols = scrollY ? headerElement.querySelectorAll('col') : null;

    ths.forEach((th, i) => {

      const col = cols[i];
      const headerCol = headerCols ? headerCols[i] : null;
      const handle = document.createElement('div');
      handle.classList.add('ant-table-resizable-handle');
      handle.style.height = `100%`;

      th.appendChild(handle);


      handle.addEventListener('mousedown', handleMouseDown);
      if ('ontouchstart' in window) {
        handle.addEventListener('touchstart', handleMouseDown);
      }

      function handleMouseDown(evt) {
        evt.preventDefault();
        evt.stopPropagation();

        //const th = handle.parentElement;
        const startPageX = evt.touches ? evt.touches[0].pageX : evt.pageX;
        const originalColumnWidth = th.offsetWidth;
        const rtlMultiplier = window.getComputedStyle(th, null).getPropertyValue('direction') === 'rtl' ? -1 : 1;
        let updatedColumnWidth = 0;
        handle.classList.add('ant-table-resizing');

        function handleMouseMove(evt) {
          evt.stopPropagation();
          const newPageX = evt.touches ? evt.touches[0].pageX : evt.pageX;
          const nextWidth = originalColumnWidth + (newPageX - startPageX) * rtlMultiplier - 5;
          if (Math.abs(nextWidth - updatedColumnWidth) > 0) {
            updatedColumnWidth = nextWidth;
            handle.style.right = '';
            handle.style.left = `${updatedColumnWidth}px`;
          }
        }

        function handleMouseUp() {
          if (updatedColumnWidth > 0) {
            th.style.width = `${updatedColumnWidth}px`;
            col.style.width = `${updatedColumnWidth}px`;
            if (headerCol) {
              headerCol.style.width = `${updatedColumnWidth}px`;
            }
            handle.style.right = '0';
            handle.style.left = '';
            handle.classList.remove('ant-table-resizing');
          }

          document.body.removeEventListener('mousemove', handleMouseMove);
          document.body.removeEventListener('mouseup', handleMouseUp);
          document.body.removeEventListener('touchmove', handleMouseMove);
          document.body.removeEventListener('touchend', handleMouseUp);
        }

        if (evt instanceof TouchEvent) {
          document.body.addEventListener('touchmove', handleMouseMove, { passive: true });
          document.body.addEventListener('touchend', handleMouseUp, { passive: true });
        } else {
          document.body.addEventListener('mousemove', handleMouseMove, { passive: true });
          document.body.addEventListener('mouseup', handleMouseUp, { passive: true });
        }
      }
    });
  }

  static isTableBottomVisible(tableElement) {
    if (!tableElement) return true;

    // Find the viewport height
    const viewportHeight = window.innerHeight;
    const tableRect = tableElement.getBoundingClientRect();

    // 检查表格底部是否在视口之外
    return tableRect.bottom <= viewportHeight;
  }

  static enableStickyScroll(wrapperRef, bodyRef, stickyRef, stickyBarRef) {
    if (!stickyRef || !stickyBarRef || !bodyRef) return;

    const updateStickyScroll = () => {
      const { clientWidth, scrollWidth, scrollLeft } = bodyRef;
      
      // Check both scroll width and table bottom visibility
      const isScrollable = scrollWidth > clientWidth;
      const isBottomVisible = tableHelper.isTableBottomVisible(wrapperRef);
      
      // Only show sticky scroll when content is scrollable AND table bottom is not visible
      if (!isScrollable || isBottomVisible) {
        if (stickyRef.style.display !== 'none') {
          stickyRef.style.display = 'none';
        }
        return;
      }

      // Show sticky scroll
      if (stickyRef.style.display !== 'block') {
        stickyRef.style.display = 'block';
      }
      
      // Sync sticky scroll width with table width
      stickyRef.style.width = `${clientWidth}px`;
      
      // Calculate bar position based on scroll position
      const scrollRatio = scrollLeft / (scrollWidth - clientWidth);
      const maxBarOffset = stickyRef.clientWidth - stickyBarRef.clientWidth;
      const barLeft = scrollRatio * maxBarOffset;
      
      stickyBarRef.style.transform = `translate3d(${barLeft}px, 0px, 0px)`;
    };

    // Handle sticky bar dragging
    let isDragging = false;
    let startMouseX = 0;
    let startBarLeft = 0;

    const onMouseDown = (e) => {
      e.preventDefault();
      isDragging = true;
      startMouseX = e.clientX;
      
      const transform = window.getComputedStyle(stickyBarRef).transform;
      const matrix = new DOMMatrix(transform);
      startBarLeft = matrix.m41;
      
      document.addEventListener('mousemove', onMouseMove);
      document.addEventListener('mouseup', onMouseUp);
      stickyBarRef.classList.add('ant-table-sticky-scroll-bar-active');
    };

    const onMouseMove = (e) => {
      if (!isDragging) return;
      
      e.preventDefault();
      const { clientWidth, scrollWidth } = bodyRef;
      const maxBarOffset = stickyRef.clientWidth - stickyBarRef.clientWidth;
      
      const mouseDelta = e.clientX - startMouseX;
      const newBarLeft = Math.max(0, Math.min(maxBarOffset, startBarLeft + mouseDelta));
      
      const scrollRatio = newBarLeft / maxBarOffset;
      const maxScroll = scrollWidth - clientWidth;
      bodyRef.scrollLeft = scrollRatio * maxScroll;
    };

    const updateStickyBarWidth = () => {
      const { clientWidth, scrollWidth } = bodyRef;
      if (scrollWidth > clientWidth) {
        const ratio = clientWidth / scrollWidth;
        const barWidth = Math.max(ratio * stickyRef.clientWidth, 20);
        stickyBarRef.style.width = `${barWidth}px`;
      }
    };

    const onMouseUp = () => {
      isDragging = false;
      document.removeEventListener('mousemove', onMouseMove);
      document.removeEventListener('mouseup', onMouseUp);
      stickyBarRef.classList.remove('ant-table-sticky-scroll-bar-active');
    };

    // Create ResizeObserver to monitor table width changes
    const resizeObserver = new ResizeObserver(() => {
      requestAnimationFrame(updateStickyScroll);
      requestAnimationFrame(updateStickyBarWidth);
    });

    // Create IntersectionObserver to monitor table visibility
    const visibilityObserver = new IntersectionObserver(
      (entries) => {
        entries.forEach((entry) => {
          if (entry.isIntersecting) {
            requestAnimationFrame(updateStickyScroll);
          }
        });
      },
      {
        threshold: [0, 1],
        root: null
      }
    );

    // Observe both the table body and wrapper for size changes
    resizeObserver.observe(bodyRef);
    resizeObserver.observe(wrapperRef);

    // Observe the table wrapper
    visibilityObserver.observe(wrapperRef);

    // Add scroll event listener to window
    const scrollHandler = () => {
      requestAnimationFrame(updateStickyScroll);
    };
    window.addEventListener('scroll', scrollHandler, { passive: true });

    // Bind events
    stickyBarRef.addEventListener('mousedown', onMouseDown);
    bodyRef.addEventListener('scroll', scrollHandler, { passive: true });
    window.addEventListener('resize', () => {
      requestAnimationFrame(() => {
        updateStickyBarWidth();
        updateStickyScroll();
      });
    });

    // Store cleanup function
    stickyRef.cleanup = () => {
      stickyBarRef.removeEventListener('mousedown', onMouseDown);
      bodyRef.removeEventListener('scroll', scrollHandler);
      window.removeEventListener('scroll', scrollHandler);
      window.removeEventListener('resize', updateStickyScroll);
      resizeObserver.disconnect();
      visibilityObserver.disconnect();
    };

    // Initial update
    updateStickyBarWidth();
    updateStickyScroll();
  }
}
