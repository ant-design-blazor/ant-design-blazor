# Table Quick Rendering Benchmarks

#### Benchmark

The benchmark uses Blazor's `HtmlRenderer` to compare `Quick=true/false`; all columns use `CellRender`. Non-virtualized cases cover 100 and 1,000 rows; virtualized cases cover 100, 1,000, and 10,000 rows. Each table has 8 or 16 columns. Virtualized cases simulate a 400px viewport and render 28 rows including overscan. Each benchmark invocation has one warmup, then takes the median of 7 runs (100 rows), 5 runs (1,000 rows), or 3 runs (10,000 rows). The table reports the median across three Release invocations. Results were collected on an Apple M4 Pro running macOS 27.0.1, with .NET SDK 11.0.100 and .NET runtime 11.0.0:

| Rows | Columns | Virtualized | Rows rendered | Quick=true time / allocations | Quick=false time / allocations | Time / allocation reduction |
| ---: | ---: | :---: | ---: | ---: | ---: | ---: |
| 100 | 8 | No | 100 | 21.59 ms / 4,394,640 B | 84.88 ms / 19,761,656 B | 74.6% / 77.8% |
| 100 | 16 | No | 100 | 40.62 ms / 6,244,304 B | 179.26 ms / 36,721,344 B | 77.3% / 83.0% |
| 100 | 8 | Yes | 28 | 3.65 ms / 738,376 B | 16.50 ms / 4,066,952 B | 77.9% / 81.8% |
| 100 | 16 | Yes | 28 | 4.04 ms / 1,084,848 B | 28.83 ms / 8,035,736 B | 86.0% / 86.5% |
| 1,000 | 8 | No | 1,000 | 183.21 ms / 48,614,608 B | 797.77 ms / 198,916,672 B | 77.0% / 75.6% |
| 1,000 | 16 | No | 1,000 | 130.88 ms / 69,913,560 B | 1,364.42 ms / 374,045,992 B | 90.4% / 81.3% |
| 1,000 | 8 | Yes | 28 | 2.83 ms / 1,086,104 B | 14.44 ms / 4,431,784 B | 80.4% / 75.5% |
| 1,000 | 16 | Yes | 28 | 3.63 ms / 1,415,680 B | 23.18 ms / 8,385,944 B | 84.3% / 83.1% |
| 10,000 | 8 | Yes | 28 | 12.91 ms / 5,162,768 B | 27.36 ms / 8,503,520 B | 52.8% / 39.3% |
| 10,000 | 16 | Yes | 28 | 14.11 ms / 5,490,792 B | 37.40 ms / 12,450,536 B | 62.3% / 55.9% |

These are component-rendering microbenchmark results; they exclude browser layout, painting, and network time. Virtualization simulates a 400px viewport by invoking the framework's viewport-fill callback, so browser JavaScript and scrolling performance are not measured. The 10,000-row cases only use virtualization to avoid the cost of rendering every cell. Virtualization renders only 28 rows here, but the table still processes the full data source, so allocations increase with 10,000 rows. Timings are sensitive to machine load; compare the Quick modes within the same scenario. Rerun with:

```bash
dotnet run --project tests/AntDesign.Benchmarks/AntDesign.Benchmarks.csproj --configuration Release --framework net11.0
```

#### WASM browser benchmark

This benchmark renders the table in a Release Blazor WebAssembly app using Chromium. Timing starts when the browser invokes the benchmark page and ends after the table renders, two browser frames pass, and the row count stabilizes. It includes JS interop, WASM rendering, and browser layout/paint waiting; it excludes WASM startup, data loading, and network time. Each Quick mode is warmed up once, then measured three times in alternating order; the median is reported. Virtualized scenarios use a real 400px table viewport and render 30 rows. This benchmark reports end-to-end UI latency, not memory allocations.

Results were collected on an Apple M4 Pro running macOS 27.0.0, Chromium 159.0.8154.6, and Node.js 24.14.0, using .NET SDK 11.0.100 / runtime 11.0.0 and Release WASM with AOT disabled:

| Rows | Columns | Virtualized | Rows rendered | Quick=true | Quick=false | Time reduction |
| ---: | ---: | :---: | ---: | ---: | ---: | ---: |
| 100 | 8 | No | 100 | 264.40 ms | 864.80 ms | 69.4% |
| 100 | 16 | No | 100 | 396.40 ms | 1,600.90 ms | 75.2% |
| 100 | 8 | Yes | 30 | 66.10 ms | 215.20 ms | 69.3% |
| 100 | 16 | Yes | 30 | 114.10 ms | 349.00 ms | 67.3% |
| 1,000 | 8 | No | 1,000 | 2,434.10 ms | 8,598.80 ms | 71.7% |
| 1,000 | 16 | No | 1,000 | 3,841.50 ms | 15,834.50 ms | 75.7% |
| 1,000 | 8 | Yes | 30 | 131.80 ms | 294.80 ms | 55.3% |
| 1,000 | 16 | Yes | 30 | 181.40 ms | 514.10 ms | 64.7% |
| 10,000 | 8 | Yes | 30 | 241.60 ms | 378.90 ms | 36.2% |
| 10,000 | 16 | Yes | 30 | 199.50 ms | 514.70 ms | 61.2% |

Timings vary with the device, browser version, and system load. Use these numbers to compare Quick modes within the same environment, rather than as fixed performance figures for every device. Rerun with:

```bash
ASPNETCORE_ENVIRONMENT=Production DOTNET_ENVIRONMENT=Production dotnet run --project site/AntDesign.Docs.Wasm/AntDesign.Docs.Wasm.csproj --configuration Release --framework net11.0 --no-launch-profile --urls http://localhost:8183
cd tests/AntDesign.Benchmarks.Wasm
npm ci
npx playwright install chromium
npm run benchmark -- --url=http://localhost:8183/en-US/table-benchmark
```

#### WASM interaction benchmark

The browser benchmark also measures interactions that trigger table updates. It tests moving to the next page, selecting one row, selecting all rows on the current page, and expanding or collapsing a tree node. The 100-row cases use 8 columns; the 1,000-row cases use 16 columns. Pagination uses half the data as the page size. Selection cases render all rows. Tree cases use one root with the configured number of children, so expand renders 101 or 1,001 rows and collapse removes the children.

The timer starts immediately before the browser action and stops after the expected page, selection state, or row count is stable for two frames. It includes event dispatch, WebAssembly component rendering, DOM updates, and two browser frames. It excludes WASM startup and table setup; each case is prepared before measurement. Each Quick mode has one warmup and three measured interactions, with the mode order alternated. The median is reported. Large cases reload both browser pages between scenarios to avoid retaining their table state.

Results were collected on an Apple M4 Pro running macOS 27.0.0, Chromium 159.0.8154.6, and Node.js 24.14.0, using .NET SDK 11.0.100 / runtime 11.0.0 and Release WASM with AOT disabled:

| Interaction | Data rows | Columns | Rows rendered after action | Quick=true | Quick=false | Time reduction |
| --- | ---: | ---: | ---: | ---: | ---: | ---: |
| Next page | 100 | 8 | 50 | 154.40 ms | 557.10 ms | 72.3% |
| Next page | 1,000 | 16 | 500 | 2,404.10 ms | 10,814.20 ms | 77.8% |
| Select one row | 100 | 8 | 100 | 375.10 ms | 924.30 ms | 59.4% |
| Select one row | 1,000 | 16 | 1,000 | 7,102.10 ms | 20,000.60 ms | 64.5% |
| Select all rows | 100 | 8 | 100 | 519.00 ms | 1,217.60 ms | 57.4% |
| Select all rows | 1,000 | 16 | 1,000 | 8,035.20 ms | 22,694.50 ms | 64.6% |
| Expand tree | 100 | 8 | 101 | 133.80 ms | 529.10 ms | 74.7% |
| Expand tree | 1,000 | 16 | 1,001 | 2,028.90 ms | 9,985.00 ms | 79.7% |
| Collapse tree | 100 | 8 | 1 | 31.90 ms | 44.30 ms | 28.0% |
| Collapse tree | 1,000 | 16 | 1 | 104.70 ms | 362.00 ms | 71.1% |

To run only the interaction cases, start the Release WASM app as above, then run:

```bash
npm run benchmark -- --url=http://localhost:8183/en-US/table-benchmark --interaction-only
```
