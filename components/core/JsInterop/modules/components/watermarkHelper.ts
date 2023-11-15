export class watermarkHelper {
    static generateBase64Url({
        width,
        height,
        gapX,
        gapY,
        offsetLeft,
        offsetTop,
        rotate,
        alpha,
        watermarkContent,
        lineSpace,
    }: {
        width: number,
        height: number,
        gapX: number,
        gapY: number,
        offsetLeft: number,
        offsetTop: number,
        rotate: number,
        alpha: number,
        watermarkContent: WatermarkText | WatermarkImage | Array<WatermarkText | WatermarkImage>,
        lineSpace: number
    }, dotnetRef, watermarkContentRef: HTMLElement, watermarkRef: HTMLElement): string {
        const onFinish = (url: string) => {
            dotnetRef.invokeMethodAsync("load", url);
        }

        const canvas = document.createElement('canvas');
        const ctx = canvas.getContext('2d');
        if (!ctx) {
            // eslint-disable-next-line no-console
            console.warn('Current environment does not support Canvas, cannot draw watermarks.');
            onFinish('');
            return;
        }
        const ratio = window.devicePixelRatio || 1;
        const canvasWidth = (gapX + width) * ratio;
        const canvasHeight = (gapY + height) * ratio;

        canvas.width = canvasWidth;
        canvas.height = canvasHeight;
        canvas.style.width = `${gapX + width}px`;
        canvas.style.height = `${gapY + height}px`;

        ctx.translate(offsetLeft * ratio, offsetTop * ratio);
        ctx.rotate((Math.PI / 180) * Number(rotate));
        ctx.globalAlpha = alpha;

        const markWidth = width * ratio;
        const markHeight = height * ratio;

        ctx.fillStyle = 'transparent';
        ctx.fillRect(0, 0, markWidth, markHeight);

        const contents = Array.isArray(watermarkContent) ? watermarkContent : [{ ...watermarkContent }];
        let top = 0;
        contents.forEach((item: WatermarkText & WatermarkImage & { top: number }) => {
            if (item.url) {
                const { url, isGrayscale = false } = item;
                // eslint-disable-next-line no-param-reassign
                item.top = top;
                top += height;
                const img = new Image();
                img.crossOrigin = 'anonymous';
                img.referrerPolicy = 'no-referrer';
                img.src = url;
                img.onload = () => {
                    // ctx.filter = 'grayscale(1)';
                    ctx.drawImage(img, 0, item.top * ratio, width * ratio, height * ratio);
                    if (isGrayscale) {
                        const imgData = ctx.getImageData(0, 0, ctx.canvas.width, ctx.canvas.height);
                        const pixels = imgData.data;
                        for (let i = 0; i < pixels.length; i += 4) {
                            const lightness = (pixels[i] + pixels[i + 1] + pixels[i + 2]) / 3;
                            pixels[i] = lightness;
                            pixels[i + 1] = lightness;
                            pixels[i + 2] = lightness;
                        }
                        ctx.putImageData(imgData, 0, 0);
                    }
                    onFinish(canvas.toDataURL());
                };
            } else if (item.text) {
                const {
                    text,
                    fontColor = 'rgba(0, 0, 0, 0.1)',
                    fontSize = 16,
                    fontFamily = undefined,
                    fontWeight = 'normal',
                    textAlign = 'start',
                    fontStyle = 'normal'
                } = item;
                // eslint-disable-next-line no-param-reassign
                item.top = top;
                top += lineSpace;
                const markSize = Number(fontSize) * ratio;

                ctx.font = `${fontStyle} normal ${fontWeight} ${markSize}px/${markHeight}px ${fontFamily}`;
                ctx.textAlign = textAlign;
                ctx.textBaseline = 'top';
                ctx.fillStyle = fontColor;
                ctx.fillText(text, 0, item.top * ratio);
            }
        });
        onFinish(canvas.toDataURL());

        const parent = watermarkRef.parentElement;

        const observer = new MutationObserver((mutationsList, observer) => {
            mutationsList.forEach((mutation) => {
                if (mutation.type === 'childList') {
                    const removeNodes = mutation.removedNodes;
                    removeNodes.forEach((node) => {
                        const element = node as HTMLElement;
                        if (element === watermarkRef) {
                            parent.appendChild(element);
                        }
                        if (element === watermarkContentRef) {
                            watermarkRef.appendChild(element);
                        }
                    });
                }
            });
        });

        observer.observe(parent, {
            attributes: true,
            childList: true,
            characterData: true,
            subtree: true,
        });

        watermarkRef['_observer'] = observer;
    }
}

export interface WatermarkText {
    /**
     * 水印文本文字颜色
     * @default rgba(0,0,0,0.1)
     */
    fontColor?: string;
    /**
     * 水印文本文字大小
     * @default 16
     */
    fontSize?: number;
    /**
     * 水印文本文字样式
     * @default undefined
     */
    fontFamily?: string | undefined;
    /**
     * 水印文本文字粗细
     * @default normal
     */
    fontWeight?: 'normal' | 'lighter' | 'bold' | 'bolder';
    /**
     * 水印文本内容
     * @default ''
     */
    text?: string;

    textAlign: 'start' | 'end';

    fontStyle?: 'normal' | 'italic' | 'oblique';
}

export interface WatermarkImage {
    /**
     * 水印图片是否需要灰阶显示
     * @default false
     */
    isGrayscale?: boolean;
    /**
     * 水印图片源地址，为了显示清楚，建议导出 2 倍或 3 倍图
     * @default ''
     */
    url?: string;
}