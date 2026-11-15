let dotNetReference;
let completePendingRender;

const nextFrame = () => new Promise(resolve => requestAnimationFrame(() => resolve()));

export function initialize(reference) {
    dotNetReference = reference;
    window.tableBenchmark = {
        ready: true,
        run: async ({ quick, rows, columns, virtualized }) => {
            const rendered = new Promise(resolve => {
                completePendingRender = resolve;
            });
            const startedAt = performance.now();

            await dotNetReference.invokeMethodAsync("SetCase", quick, rows, columns, virtualized);
            await rendered;
            await nextFrame();
            await nextFrame();

            const root = document.querySelector("#benchmark-case");
            const expectedRows = virtualized ? 1 : rows;
            let renderedRows = 0;
            let previousRows = -1;
            let stableFrames = 0;
            const deadline = performance.now() + 10000;

            while (performance.now() < deadline) {
                renderedRows = root?.querySelectorAll("tbody tr.ant-table-row-level-0").length ?? 0;
                if (renderedRows === previousRows && renderedRows >= expectedRows) {
                    stableFrames++;
                } else {
                    stableFrames = 0;
                }
                if (stableFrames >= 2) {
                    break;
                }
                previousRows = renderedRows;
                await nextFrame();
            }

            if (renderedRows < expectedRows) {
                throw new Error(`Expected at least ${expectedRows} rows, but rendered ${renderedRows}.`);
            }

            return {
                elapsedMilliseconds: performance.now() - startedAt,
                renderedRows
            };
        }
    };
}

export function renderComplete() {
    completePendingRender?.();
    completePendingRender = undefined;
}
