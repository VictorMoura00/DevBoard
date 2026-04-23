using DevBoard.Modules.Projects.Data;
using DevBoard.Modules.Projects.Models;

namespace DevBoard.Modules.Projects.Queries;

public sealed class ListProjectsHandler(IProjectRepository projectRepository)
{
    public async Task<IReadOnlyCollection<Project>> HandleAsync(CancellationToken cancellationToken = default)
    {
        return await projectRepository.ListAsync(cancellationToken);
    }
}
