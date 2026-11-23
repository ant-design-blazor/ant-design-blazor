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
        },
        setupInteraction: async ({ quick, interaction, rows, columns }) => {
            const rendered = new Promise(resolve => {
                completePendingRender = resolve;
            });
            await dotNetReference.invokeMethodAsync("SetInteractionCase", quick, interaction, rows, columns);
            await rendered;
            await nextFrame();
            await nextFrame();
        },
        runInteraction: async ({ interaction, rows }) => {

            const root = document.querySelector("#benchmark-case");
            const rowCount = () => root?.querySelectorAll("tbody tr.ant-table-row").length ?? 0;
            const waitFor = async (predicate, description) => {
                const deadline = performance.now() + 30_000;
                let stableFrames = 0;
                while (performance.now() < deadline) {
                    if (predicate()) {
                        stableFrames++;
                    } else {
                        stableFrames = 0;
                    }
                    if (stableFrames >= 2) return;
                    await nextFrame();
                }
                throw new Error(`Timed out waiting for ${description} (currently ${rowCount()} rows).`);
            };

            const pageSize = Math.max(1, Math.floor(rows / 2));
            if (interaction === "pagination" && !root.querySelector("tbody tr.ant-table-row")?.textContent?.includes("Person 0")) {
                root.querySelector("li.ant-pagination-prev")?.click();
                await waitFor(() => root.querySelector("tbody tr.ant-table-row")?.textContent?.includes("Person 0") === true, "the first page before pagination measurement");
            } else if (interaction === "selection-one") {
                const checkbox = root.querySelector("tbody input[type=checkbox]");
                if (checkbox?.checked) {
                    checkbox.click();
                    await waitFor(() => root.querySelectorAll("tbody tr.ant-table-row-selected").length === 0, "the row selection to clear before selection measurement");
                }
            } else if (interaction === "selection-all") {
                const headerCheckbox = root.querySelector("thead input[type=checkbox]");
                const rowCheckboxes = [...root.querySelectorAll("tbody input[type=checkbox]")];
                if (rowCheckboxes.length > 0 && rowCheckboxes.every(input => input.checked)) {
                    headerCheckbox?.click();
                    await waitFor(() => root.querySelectorAll("tbody tr.ant-table-row-selected").length === 0, "all row selections to clear before select-all measurement");
                }
            } else if (interaction === "tree-expand" && rowCount() !== 1) {
                root.querySelector(".ant-table-row-expand-icon-expanded")?.click();
                await waitFor(() => rowCount() === 1, "the tree to collapse before expand measurement");
            } else if (interaction === "tree-collapse" && rowCount() === 1) {
                root.querySelector(".ant-table-row-expand-icon-collapsed")?.click();
                await waitFor(() => rowCount() === rows + 1, "tree rows to expand before collapse measurement");
            }

            let predicate;
            let description;
            let action;
            switch (interaction) {
                case "pagination": {
                    const next = root.querySelector("li.ant-pagination-next");
                    if (!next) throw new Error("Pagination control was not rendered.");
                    predicate = () => root.querySelector("tbody tr.ant-table-row")?.textContent?.includes(`Person ${pageSize}`) === true;
                    description = `page ${pageSize} to become visible`;
                    action = () => next.click();
                    break;
                }
                case "selection-one": {
                    const checkbox = root.querySelector("tbody input[type=checkbox]");
                    if (!checkbox) throw new Error("Row selection checkbox was not rendered.");
                    predicate = () => root.querySelector("tbody input[type=checkbox]")?.checked === true
                        && root.querySelectorAll("tbody tr.ant-table-row-selected").length === 1;
                    description = "the selected row checkbox";
                    action = () => checkbox.click();
                    break;
                }
                case "selection-all": {
                    const checkbox = root.querySelector("thead input[type=checkbox]");
                    if (!checkbox) throw new Error("Select-all checkbox was not rendered.");
                    predicate = () => {
                        const rowCheckboxes = [...root.querySelectorAll("tbody input[type=checkbox]")];
                        return rowCheckboxes.length === rows
                            && rowCheckboxes.every(input => input.checked)
                            && root.querySelectorAll("tbody tr.ant-table-row-selected").length === rows;
                    };
                    description = `all ${rows} row checkboxes to become selected`;
                    action = () => checkbox.click();
                    break;
                }
                case "tree-expand": {
                    const button = root.querySelector(".ant-table-row-expand-icon-collapsed");
                    if (!button) throw new Error("Tree expand button was not rendered.");
                    predicate = () => rowCount() === rows + 1;
                    description = `${rows} child rows to render`;
                    action = () => button.click();
                    break;
                }
                case "tree-collapse": {
                    const button = root.querySelector(".ant-table-row-expand-icon-expanded");
                    if (!button) throw new Error("Expanded tree collapse button was not rendered.");
                    predicate = () => rowCount() === 1;
                    description = "tree children to be removed";
                    action = () => button.click();
                    break;
                }
                default:
                    throw new Error(`Unknown interaction benchmark: ${interaction}`);
            }

            const startedAt = performance.now();
            action();
            await waitFor(predicate, description);
            await nextFrame();
            await nextFrame();

            return {
                elapsedMilliseconds: performance.now() - startedAt,
                renderedRows: rowCount()
            };
        }
    };
}

export function renderComplete() {
    completePendingRender?.();
    completePendingRender = undefined;
}
