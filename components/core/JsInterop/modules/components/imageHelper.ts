export class imageHelper {
    static initializeDragAndDrop(): void {
        const previewImg = document.getElementById('preview-img') as HTMLDivElement;

        let initialMouseX: number, initialMouseY: number, initialDiv2X: number, initialDiv2Y: number;

        let isDragging = false;

        previewImg.addEventListener('mousedown', (event: MouseEvent) => {
            initialMouseX = event.clientX;
            initialMouseY = event.clientY;
            initialDiv2X = previewImg.offsetLeft;
            initialDiv2Y = previewImg.offsetTop;
            isDragging = true;

            previewImg.style.cursor = 'grabbing';
        });

        previewImg.addEventListener('dragstart', (event: DragEvent) => {
            event.preventDefault();
        });

        document.addEventListener('mousemove', (event: MouseEvent) => {
            if (isDragging) {
                const deltaX = event.clientX - initialMouseX;
                const deltaY = event.clientY - initialMouseY;

                previewImg.style.left = initialDiv2X + deltaX + 'px';
                previewImg.style.top = initialDiv2Y + deltaY + 'px';
            }
        });

        document.addEventListener('mouseup', () => {
            isDragging = false;
            previewImg.style.cursor = 'grab';
        });
    }
}
