import { chromium } from "playwright";
import os from "node:os";

const baseUrl = process.argv.find(argument => argument.startsWith("--url="))?.slice("--url=".length)
    ?? process.env.WASM_BENCHMARK_URL
    ?? "http://localhost:8183/en-US/table-benchmark";
const repetitions = Number(process.env.WASM_BENCHMARK_REPETITIONS ?? 3);
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
    console.error(`Measured repetitions per mode: ${repetitions}; warmups per mode: 1`);
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

    if (pageErrors.length > 0) {
        console.error(`Browser page errors (collected after all benchmark cases completed):\n${pageErrors.join("\n")}`);
    }
} finally {
    await browser.close();
}
