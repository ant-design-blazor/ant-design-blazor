export class imageHelper {
    static imgDragAndDrop(element: HTMLImageElement): void {
        if (!element) {
            throw new Error('Element not found.');
        }

        let mouseX: number, mouseY: number, imgX: number, imgY: number;
        let isDragging = false;

        function handleMouseDown(event: MouseEvent) {
            mouseX = event.clientX;
            mouseY = event.clientY;
            imgX = element.offsetLeft;
            imgY = element.offsetTop;
            isDragging = true;

            element.style.cursor = 'grabbing';

            document.addEventListener('mousemove', handleMouseMove);
            document.addEventListener('mouseup', handleMouseUp);
        }

        function handleMouseMove(event: MouseEvent) {
            if (isDragging) {
                const deltaX = event.clientX - mouseX;
                const deltaY = event.clientY - mouseY;

                element.style.left = imgX + deltaX + 'px';
                element.style.top = imgY + deltaY + 'px';
            }
        }

        function handleMouseUp() {
            if (isDragging) {
                isDragging = false;
                element.style.cursor = 'grab';

                document.removeEventListener('mousemove', handleMouseMove);
                document.removeEventListener('mouseup', handleMouseUp);
            }
        }

        element.addEventListener('mousedown', handleMouseDown);

        element.addEventListener('dragstart', (event: DragEvent) => {
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
