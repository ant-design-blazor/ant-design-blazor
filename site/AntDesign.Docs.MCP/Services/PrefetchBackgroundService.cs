using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System.Threading;
using System.Threading.Tasks;

namespace AntDesign.Docs.MCP.Services;

public class PrefetchBackgroundService : BackgroundService
{
    private readonly ComponentService _componentService;
    private readonly DemoService _demoService;
    private readonly ILogger<PrefetchBackgroundService> _logger;

    // NOTE: Keep this in sync with the package id constant in AntDesignTools
    private const string PackageId = "AntDesign.Docs.MCP";

    public PrefetchBackgroundService(ComponentService componentService, DemoService demoService, ILogger<PrefetchBackgroundService> logger)
    {
        _componentService = componentService;
        _demoService = demoService;
        _logger = logger;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        var forceOffline = string.Equals(Environment.GetEnvironmentVariable("ANT_DESIGN_DOCS_FORCE_OFFLINE"), "1", StringComparison.OrdinalIgnoreCase)
                           || string.Equals(Environment.GetEnvironmentVariable("ANT_DESIGN_DOCS_FORCE_OFFLINE"), "true", StringComparison.OrdinalIgnoreCase);

        _logger?.LogInformation("PrefetchBackgroundService starting (forceOffline={forceOffline})", forceOffline);

        // Start component and demo load in parallel
        var compTask = Task.Run(async () =>
        {
            try
            {
                await _componentService.LoadComponentsAsync();
                _logger?.LogInformation("Component data prefetched, {count} components", _componentService.ListComponents().Count());
            }
            catch (Exception ex)
            {
                _logger?.LogWarning(ex, "Failed to prefetch component data");
            }
        }, stoppingToken);

        var demoTask = Task.Run(async () =>
        {
            try
            {
                var demos = await _demoService.LoadDemosAsync();
                _logger?.LogInformation("Demo data prefetched, {count} demos", demos.Count);
            }
            catch (Exception ex)
            {
                _logger?.LogWarning(ex, "Failed to prefetch demo data");
            }
        }, stoppingToken);

        // Start NuGet check only when not forced offline
        Task? nugetTask = null;
        if (!forceOffline)
        {
            nugetTask = Task.Run(async () =>
            {
                try
                {
                    await NuGetService.StartBackgroundCheckAsync(PackageId);
                    _logger?.LogInformation("NuGet background check completed (cached: {latest})", NuGetService.GetCachedLatest());
                }
                catch (Exception ex)
                {
                    _logger?.LogWarning(ex, "NuGet background check failed");
                }
            }, stoppingToken);
        }

        var tasks = new List<Task> { compTask, demoTask };
        if (nugetTask != null) tasks.Add(nugetTask);

        try
        {
            await Task.WhenAll(tasks);
            _logger?.LogInformation("PrefetchBackgroundService initial tasks completed");
        }
        catch (OperationCanceledException) { /* shutdown */ }
    }
}
