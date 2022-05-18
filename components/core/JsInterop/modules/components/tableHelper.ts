export class tableHelper {
  static bindTableScroll(bodyRef, tableRef, headerRef, scrollX, scrollY) {
    bodyRef.bindScroll = () => {
      if (scrollX) {
        tableHelper.SetScrollPositionClassName(bodyRef, tableRef);
      }
      if (scrollY) {
        headerRef.scrollLeft = bodyRef.scrollLeft;
      }
    }

    // direct setting classlist will not work, so delay 500ms for workaround
    setTimeout(() => {
     bodyRef && bodyRef.bindScroll();
    }, 500);

    bodyRef.addEventListener && bodyRef.addEventListener('scroll', bodyRef.bindScroll);
    window.addEventListener('resize', bodyRef.bindScroll);
  }

  static unbindTableScroll(bodyRef) {
    if (bodyRef) {
      bodyRef.removeEventListener && bodyRef.removeEventListener('scroll', bodyRef.bindScroll);
      window.removeEventListener('resize', bodyRef.bindScroll);
    }
  }

  static SetScrollPositionClassName(bodyRef, tableRef) {

    let scrollLeft = bodyRef.scrollLeft;
    let scrollWidth = bodyRef.scrollWidth;
    let clientWidth = bodyRef.clientWidth;

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

    pingLeft ? tableRef.classList.add("ant-table-ping-left") : tableRef.classList.remove("ant-table-ping-left");
    pingRight ? tableRef.classList.add("ant-table-ping-right") : tableRef.classList.remove("ant-table-ping-right");
  }
}