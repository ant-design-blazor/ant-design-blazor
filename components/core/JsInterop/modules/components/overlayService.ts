/**
 * Overlay Service JS Module
 * Provides positioning support for dynamically rendered overlays
 */

/**
 * Position an overlay element at the specified coordinates
 * @param {string} elementId - The ID of the overlay element to position
 * @param {number} x - X coordinate (left position in pixels)
 * @param {number} y - Y coordinate (top position in pixels)  
 * @param {string} container - Container selector (e.g., "body", "#myContainer")
 */
export function positionOverlay(elementId: string, x: number, y: number, container: string = "body") {
    const element = document.getElementById(elementId);
    
    if (!element) {
        console.warn(`positionOverlay: element with id "${elementId}" not found`);
        return;
    }

    try {
        // Get the container element
        let containerElement = document.body;
        if (container !== "body") {
            const found = document.querySelector(container);
            if (found) {
                containerElement = found as HTMLElement;
            } else {
                console.warn(`positionOverlay: container "${container}" not found, using body`);
            }
        }

        // Move element to container if not already there
        if (element.parentElement !== containerElement) {
            containerElement.appendChild(element);
        }

        // Apply positioning styles
        element.style.position = 'fixed';
        element.style.left = `${x}px`;
        element.style.top = `${y}px`;
        element.style.display = 'block';
        element.style.zIndex = String(getMaxZIndex() + 1);

        // Ensure overlay stays within viewport
        adjustToViewport(element);

    } catch (error) {
        console.error('Error in positionOverlay:', error);
    }
}

/**
 * Adjust overlay position to ensure it stays within viewport
 * @param {HTMLElement} element - The overlay element to adjust
 */
function adjustToViewport(element) {
    if (!element) return;

    const rect = element.getBoundingClientRect();
    const viewportWidth = window.innerWidth || document.documentElement.clientWidth;
    const viewportHeight = window.innerHeight || document.documentElement.clientHeight;

    let left = parseFloat(element.style.left) || 0;
    let top = parseFloat(element.style.top) || 0;

    // Adjust if overflowing right
    if (rect.right > viewportWidth) {
        left = Math.max(0, viewportWidth - rect.width);
        element.style.left = `${left}px`;
    }

    // Adjust if overflowing bottom
    if (rect.bottom > viewportHeight) {
        top = Math.max(0, viewportHeight - rect.height);
        element.style.top = `${top}px`;
    }

    // Adjust if overflowing left
    if (rect.left < 0) {
        element.style.left = '0px';
    }

    // Adjust if overflowing top
    if (rect.top < 0) {
        element.style.top = '0px';
    }
}

/**
 * Get the maximum z-index currently in use
 * @returns {number} Maximum z-index value
 */
function getMaxZIndex() {
    // querySelectorAll returns a NodeList; convert to Array to satisfy older TS configs
    const elements = Array.from(document.querySelectorAll('*')) as Element[];
    let maxZIndex = 1000;

    for (const element of elements) {
        const zIndexRaw = window.getComputedStyle(element).zIndex;
        const zIndex = parseInt(zIndexRaw || '0', 10);
        if (!isNaN(zIndex) && zIndex > maxZIndex) {
            maxZIndex = zIndex;
        }
    }

    return maxZIndex;
}

/**
 * Remove an overlay element from the DOM
 * @param {HTMLElement} element - The overlay element to remove
 */
export function removeOverlay(element) {
    if (element && element.parentElement) {
        element.parentElement.removeChild(element);
    }
}

/**
 * Hide an overlay element without removing it
 * @param {HTMLElement} element - The overlay element to hide
 */
export function hideOverlay(element) {
    if (element) {
        element.style.display = 'none';
    }
}

/**
 * Show a hidden overlay element
 * @param {HTMLElement} element - The overlay element to show
 */
export function showOverlay(element) {
    if (element) {
        element.style.display = 'block';
    }
}
