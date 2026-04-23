using DevBoard.Api.Abstractions.Routing;
using DevBoard.Modules.Projects.Commands;
using DevBoard.Modules.Projects.Endpoints;
using DevBoard.Modules.Projects.Handlers;
using DevBoard.Modules.Projects.Models;
using DevBoard.Modules.Projects.Queries;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace DevBoard.Modules.Projects;

public sealed class ProjectsModule : IApiModule
{
    public void AddServices(IServiceCollection services, IConfiguration configuration)
    {
        ArgumentNullException.ThrowIfNull(services);
        ArgumentNullException.ThrowIfNull(configuration);

        services.AddScoped<CreateProjectHandler>();
        services.AddScoped<UpdateProjectHandler>();
        services.AddScoped<ArchiveProjectHandler>();
        services.AddScoped<ListProjectsHandler>();
        services.AddScoped<GetProjectByIdHandler>();
    }

    public void MapEndpoints(IEndpointRouteBuilder endpoints)
    {
        ArgumentNullException.ThrowIfNull(endpoints);

        var group = endpoints.MapGroup("/projects")
            .WithTags("Projects");

        group.MapGet("/", async Task<Ok<IReadOnlyCollection<ProjectResponse>>> (ListProjectsHandler handler, CancellationToken cancellationToken) =>
            {
                var projects = await handler.HandleAsync(cancellationToken);
                return TypedResults.Ok<IReadOnlyCollection<ProjectResponse>>(
                    projects.Select(ProjectResponse.FromModel).ToArray());
            })
            .WithName("ListProjects");

        group.MapGet("/{id:guid}", async Task<Results<NotFound, Ok<ProjectResponse>>> (Guid id, GetProjectByIdHandler handler, CancellationToken cancellationToken) =>
            {
                var project = await handler.HandleAsync(id, cancellationToken);
                return project is null
                    ? TypedResults.NotFound()
                    : TypedResults.Ok(ProjectResponse.FromModel(project));
            })
            .WithName("GetProjectById");

        group.MapPost("/", async Task<Results<ValidationProblem, Created<ProjectResponse>>> (CreateProjectRequest request, CreateProjectHandler handler, CancellationToken cancellationToken) =>
            {
                var errors = ProjectEndpointValidation.Validate(request);
                if (errors.Count > 0)
                {
                    return TypedResults.ValidationProblem(errors);
                }

                var command = new CreateProject(
                    request.Name,
                    request.Description,
                    request.RepositoryUrl,
                    request.Tags ?? []);

                var project = await handler.HandleAsync(command, cancellationToken);
                var response = ProjectResponse.FromModel(project);

                return TypedResults.Created($"/api/projects/{project.Id}", response);
            })
            .WithName("CreateProject");

        group.MapPut("/{id:guid}", async Task<Results<ValidationProblem, NotFound, Ok<ProjectResponse>>> (Guid id, UpdateProjectRequest request, UpdateProjectHandler handler, CancellationToken cancellationToken) =>
            {
                var errors = ProjectEndpointValidation.Validate(request);
                if (errors.Count > 0)
                {
                    return TypedResults.ValidationProblem(errors);
                }

                var command = new UpdateProject(
                    id,
                    request.Name,
                    request.Description,
                    request.RepositoryUrl,
                    request.Status,
                    request.Tags ?? []);

                var project = await handler.HandleAsync(command, cancellationToken);

                return project is null
                    ? TypedResults.NotFound()
                    : TypedResults.Ok(ProjectResponse.FromModel(project));
            })
            .WithName("UpdateProject");

        group.MapPost("/{id:guid}/archive", async Task<Results<NotFound, Ok<ProjectResponse>>> (Guid id, ArchiveProjectHandler handler, CancellationToken cancellationToken) =>
            {
                var project = await handler.HandleAsync(new ArchiveProject(id), cancellationToken);

                return project is null
                    ? TypedResults.NotFound()
                    : TypedResults.Ok(ProjectResponse.FromModel(project));
            })
            .WithName("ArchiveProject");
    }
}
