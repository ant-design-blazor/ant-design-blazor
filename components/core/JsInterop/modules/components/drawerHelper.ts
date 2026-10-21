export class drawerHelper {
  static attach(component, wrapper, dragger, placement, maxSize) {
    const horizontal = placement === 'left' || placement === 'right';
    let startPosition = 0;
    let startSize = 0;
    let dragging = false;

    const removeListeners = () => {
      document.removeEventListener('mousemove', onMouseMove);
      document.removeEventListener('mouseup', onMouseUp);
    };

    const onMouseMove = (event) => {
      if (!dragging) return;
      let delta = (horizontal ? event.clientX : event.clientY) - startPosition;
      if (placement === 'right' || placement === 'bottom') delta = -delta;
      let nextSize = Math.max(0, Math.round(startSize + delta));
      if (maxSize > 0) nextSize = Math.min(nextSize, maxSize);

      const property = horizontal ? 'width' : 'height';
      wrapper.style[property] = `${nextSize}px`;
      component.invokeMethodAsync('OnDrawerResized', nextSize);
    };

    const onMouseUp = () => {
      if (!dragging) return;
      dragging = false;
      wrapper.classList.remove('ant-drawer-content-wrapper-dragging');
      dragger.classList.remove('ant-drawer-resizable-dragger-dragging');
      removeListeners();
      const rect = wrapper.getBoundingClientRect();
      component.invokeMethodAsync('OnDrawerResizeEnded', Math.round(horizontal ? rect.width : rect.height));
    };

    const onMouseDown = (event) => {
      event.preventDefault();
      event.stopPropagation();
      const rect = wrapper.getBoundingClientRect();
      startPosition = horizontal ? event.clientX : event.clientY;
      startSize = horizontal ? rect.width : rect.height;
      dragging = true;
      wrapper.classList.add('ant-drawer-content-wrapper-dragging');
      dragger.classList.add('ant-drawer-resizable-dragger-dragging');
      document.addEventListener('mousemove', onMouseMove);
      document.addEventListener('mouseup', onMouseUp);
      component.invokeMethodAsync('OnDrawerResizeStarted', Math.round(startSize));
    };

    dragger.addEventListener('mousedown', onMouseDown);
    return {
      dispose() {
        removeListeners();
        dragger.removeEventListener('mousedown', onMouseDown);
      },
    };
  }
}
