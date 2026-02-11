// Caution! Be sure you understand the caveats before publishing an application with
// offline support. See https://aka.ms/blazor-offline-considerations

self.importScripts('./service-worker-assets.js');
self.addEventListener('install', event => event.waitUntil(onInstall(event)));
self.addEventListener('activate', event => event.waitUntil(onActivate(event)));
self.addEventListener('fetch', event => event.respondWith(onFetch(event)));
self.addEventListener('message', event => onMessage(event));

const cacheNamePrefix = 'offline-cache-{version}';
const cacheName = `${cacheNamePrefix}${self.assetsManifest.version}`;
const offlineAssetsInclude = [ /\.dll$/, /\.pdb$/, /\.wasm/, /\.html/, /\.js$/, /\.json$/, /\.css$/, /\.woff$/, /\.png$/, /\.jpe?g$/, /\.gif$/, /\.ico$/ ];
const offlineAssetsExclude = [ /^service-worker\.js$/, /^version\.json$/ ];

async function onInstall(event) {
    console.info('Service worker: Install');

    // Fetch and cache all matching items from the assets manifest
    const assetsRequests = self.assetsManifest.assets
        .filter(asset => offlineAssetsInclude.some(pattern => pattern.test(asset.url)))
        .filter(asset => !offlineAssetsExclude.some(pattern => pattern.test(asset.url)))
        .map(asset => {
            // Avoid setting integrity for HTML files or external URLs because they may be altered or redirected
            const init = {};
            const url = asset.url;
            const isHtml = /\.html$/i.test(url);
            const isExternal = /^https?:\/\//i.test(url);
            if (asset.hash && !isHtml && !isExternal) {
                init.integrity = asset.hash;
            }
            return new Request(url, init);
        });

    const cache = await caches.open(cacheName);
    // Cache each asset individually and ignore failures so install can complete
    await Promise.all(assetsRequests.map(async (request) => {
        try {
            const response = await fetch(request);
            if (!response.ok) throw new Error('Request failed with status ' + response.status);
            await cache.put(request, response);
        } catch (err) {
            console.warn('Service worker: Failed to cache', request.url, err);
        }
    }));
}

async function onActivate(event) {
    console.info('Service worker: Activate');

    // Delete unused caches
    const cacheKeys = await caches.keys();
    await Promise.all(cacheKeys
        .filter(key => key.startsWith(cacheNamePrefix) && key !== cacheName)
        .map(key => caches.delete(key)));
}

async function onFetch(event) {
    let cachedResponse = null;
    if (event.request.method === 'GET') {
        // For all navigation requests, try to serve index.html from cache
        // If you need some URLs to be server-rendered, edit the following check to exclude those URLs
        const shouldServeIndexHtml = event.request.mode === 'navigate';

        const request = shouldServeIndexHtml ? 'index.html' : event.request;
        const cache = await caches.open(cacheName);
        cachedResponse = await cache.match(request);
    }

    return cachedResponse || fetch(event.request);
}

function onMessage(event) {
    if (event.data.action === 'skipWaiting') {
        self.skipWaiting();
    }
}
