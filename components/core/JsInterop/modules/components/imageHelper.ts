export class imageHelper {
  static imgDragAndDrop(element: HTMLElement | HTMLImageElement): void {
    const image = element instanceof HTMLImageElement ? element : element?.querySelector('img');
    if (!image) {
      return;
    }

    let mouseX: number, mouseY: number, imgX: number, imgY: number;
    let isDragging = false;

    function handleMouseDown(event: MouseEvent) {
      mouseX = event.clientX;
      mouseY = event.clientY;
      imgX = image.offsetLeft;
      imgY = image.offsetTop;
      isDragging = true;

      image.style.cursor = 'grabbing';

      document.addEventListener('mousemove', handleMouseMove);
      document.addEventListener('mouseup', handleMouseUp);
    }

    function handleMouseMove(event: MouseEvent) {
      if (isDragging) {
        const deltaX = event.clientX - mouseX;
        const deltaY = event.clientY - mouseY;

        image.style.left = imgX + deltaX + 'px';
        image.style.top = imgY + deltaY + 'px';
      }
    }

    function handleMouseUp() {
      if (isDragging) {
        isDragging = false;
        image.style.cursor = 'grab';

        document.removeEventListener('mousemove', handleMouseMove);
        document.removeEventListener('mouseup', handleMouseUp);
      }
    }

    image.addEventListener('mousedown', handleMouseDown);

    image.addEventListener('dragstart', (event: DragEvent) => {
      event.preventDefault();
    });

        // Handle mouse leaving window
    window.addEventListener('mouseout', (event: MouseEvent) => {
      if (event.target === document.body || event.target === document.documentElement) {
        handleMouseUp();
      }
    });
  }
}
