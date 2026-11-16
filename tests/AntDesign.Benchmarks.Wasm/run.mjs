import { chromium } from "playwright";
import os from "node:os";

const baseUrl = process.argv.find(argument => argument.startsWith("--url="))?.slice("--url=".length)
    ?? process.env.WASM_BENCHMARK_URL
    ?? "http://localhost:8183/en-US/table-benchmark";
const repetitions = Number(process.env.WASM_BENCHMARK_REPETITIONS ?? 3);
const interactionRepetitions = Number(process.env.WASM_INTERACTION_REPETITIONS ?? 3);
const initialOnly = process.argv.includes("--initial-only");
const interactionOnly = process.argv.includes("--interaction-only");
const interactionCaseFilter = process.argv.find(argument => argument.startsWith("--interaction-case="))?.slice("--interaction-case=".length);
const interactionCaseFilters = interactionCaseFilter ? new Set(interactionCaseFilter.split(",")) : null;
const scenarios = [
    { rows: 100, columns: 8, virtualized: false },
    { rows: 100, columns: 16, virtualized: false },
    { rows: 100, columns: 8, virtualized: true },
    { rows: 100, columns: 16, virtualized: true },
    { rows: 1_000, columns: 8, virtualized: false },
    { rows: 1_000, columns: 16, virtualized: false },
    { rows: 1_000, columns: 8, virtualized: true },
    { rows: 1_000, columns: 16, virtualized: true },
    { rows: 10_000, columns: 8, virtualized: true },
    { rows: 10_000, columns: 16, virtualized: true }
];
const interactionScenarios = [
    { interaction: "pagination", rows: 100, columns: 8 },
    { interaction: "pagination", rows: 1_000, columns: 16 },
    { interaction: "selection-one", rows: 100, columns: 8 },
    { interaction: "selection-one", rows: 1_000, columns: 16 },
    { interaction: "selection-all", rows: 100, columns: 8 },
    { interaction: "selection-all", rows: 1_000, columns: 16 },
    { interaction: "tree-expand", rows: 100, columns: 8 },
    { interaction: "tree-expand", rows: 1_000, columns: 16 },
    { interaction: "tree-collapse", rows: 100, columns: 8 },
    { interaction: "tree-collapse", rows: 1_000, columns: 16 }
];

const browser = await chromium.launch({ headless: true });
const page = await browser.newPage({ viewport: { width: 1440, height: 1000 } });
const pageErrors = [];
page.on("pageerror", error => pageErrors.push(error.stack ?? error.message));

try {
    await page.goto(baseUrl, { waitUntil: "domcontentloaded", timeout: 120_000 });
    await page.waitForFunction(() => window.tableBenchmark?.ready === true, null, { timeout: 120_000 });

    console.error(`Browser: Chromium ${browser.version()}`);
    console.error(`Machine: ${os.cpus()[0]?.model ?? os.arch()} (${os.platform()} ${os.release()})`);
    console.error(`Node.js: ${process.version}`);
    console.error(`URL: ${baseUrl}`);
    console.error(`Measured repetitions per mode: ${repetitions}; interaction repetitions per mode: ${interactionRepetitions}; warmups per mode: 1`);

    if (!interactionOnly) {
        console.log("Rows,Columns,Virtualized,Quick,MedianMs,RenderedRows");
        for (const scenario of scenarios) {
            const measurements = { true: [], false: [] };
            const modes = [true, false];

            for (const quick of modes) {
                await page.evaluate(configuration => window.tableBenchmark.run(configuration), { ...scenario, quick });
            }

            for (let iteration = 0; iteration < repetitions; iteration++) {
                const order = iteration % 2 === 0 ? modes : [...modes].reverse();
                for (const quick of order) {
                    const result = await page.evaluate(configuration => window.tableBenchmark.run(configuration), { ...scenario, quick });
                    measurements[String(quick)].push(result);
                }
            }

            for (const quick of modes) {
                const values = measurements[String(quick)].map(result => result.elapsedMilliseconds).sort((left, right) => left - right);
                const renderedRows = measurements[String(quick)].map(result => result.renderedRows).sort((left, right) => left - right);
                const median = values[Math.floor(values.length / 2)];
                const medianRows = renderedRows[Math.floor(renderedRows.length / 2)];
                console.log(`${scenario.rows},${scenario.columns},${scenario.virtualized},${quick},${median.toFixed(2)},${medianRows}`);
            }
        }
    }

    if (!initialOnly) {
        const interactionPage = await browser.newPage({ viewport: { width: 1440, height: 1000 } });
        interactionPage.on("pageerror", error => pageErrors.push(error.stack ?? error.message));
        await interactionPage.goto(baseUrl, { waitUntil: "domcontentloaded", timeout: 120_000 });
        await interactionPage.waitForFunction(() => window.tableBenchmark?.ready === true, null, { timeout: 120_000 });

        const filteredInteractionScenarios = interactionCaseFilters
            ? interactionScenarios.filter(scenario => interactionCaseFilters.has(`${scenario.interaction}:${scenario.rows}:${scenario.columns}`))
            : interactionScenarios;
        if (filteredInteractionScenarios.length === 0) {
            throw new Error(`No interaction benchmark matches --interaction-case=${interactionCaseFilter}.`);
        }
        console.log("Interaction,DataRows,Columns,Quick,MedianMs,RenderedRows");
        for (const scenario of filteredInteractionScenarios) {
            const measurements = { true: [], false: [] };
            const modes = [true, false];
            const pages = { true: page, false: interactionPage };

            for (const quick of modes) {
                const scenarioPage = pages[String(quick)];
                await scenarioPage.evaluate(configuration => window.tableBenchmark.setupInteraction(configuration), { ...scenario, quick });
                await scenarioPage.evaluate(configuration => window.tableBenchmark.runInteraction(configuration), { ...scenario, quick });
            }

            for (let iteration = 0; iteration < interactionRepetitions; iteration++) {
                const order = iteration % 2 === 0 ? modes : [...modes].reverse();
                for (const quick of order) {
                    const scenarioPage = pages[String(quick)];
                    const result = await scenarioPage.evaluate(configuration => window.tableBenchmark.runInteraction(configuration), { ...scenario, quick });
                    measurements[String(quick)].push(result);
                }
            }

            for (const quick of modes) {
                const values = measurements[String(quick)].map(result => result.elapsedMilliseconds).sort((left, right) => left - right);
                const renderedRows = measurements[String(quick)].map(result => result.renderedRows).sort((left, right) => left - right);
                const median = values[Math.floor(values.length / 2)];
                const medianRows = renderedRows[Math.floor(renderedRows.length / 2)];
                console.log(`${scenario.interaction},${scenario.rows},${scenario.columns},${quick},${median.toFixed(2)},${medianRows}`);
            }

            if (scenario.rows >= 1_000) {
                await Promise.all([page.reload({ waitUntil: "domcontentloaded" }), interactionPage.reload({ waitUntil: "domcontentloaded" })]);
                await Promise.all([
                    page.waitForFunction(() => window.tableBenchmark?.ready === true, null, { timeout: 120_000 }),
                    interactionPage.waitForFunction(() => window.tableBenchmark?.ready === true, null, { timeout: 120_000 })
                ]);
            }
        }
    }

    if (pageErrors.length > 0) {
        console.error(`Browser page errors (collected after all benchmark cases completed):\n${pageErrors.join("\n")}`);
    }
} finally {
    await browser.close();
}
