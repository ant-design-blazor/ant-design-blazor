using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Serilog;
using Serilog.Events;
using Xunit.Abstractions;

namespace AntDesign.TestKit
{
    public static class ServiceCollectionLoggingExtensions
    {
        public static IServiceCollection AddXunitLogger(this IServiceCollection services, ITestOutputHelper outputHelper)
        {
            var serilogLogger = new LoggerConfiguration()
                .MinimumLevel.Verbose()
                .WriteTo.TestOutput(outputHelper, LogEventLevel.Verbose)
                .CreateLogger();

            services.AddSingleton<ILoggerFactory>(new LoggerFactory().AddSerilog(serilogLogger, dispose: true));
            services.AddSingleton(typeof(ILogger<>), typeof(Logger<>));

            return services;
        }
    }
}
