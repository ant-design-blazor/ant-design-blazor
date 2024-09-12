export class tableHelper {
  static isHidden(element) {
    if (element instanceof HTMLElement) {
      const computedStyle = getComputedStyle(element);
      if (computedStyle.display === "none" || computedStyle.visibility === "hidden") {
        return true;
      }
    }
    if (element.parentNode != null) {
      if (tableHelper.isHidden(element.parentNode)) {
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
  static getTotalHeightAbove(element): number {
    let totalHeight = 0;
    const rect = element.getBoundingClientRect();
    totalHeight += rect.top;

    if (element instanceof HTMLElement) {
      const computedStyle = getComputedStyle(element);
      totalHeight += parseFloat(computedStyle.paddingTop);
      totalHeight += parseFloat(computedStyle.borderTop);
    }
    return totalHeight + 1;
  }
  static getTotalHeightBelow(element, left: number, right: number): number {
    let totalHeight = 0;
    let currentElement = element.nextElementSibling;

    if (element instanceof HTMLElement) {
      let previousElement = element;
      let previousComputedStyle = getComputedStyle(element);
      if (!tableHelper.isIgnore(element, previousComputedStyle, left, right)) {
        totalHeight += parseFloat(previousComputedStyle.marginBottom);
      }

      while (currentElement) {
        const currentComputedStyle = getComputedStyle(currentElement);
        if (!tableHelper.isIgnore(currentElement, currentComputedStyle, left, right)) {
          totalHeight += currentElement.offsetHeight + Math.max(parseFloat(currentComputedStyle.marginTop), parseFloat(previousComputedStyle.marginBottom));
        }
        previousElement = currentElement;
        currentElement = currentElement.nextElementSibling;
        previousComputedStyle = currentComputedStyle;
      }
      if (!tableHelper.isIgnore(previousElement, previousComputedStyle, left, right)) {
        totalHeight += parseFloat(previousComputedStyle.marginBottom);
      }
    }
    if (element.parentNode != null) {
      if (element.parentNode instanceof HTMLElement) {
        const parentComputedStyle = getComputedStyle(element.parentNode);
        if (!tableHelper.isIgnore(element.parentNode, parentComputedStyle, left, right)) {
          totalHeight += parseFloat(parentComputedStyle.paddingBottom);
        }
      }
      totalHeight += tableHelper.getTotalHeightBelow(element.parentNode, left, right);
    }
    return totalHeight;
  }
  static setBodyHeight(bodyRef) {
    const rect = bodyRef.getBoundingClientRect();
    if (tableHelper.isHidden(bodyRef)) {
      return;
    }
    // 计算上面元素的总高度
    const heightAbove = tableHelper.getTotalHeightAbove(bodyRef);
    //console.log('heightAbove:' + heightAbove);

    // 计算下面元素的总高度
    const heightBelow = tableHelper.getTotalHeightBelow(bodyRef, rect.left, rect.right);
    //console.log('heightBelow:' + heightBelow);

    // 计算视口高度并减去滚动条的宽度
    const viewportHeight = window.innerHeight;

    // 设置目标元素的高度

    const heightStyle = `${viewportHeight - heightAbove - heightBelow}px`;
    if (heightStyle !== bodyRef.style.height) {
      bodyRef.style.height = heightStyle;
    }

  }
  static bindTableScroll(wrapperRef, bodyRef, tableRef, headerRef, scrollX, scrollY, resizable, autoHeight) {
    bodyRef.bindScroll = () => {
      if (scrollX) {
        tableHelper.SetScrollPositionClassName(bodyRef, wrapperRef);
      }
      if (scrollY) {
        headerRef.scrollLeft = bodyRef.scrollLeft;
      }
      if (autoHeight) {
        tableHelper.setBodyHeight(bodyRef);
      }

    }

    // direct setting classlist will not work, so delay 500ms for workaround
    setTimeout(() => {
      bodyRef && bodyRef.bindScroll();
    }, 500);

    bodyRef.addEventListener && bodyRef.addEventListener('scroll', bodyRef.bindScroll);
    window.addEventListener('resize', bodyRef.bindScroll);

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
  }

  static unbindTableScroll(bodyRef) {
    if (bodyRef) {
      bodyRef.removeEventListener && bodyRef.removeEventListener('scroll', bodyRef.bindScroll);
      window.removeEventListener('resize', bodyRef.bindScroll);
      if (bodyRef.observer) {
        bodyRef.observer.disconnect();
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

    pingLeft ? wrapperRef.classList.add("ant-table-ping-left") : wrapperRef.classList.remove("ant-table-ping-left");
    pingRight ? wrapperRef.classList.add("ant-table-ping-right") : wrapperRef.classList.remove("ant-table-ping-right");
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
      handle.style.height = `${tableElement.offsetHeight}px`;

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
}