using Microsoft.AspNetCore.Http.Features;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Serilog;

namespace FleaMarket.Api;

public static class FleaMarketConfigurator
{
    public static WebApplicationBuilder WebApplicationBuilder(string[] args, bool testEnvironment = false)
    {
        ConfigureLogging(testEnvironment);

        var builder = WebApplication.CreateBuilder(args);

        builder.Configuration
            .SetBasePath(AppDomain.CurrentDomain.BaseDirectory)
            .AddJsonFile("appsettings.json", false)
            .AddJsonFile("appsettings.local.json", true)
            .AddCommandLine(args)
            .AddEnvironmentVariables();

        builder.Services
            .AddControllers()
            .AddNewtonsoftJson(options =>
            {
                options.SerializerSettings.Converters.Add(new StringEnumConverter());
                options.SerializerSettings.NullValueHandling = NullValueHandling.Ignore;
                options.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
            });

        builder.Host
            .UseSerilog((context, services, configuration) => configuration
                .ReadFrom
                .Configuration(context.Configuration)
                .ReadFrom
                .Services(services)
                .Enrich
                .FromLogContext()
                .WriteTo
#if !DEBUG
                .Console()
#else
                .Console(new Serilog.Formatting.Compact.RenderedCompactJsonFormatter())
#endif
            );

        return builder;
    }

    public static WebApplication FleaMarketApplication(WebApplicationBuilder builder, bool testEnvironment = false)
    {
        var application = builder.Build();

        application.UseSerilogRequestLogging(options =>
        {
            options.EnrichDiagnosticContext = (diagnosticContext, httpContext) =>
            {
                var endpointFeature = httpContext.Features.Get<IEndpointFeature>();
                var endpointPattern = "";

                if (endpointFeature.Endpoint is RouteEndpoint routeEndpoint)
                {
                    endpointPattern = routeEndpoint.RoutePattern.RawText ?? string.Empty;
                }
                
                diagnosticContext.Set("RequestHost", httpContext.Request.Host.Value);
                diagnosticContext.Set("RequestScheme", httpContext.Request.Scheme);
                diagnosticContext.Set("RoutePattern", endpointPattern);
            };
        });

        application.UseHsts();
        application.UseHttpsRedirection();

        application.UseRouting();

        application.MapControllers();

        return application;
    }

    private static void ConfigureLogging(bool testEnvironment = false)
    {
        var loggerConfiguration = new LoggerConfiguration();
        loggerConfiguration
            .WriteTo
#if DEBUG
            .Console();
#else
            .Console(new Serilog.Formatting.Compact.RenderedCompactJsonFormatter());
#endif

        Log.Logger = loggerConfiguration.CreateBootstrapLogger();
    }
}