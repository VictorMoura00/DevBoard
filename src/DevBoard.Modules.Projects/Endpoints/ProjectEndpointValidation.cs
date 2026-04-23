using DevBoard.Modules.Projects.Models;
using Microsoft.AspNetCore.Http;

namespace DevBoard.Modules.Projects.Endpoints;

internal static class ProjectEndpointValidation
{
    public static Dictionary<string, string[]> Validate(CreateProjectRequest request)
    {
        ArgumentNullException.ThrowIfNull(request);

        return ValidateCore(request.Name, request.Description, request.RepositoryUrl, request.Tags);
    }

    public static Dictionary<string, string[]> Validate(UpdateProjectRequest request)
    {
        ArgumentNullException.ThrowIfNull(request);

        var errors = ValidateCore(request.Name, request.Description, request.RepositoryUrl, request.Tags);

        if (request.Status is ProjectStatus.Archived)
        {
            errors["status"] = ["Use the archive endpoint to archive a project."];
        }

        return errors;
    }

    public static IResult ValidationProblem(Dictionary<string, string[]> errors)
    {
        ArgumentNullException.ThrowIfNull(errors);

        return TypedResults.ValidationProblem(errors);
    }

    private static Dictionary<string, string[]> ValidateCore(
        string name,
        string? description,
        string? repositoryUrl,
        IReadOnlyCollection<string>? tags)
    {
        var errors = new Dictionary<string, string[]>(StringComparer.OrdinalIgnoreCase);

        if (string.IsNullOrWhiteSpace(name))
        {
            errors["name"] = ["Name is required."];
        }
        else if (name.Length > 200)
        {
            errors["name"] = ["Name must have at most 200 characters."];
        }

        if (description is { Length: > 2000 })
        {
            errors["description"] = ["Description must have at most 2000 characters."];
        }

        if (!string.IsNullOrWhiteSpace(repositoryUrl) &&
            !Uri.TryCreate(repositoryUrl, UriKind.Absolute, out _))
        {
            errors["repositoryUrl"] = ["RepositoryUrl must be a valid absolute URL."];
        }

        if (tags?.Any(tag => string.IsNullOrWhiteSpace(tag)) == true)
        {
            errors["tags"] = ["Tags cannot contain blank values."];
        }

        return errors;
    }
}
