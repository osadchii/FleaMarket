using FleaMarket.Data;
using FleaMarket.Infrastructure.Configurations;
using FleaMarket.Infrastructure.ControllerHandlers;
using FleaMarket.Infrastructure.HostedServices;
using FleaMarket.Infrastructure.Services;
using FleaMarket.Infrastructure.Services.MessageSender;
using FleaMarket.Infrastructure.StateHandlers;
using FleaMarket.Infrastructure.Telegram;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Serilog;
using Environment = FleaMarket.Infrastructure.Constants.Environment;

namespace FleaMarket.Api;

public static class FleaMarketConfigurator
{
    public static WebApplicationBuilder WebApplicationBuilder(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        if (builder.Environment.EnvironmentName != Environment.Test)
        {
            ConfigureLogging();
        }

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

        builder.Services.AddHttpClient();

        builder.Services
            .AddDbContext<FleaMarketDatabaseContext>((_, options) =>
            {
                options.UseNpgsql(builder.Configuration.GetConnectionString("Default"),
                    contextBuilder => { contextBuilder.UseQuerySplittingBehavior(QuerySplittingBehavior.SplitQuery); });
            });

        builder.Services
            .AddServices();
        
        builder.Services
            .AddStateHandlers();

        if (builder.Environment.EnvironmentName != Environment.Test)
        {
            builder.Services
                .AddHostedServices();

            builder.Services
                .AddSender(builder.Configuration);
        }

        builder.Services
            .AddMemoryCache();

        builder.Services
            .AddMediatr();

        builder.Services
            .AddFleaMarketTelegramBot();

        builder.Services
            .Configure<ApplicationConfiguration>(builder.Configuration
                .GetSection("Application"));

        builder.Host
            .UseSerilog((context, services, configuration) =>
                    configuration
                        .ReadFrom
                        .Configuration(context.Configuration)
                        .ReadFrom
                        .Services(services)
                        .Enrich
                        .FromLogContext()
                        .WriteTo
#if DEBUG
                        .Console()
#else
                        .Console(new Serilog.Formatting.Compact.RenderedCompactJsonFormatter())
#endif
            );

        return builder;
    }

    public static WebApplication FleaMarketApplication(WebApplicationBuilder builder)
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
        application.UseRouting();
        application.MapControllers();

        return application;
    }

    private static void ConfigureLogging()
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