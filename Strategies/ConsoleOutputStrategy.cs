using docs_project.Models;

namespace docs_project.Strategies;

public class ConsoleOutputStrategy : IOutputStrategy
{
    public Task OutputAsync(IEnumerable<DeathCause> records)
    {
        foreach (var record in records)
            Console.WriteLine(record);

        return Task.CompletedTask;
    }
}
