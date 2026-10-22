import { downloadHelper } from './downloadHelper';

export class qrCodeHelper {
  private static renders = new WeakMap<HTMLElement, object>();

  static draw(root: HTMLElement, size: number, moduleCount: number, path: string, excavatedPath: string,
    color: string, background: string, icon: string, imageBounds: number[]) {
    const canvas = root.querySelector('canvas');
    if (!canvas) return;
    const render = {};
    qrCodeHelper.renders.set(root, render);
    const ratio = window.devicePixelRatio || 1;
    canvas.width = Math.max(size, 1) * ratio;
    canvas.height = Math.max(size, 1) * ratio;
    const context = canvas.getContext('2d');
    if (!context) return;

    const resolveColor = (value: string, fallback: string) => {
      if (!value) return fallback;
      const sample = document.createElement('span');
      sample.style.color = value;
      sample.style.display = 'none';
      root.appendChild(sample);
      const result = getComputedStyle(sample).color;
      sample.remove();
      return result;
    };
    const foreground = resolveColor(color, getComputedStyle(root).color);
    const bg = resolveColor(background, 'transparent');
    const paint = (image?: HTMLImageElement) => {
      context.setTransform(1, 0, 0, 1, 0, 0);
      context.clearRect(0, 0, canvas.width, canvas.height);
      const scale = canvas.width / moduleCount;
      context.scale(scale, scale);
      context.fillStyle = bg;
      context.fillRect(0, 0, moduleCount, moduleCount);
      context.fillStyle = foreground;
      context.fill(new Path2D(image ? excavatedPath : path));
      if (image) context.drawImage(image, imageBounds[0], imageBounds[1], imageBounds[2], imageBounds[3]);
    };
    paint();
    const image = root.querySelector('img');
    if (icon && image) {
      const onLoad = () => {
        if (qrCodeHelper.renders.get(root) === render && canvas.isConnected) paint(image);
      };
      if (image.complete && image.naturalWidth && image.naturalHeight) onLoad();
      else image.addEventListener('load', onLoad, { once: true });
    }
  }

  static download(root: HTMLElement, fileName?: string) {
    const canvas = root.querySelector('canvas');
    if (canvas) {
      downloadHelper.triggerFileDownload(fileName || 'QRCode.png', canvas.toDataURL('image/png'));
      return;
    }
    const svg = root.querySelector('svg');
    if (!svg) return;
    const exported = svg.cloneNode(true) as SVGElement;
    // Resolve inherited colors so the downloaded SVG is standalone.
    const sourcePaths = svg.querySelectorAll('path');
    exported.querySelectorAll('path').forEach((path, index) => path.setAttribute('fill', getComputedStyle(sourcePaths[index]).fill));
    const url = URL.createObjectURL(new Blob([new XMLSerializer().serializeToString(exported)], { type: 'image/svg+xml;charset=utf-8' }));
    downloadHelper.triggerFileDownload(fileName || 'QRCode.svg', url);
    setTimeout(() => URL.revokeObjectURL(url), 1000);
  }
}
