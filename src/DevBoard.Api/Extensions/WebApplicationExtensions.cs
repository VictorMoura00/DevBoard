using DevBoard.Modules.Notifications;
using DevBoard.Modules.Projects;
using DevBoard.Modules.Tasks;
using Scalar.AspNetCore;

namespace DevBoard.Api.Extensions;

public static class WebApplicationExtensions
{
    public static WebApplication ConfigurePipeline(this WebApplication app)
    {
        ArgumentNullException.ThrowIfNull(app);

        if (app.Environment.IsDevelopment())
        {
            app.MapOpenApi();
            app.MapScalarApiReference("/docs", options =>
            {
                options.WithTitle("DevBoard API")
                    .WithClassicLayout();
            });
        }

        if (!IsRunningInContainer())
        {
            app.UseHttpsRedirection();
        }

        return app;
    }

    public static WebApplication MapApiEndpoints(this WebApplication app)
    {
        ArgumentNullException.ThrowIfNull(app);

        app.MapHealthChecks("/health");

        var api = app.MapGroup("/api");
        api.MapProjectsModule();
        api.MapTasksModule();
        api.MapNotificationsModule();

        return app;
    }

    private static bool IsRunningInContainer()
    {
        return string.Equals(
            Environment.GetEnvironmentVariable("DOTNET_RUNNING_IN_CONTAINER"),
            "true",
            StringComparison.OrdinalIgnoreCase);
    }
}
