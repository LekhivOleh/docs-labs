using docs_project.Models;

namespace docs_project.Strategies;

public interface IOutputStrategy
{
    Task OutputAsync(IEnumerable<DeathCause> records);
}
