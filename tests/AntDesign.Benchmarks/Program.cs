using System.Diagnostics;
using System.Linq.Expressions;
using System.Reflection;
using AntDesign;
using AntDesign.TableModels;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.Web.Virtualization;
using Microsoft.JSInterop;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

var selectors = new Expression<Func<BenchmarkRow, string>>[]
{
    row => row.Name,
    row => row.Department,
    row => row.City,
    row => row.Email,
    row => row.Role,
    row => row.Status,
    row => row.Region,
    row => row.Description,
    row => row.Category,
    row => row.Phone,
    row => row.Country,
    row => row.Code,
    row => row.Owner,
    row => row.Team,
    row => row.Notes,
    row => row.Updated
};

var scenarios = new[]
{
    (Rows: 100, Columns: 8, Virtualized: false),
    (Rows: 100, Columns: 16, Virtualized: false),
    (Rows: 100, Columns: 8, Virtualized: true),
    (Rows: 100, Columns: 16, Virtualized: true),
    (Rows: 1_000, Columns: 8, Virtualized: false),
    (Rows: 1_000, Columns: 16, Virtualized: false),
    (Rows: 1_000, Columns: 8, Virtualized: true),
    (Rows: 1_000, Columns: 16, Virtualized: true),
    (Rows: 10_000, Columns: 8, Virtualized: true),
    (Rows: 10_000, Columns: 16, Virtualized: true)
};

Console.WriteLine("Table initial-render benchmark (static HtmlRenderer; virtualized cases simulate a 400px viewport)");
Console.WriteLine("Rows,Columns,Virtualized,Quick,MedianMs,MedianAllocatedBytes,RenderedRows");

foreach (var scenario in scenarios)
{
    var rows = CreateRows(scenario.Rows);
    var columns = selectors.Take(scenario.Columns).ToArray();
    var repetitions = scenario.Rows >= 10_000 ? 3 : scenario.Rows >= 1_000 ? 5 : 7;
    var quickMeasurements = new List<Measurement>();
    var standardMeasurements = new List<Measurement>();

    for (var warmup = 0; warmup < 1; warmup++)
    {
        await Measure(quick: true, scenario.Virtualized, rows, columns);
        await Measure(quick: false, scenario.Virtualized, rows, columns);
    }

    for (var iteration = 0; iteration < repetitions; iteration++)
    {
        if (iteration % 2 == 0)
        {
            quickMeasurements.Add(await Measure(quick: true, scenario.Virtualized, rows, columns));
            standardMeasurements.Add(await Measure(quick: false, scenario.Virtualized, rows, columns));
        }
        else
        {
            standardMeasurements.Add(await Measure(quick: false, scenario.Virtualized, rows, columns));
            quickMeasurements.Add(await Measure(quick: true, scenario.Virtualized, rows, columns));
        }
    }

    var quickResult = Median(quickMeasurements);
    var standardResult = Median(standardMeasurements);
    Console.WriteLine($"{scenario.Rows},{scenario.Columns},{scenario.Virtualized},true,{quickResult.ElapsedMilliseconds:F2},{quickResult.AllocatedBytes},{quickResult.RenderedRows}");
    Console.WriteLine($"{scenario.Rows},{scenario.Columns},{scenario.Virtualized},false,{standardResult.ElapsedMilliseconds:F2},{standardResult.AllocatedBytes},{standardResult.RenderedRows}");
    Console.WriteLine($"  Quick reductions: time {(1 - quickResult.ElapsedMilliseconds / standardResult.ElapsedMilliseconds):P1}, allocations {(1 - (double)quickResult.AllocatedBytes / standardResult.AllocatedBytes):P1}");
}

static BenchmarkRow[] CreateRows(int count) => Enumerable.Range(0, count)
    .Select(index => new BenchmarkRow
    {
        Id = index,
        Name = $"Person {index}",
        Department = $"Department {index % 10}",
        City = $"City {index % 20}",
        Email = $"person{index}@example.test",
        Role = $"Role {index % 5}",
        Status = index % 2 == 0 ? "Active" : "Inactive",
        Region = $"Region {index % 4}",
        Description = $"Benchmark row {index}",
        Category = $"Category {index % 6}",
        Phone = $"555-{index:0000}",
        Country = $"Country {index % 3}",
        Code = $"C{index:0000}",
        Owner = $"Owner {index % 7}",
        Team = $"Team {index % 5}",
        Notes = $"Notes for row {index}",
        Updated = $"2026-11-{index % 28 + 1:00}"
    })
    .ToArray();

static Measurement Median(IReadOnlyCollection<Measurement> measurements)
{
    var elapsed = measurements.Select(measurement => measurement.ElapsedMilliseconds).Order().ElementAt(measurements.Count / 2);
    var allocated = measurements.Select(measurement => measurement.AllocatedBytes).Order().ElementAt(measurements.Count / 2);
    var rows = measurements.Select(measurement => measurement.RenderedRows).Order().ElementAt(measurements.Count / 2);
    return new Measurement(elapsed, allocated, rows);
}

static async Task<Measurement> Measure(
    bool quick,
    bool virtualized,
    BenchmarkRow[] rows,
    Expression<Func<BenchmarkRow, string>>[] selectors)
{
    var services = new ServiceCollection();
    services.AddLogging();
    services.AddAntDesign();
    services.AddSingleton<IJSRuntime, NoOpJsRuntime>();
    services.AddSingleton<IComponentActivator>(serviceProvider => new TrackingComponentActivator(serviceProvider));
    await using var serviceProvider = services.BuildServiceProvider();
    var componentActivator = (TrackingComponentActivator)serviceProvider.GetRequiredService<IComponentActivator>();
    await using var renderer = new HtmlRenderer(serviceProvider, serviceProvider.GetRequiredService<ILoggerFactory>());

    RenderFragment<BenchmarkRow> columnDefinitions = _ => builder =>
    {
        for (var columnIndex = 0; columnIndex < selectors.Length; columnIndex++)
        {
            builder.OpenComponent<PropertyColumn<BenchmarkRow, string>>(0);
            builder.AddAttribute(1, nameof(PropertyColumn<BenchmarkRow, string>.Title), $"Column {columnIndex + 1}");
            builder.AddAttribute(2, nameof(PropertyColumn<BenchmarkRow, string>.Property), selectors[columnIndex]);
            builder.AddAttribute(3, nameof(Column<string>.CellRender), (RenderFragment<CellData<string>>)(cell => contentBuilder => contentBuilder.AddContent(0, cell.FieldValue)));
            builder.CloseComponent();
        }
    };

    var parameters = ParameterView.FromDictionary(new Dictionary<string, object?>
    {
        [nameof(Table<BenchmarkRow>.DataSource)] = rows,
        [nameof(Table<BenchmarkRow>.HidePagination)] = true,
        [nameof(Table<BenchmarkRow>.PageSize)] = rows.Length,
        [nameof(Table<BenchmarkRow>.Quick)] = quick,
        [nameof(Table<BenchmarkRow>.EnableVirtualization)] = virtualized,
        [nameof(Table<BenchmarkRow>.ScrollY)] = virtualized ? "400px" : null,
        [nameof(Table<BenchmarkRow>.ChildContent)] = columnDefinitions
    });

    var allocatedBefore = GC.GetTotalAllocatedBytes(precise: true);
    var stopwatch = Stopwatch.StartNew();
    var root = await renderer.Dispatcher.InvokeAsync(() => renderer.RenderComponentAsync<Table<BenchmarkRow>>(parameters));
    if (virtualized)
    {
        var virtualize = componentActivator.CreatedComponents.OfType<Virtualize<RowData<BenchmarkRow>>>().Single();
        var beforeSpacerVisible = virtualize.GetType().GetMethods(BindingFlags.Instance | BindingFlags.NonPublic)
            .Single(method => method.Name.Contains("IVirtualizeJsCallbacks.OnBeforeSpacerVisible", StringComparison.Ordinal));
        await renderer.Dispatcher.InvokeAsync(async () =>
        {
            var reason = Enum.Parse(beforeSpacerVisible.GetParameters()[3].ParameterType, "ViewportFill");
            var callbackResult = beforeSpacerVisible.Invoke(virtualize, new object[] { 0f, 0f, 400f, reason });
            if (callbackResult is Task callbackTask)
            {
                await callbackTask;
            }
            else if (callbackResult is ValueTask callbackValueTask)
            {
                await callbackValueTask;
            }
        });
    }
    stopwatch.Stop();
    var allocatedBytes = GC.GetTotalAllocatedBytes(precise: true) - allocatedBefore;
    var markup = await renderer.Dispatcher.InvokeAsync(root.ToHtmlString);
    var renderedRows = CountDataRows(markup);

    return new Measurement(stopwatch.Elapsed.TotalMilliseconds, allocatedBytes, renderedRows);
}

static int CountDataRows(string markup)
{
    const string rowClass = "ant-table-row ant-table-row-level-";
    var count = 0;
    var offset = 0;
    while ((offset = markup.IndexOf(rowClass, offset, StringComparison.Ordinal)) >= 0)
    {
        count++;
        offset += rowClass.Length;
    }

    return count;
}

internal sealed class BenchmarkRow
{
    public int Id { get; init; }
    public string Name { get; init; } = string.Empty;
    public string Department { get; init; } = string.Empty;
    public string City { get; init; } = string.Empty;
    public string Email { get; init; } = string.Empty;
    public string Role { get; init; } = string.Empty;
    public string Status { get; init; } = string.Empty;
    public string Region { get; init; } = string.Empty;
    public string Description { get; init; } = string.Empty;
    public string Category { get; init; } = string.Empty;
    public string Phone { get; init; } = string.Empty;
    public string Country { get; init; } = string.Empty;
    public string Code { get; init; } = string.Empty;
    public string Owner { get; init; } = string.Empty;
    public string Team { get; init; } = string.Empty;
    public string Notes { get; init; } = string.Empty;
    public string Updated { get; init; } = string.Empty;
}

internal sealed class NoOpJsRuntime : IJSRuntime
{
    public ValueTask<TValue> InvokeAsync<TValue>(string identifier, object?[]? args) => ValueTask.FromResult(default(TValue)!);

    public ValueTask<TValue> InvokeAsync<TValue>(string identifier, CancellationToken cancellationToken, object?[]? args) => ValueTask.FromResult(default(TValue)!);
}

internal sealed class TrackingComponentActivator(IServiceProvider services) : IComponentActivator
{
    public List<IComponent> CreatedComponents { get; } = new();

    public IComponent CreateInstance(Type componentType)
    {
        var component = (IComponent)ActivatorUtilities.CreateInstance(services, componentType);
        CreatedComponents.Add(component);
        return component;
    }
}

internal readonly record struct Measurement(double ElapsedMilliseconds, long AllocatedBytes, int RenderedRows);
