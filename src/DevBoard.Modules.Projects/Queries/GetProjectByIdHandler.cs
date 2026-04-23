using DevBoard.Modules.Projects.Data;
using DevBoard.Modules.Projects.Models;

namespace DevBoard.Modules.Projects.Queries;

public sealed class GetProjectByIdHandler(IProjectRepository projectRepository)
{
    public async Task<Project?> HandleAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await projectRepository.GetByIdAsync(id, cancellationToken);
    }
}
