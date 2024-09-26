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
    if (parentElement != null) {
      const parentComputedStyle = getComputedStyle(parentElement);
      if (parentComputedStyle.rowGap) {
        if (parentComputedStyle.rowGap.endsWith('px')) {
          rowGap = parseFloat(parentComputedStyle.rowGap);
        }
        if (parentComputedStyle.borderStyle != 'none') {
          if (parentComputedStyle.borderBottom) {
            parentBorderBottom = parseFloat(parentComputedStyle.borderBottom);
          }
        }
        if (parentComputedStyle.paddingBottom) {
          parentPaddingBottom = parseFloat(parentComputedStyle.paddingBottom);
        }
      }
    }
    let previousElement = element;
    let previousComputedStyle = getComputedStyle(element);


    while (currentElement) {
      const currentComputedStyle = getComputedStyle(currentElement);
      if (!tableHelper.isIgnore(currentElement, currentComputedStyle, left, right)) {
        totalHeight += currentElement.offsetHeight + Math.max(parseFloat(currentComputedStyle.marginTop), parseFloat(previousComputedStyle.marginBottom)) + rowGap;

        previousElement = currentElement;
        previousComputedStyle = currentComputedStyle;
      }
      currentElement = currentElement.nextElementSibling;
   
    }
    if (!tableHelper.isIgnore(previousElement, previousComputedStyle, left, right)) {
      marginBottom = Math.max(parseFloat(previousComputedStyle.marginBottom), marginBottom);
      if (parentBorderBottom > 0.1 || parentPaddingBottom>0.1) {
        totalHeight += marginBottom;
        marginBottom = 0;
      }

    }
    

    if (parentElement!=null) {     

      const parentComputedStyle = getComputedStyle(parentElement);
      if (!tableHelper.isIgnore(parentElement, parentComputedStyle, left, right)) {

        totalHeight += parentPaddingBottom;   
        totalHeight += parentBorderBottom;
        
      }
      if (parentElement == rootElement) {
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
    if (element != null) {
      if (height !== 'auto' && height !== '0px' && height !== '') {
        // 将 'px' 以外的单位转换为像素
        if (height.endsWith('px')) {
          return tableHelper.parseHeightValue(height);
        } else if (height.endsWith('vh')) {
          return (tableHelper.parseHeightValue(height) / 100) * window.innerHeight;
        } else if (height.endsWith('%')) {
          // 获取父元素的高度
          const parentElement = element.parentElement;
          if (parentElement != null) {
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
    if (element != null) {
      if (element.style && element.style.height) {
        const computedStyle = window.getComputedStyle(element);
        const height = tableHelper.getCssHeight(element, computedStyle.height);
        if (height > 0) {
          return height;
        }
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