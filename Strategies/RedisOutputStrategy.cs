using System.Text.Json;
using docs_project.Models;
using StackExchange.Redis;

namespace docs_project.Strategies;

public class RedisOutputStrategy : IOutputStrategy
{
    private readonly string _connectionString;
    private readonly string _key;

    public RedisOutputStrategy(string connectionString, string key)
    {
        _connectionString = connectionString;
        _key = key;
    }

    public async Task OutputAsync(IEnumerable<DeathCause> records)
    {
        using var connection = await ConnectionMultiplexer.ConnectAsync(_connectionString);
        var db = connection.GetDatabase();

        await db.KeyDeleteAsync(_key);

        foreach (var record in records)
        {
            var json = JsonSerializer.Serialize(record);
            await db.ListRightPushAsync(_key, json);
        }

        var count = await db.ListLengthAsync(_key);
        Console.WriteLine($"Pushed {count} records to Redis list '{_key}' at {_connectionString}");
    }
}
