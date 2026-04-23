using System.Reflection;

namespace DevBoard.Api.Extensions;

public static class AssemblyExtensions
{
    public static IReadOnlyCollection<Assembly> GetDevBoardAssemblies()
    {
        var discovered = new Dictionary<string, Assembly>(StringComparer.Ordinal);
        var queue = new Queue<Assembly>();

        queue.Enqueue(typeof(Program).Assembly);

        while (queue.Count > 0)
        {
            var assembly = queue.Dequeue();
            var assemblyName = assembly.GetName().Name;

            if (assemblyName is null || !assemblyName.StartsWith("DevBoard", StringComparison.Ordinal))
            {
                continue;
            }

            if (!discovered.TryAdd(assembly.FullName ?? assemblyName, assembly))
            {
                continue;
            }

            foreach (var reference in assembly.GetReferencedAssemblies())
            {
                if (!reference.Name?.StartsWith("DevBoard", StringComparison.Ordinal) ?? true)
                {
                    continue;
                }

                try
                {
                    queue.Enqueue(Assembly.Load(reference));
                }
                catch
                {
                    // Ignore references that cannot be loaded during startup discovery.
                }
            }
        }

        return discovered.Values.ToArray();
    }
}
