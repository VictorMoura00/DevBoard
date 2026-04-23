using System.Net;
using System.Net.Http.Json;
using DevBoard.Modules.Projects.Endpoints;
using DevBoard.Modules.Projects.Models;

namespace DevBoard.IntegrationTests;

[Collection(IntegrationTestCollection.Name)]
public sealed class ProjectsEndpointsTests(CustomWebApplicationFactory factory)
{
    [Fact]
    public async Task ProjectsCrudFlow_WorksEndToEnd()
    {
        await factory.ResetDatabaseAsync();

        using var client = factory.CreateClient();

        var createRequest = new CreateProjectRequest(
            "DevBoard",
            "Project management app",
            "https://github.com/VictorMoura00/DevBoard",
            ["dotnet", "angular"]);

        using var createResponse = await client.PostAsJsonAsync("/api/projects", createRequest);

        Assert.Equal(HttpStatusCode.Created, createResponse.StatusCode);

        var createdProject = await createResponse.Content.ReadFromJsonAsync<ProjectResponse>();
        Assert.NotNull(createdProject);
        Assert.Equal("DevBoard", createdProject.Name);
        Assert.Equal(ProjectStatus.Active, createdProject.Status);
        Assert.Equal(2, createdProject.Tags.Count);

        var projectId = createdProject.Id;

        var listedProjects = await client.GetFromJsonAsync<List<ProjectResponse>>("/api/projects");
        Assert.NotNull(listedProjects);
        Assert.Single(listedProjects);

        var fetchedProject = await client.GetFromJsonAsync<ProjectResponse>($"/api/projects/{projectId}");
        Assert.NotNull(fetchedProject);
        Assert.Equal(projectId, fetchedProject.Id);

        var updateRequest = new UpdateProjectRequest(
            "DevBoard API",
            "Updated description",
            "https://github.com/VictorMoura00/DevBoard",
            ProjectStatus.Paused,
            ["backend"]);

        using var updateResponse = await client.PutAsJsonAsync($"/api/projects/{projectId}", updateRequest);

        updateResponse.EnsureSuccessStatusCode();

        var updatedProject = await updateResponse.Content.ReadFromJsonAsync<ProjectResponse>();
        Assert.NotNull(updatedProject);
        Assert.Equal("DevBoard API", updatedProject.Name);
        Assert.Equal(ProjectStatus.Paused, updatedProject.Status);
        Assert.Single(updatedProject.Tags);

        using var archiveResponse = await client.PostAsync($"/api/projects/{projectId}/archive", content: null);

        archiveResponse.EnsureSuccessStatusCode();

        var archivedProject = await archiveResponse.Content.ReadFromJsonAsync<ProjectResponse>();
        Assert.NotNull(archivedProject);
        Assert.Equal(ProjectStatus.Archived, archivedProject.Status);
        Assert.NotNull(archivedProject.ArchivedAt);
    }

    [Fact]
    public async Task CreateProject_ReturnsValidationProblem_WhenRequestIsInvalid()
    {
        await factory.ResetDatabaseAsync();

        using var client = factory.CreateClient();

        var request = new CreateProjectRequest(
            "",
            null,
            "not-an-url",
            ["ok", " "]);

        using var response = await client.PostAsJsonAsync("/api/projects", request);

        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
    }
}
