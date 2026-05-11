using docs_project.Fetchers;
using docs_project.Readers;
using docs_project.Strategies;
using Microsoft.Extensions.Configuration;

var configuration = new ConfigurationBuilder()
    .SetBasePath(AppContext.BaseDirectory)
    .AddJsonFile("appsettings.json", optional: false)
    .Build();

var dataSourceUrl = configuration["DataSourceUrl"]
    ?? "https://data.cityofnewyork.us/api/views/jb7j-dtam/rows.csv?accessType=DOWNLOAD";
var dataFilePath = configuration["DataFilePath"] ?? "Data/nyc_causes_of_death.csv";
var strategyName = configuration["OutputStrategy"] ?? "Console";
var pageSize = int.TryParse(configuration["PageSize"], out var ps) && ps > 0 ? ps : 100;

var fetcher = new DataFetcher(dataSourceUrl, dataFilePath);
await fetcher.FetchAndReplaceAsync();

IOutputStrategy strategy = strategyName switch
{
    "Kafka" => new KafkaOutputStrategy(
        configuration["Kafka:BootstrapServers"] ?? "localhost:9092",
        configuration["Kafka:Topic"] ?? "nyc-death-causes"),
    "Redis" => new RedisOutputStrategy(
        configuration["Redis:ConnectionString"] ?? "localhost:6379",
        configuration["Redis:Key"] ?? "nyc:death-causes"),
    "Console" => new ConsoleOutputStrategy(),
    _ => new ConsoleOutputStrategy()
};

var records = new CsvDataReader(dataFilePath).Read().ToList();
var totalPages = (int)Math.Ceiling(records.Count / (double)pageSize);

Console.WriteLine($"Loaded {records.Count} records from '{dataFilePath}'");
Console.WriteLine($"Output strategy: {strategyName} | Page size: {pageSize}");
Console.WriteLine(new string('-', 60));

for (var page = 0; page < totalPages; page++)
{
    var batch = records.Skip(page * pageSize).Take(pageSize).ToList();

    Console.WriteLine($"[Page {page + 1}/{totalPages} — rows {page * pageSize + 1}–{page * pageSize + batch.Count}]");
    await strategy.OutputAsync(batch);

    if (page < totalPages - 1)
    {
        Console.WriteLine(new string('-', 60));
        Console.Write("Press any key for the next page...");
        Console.ReadKey(intercept: true);
        Console.WriteLine();
        Console.WriteLine(new string('-', 60));
    }
}

Console.WriteLine(new string('-', 60));
Console.WriteLine("End (but in the end, it doesnt even matter).");
