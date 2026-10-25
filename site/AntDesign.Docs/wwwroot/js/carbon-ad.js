// A single vendor-owned slot survives interactive rerenders and SPA navigation.
// In particular, no executable ad script is emitted during server prerendering.
let slot;
let parking;

export function mount(host) {
    if (!host?.isConnected) return;
    if (!slot) {
        slot = document.createElement('div');
        slot.dataset.carbonAdSlot = '';
        host.appendChild(slot);
        const script = document.createElement('script');
        script.id = '_carbonads_js';
        script.async = true;
        script.src = 'https://cdn.carbonads.com/carbon.js?serve=CW7I6K7I&placement=antblazorcom&format=cover';
        slot.appendChild(script);
    } else if (slot.parentElement !== host) {
        host.appendChild(slot);
    }
}

export function unmount(host) {
    // A stale disposal must not detach a slot already adopted by the next page.
    if (!slot || slot.parentElement !== host) return;
    if (!parking) {
        parking = document.createElement('div');
        parking.hidden = true;
        parking.dataset.carbonAdParking = '';
        document.body.appendChild(parking);
    }
    // Keep the slot connected even while the asynchronous vendor script loads.
    // Returning to a docs page reuses it instead of requesting a second ad.
    parking.appendChild(slot);
}
