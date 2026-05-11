using System.Text.Json;
using Confluent.Kafka;
using docs_project.Models;

namespace docs_project.Strategies;

public class KafkaOutputStrategy : IOutputStrategy
{
    private readonly string _bootstrapServers;
    private readonly string _topic;

    public KafkaOutputStrategy(string bootstrapServers, string topic)
    {
        _bootstrapServers = bootstrapServers;
        _topic = topic;
    }

    public async Task OutputAsync(IEnumerable<DeathCause> records)
    {
        var config = new ProducerConfig { BootstrapServers = _bootstrapServers };

        using var producer = new ProducerBuilder<Null, string>(config).Build();

        foreach (var record in records)
        {
            var json = JsonSerializer.Serialize(record);
            await producer.ProduceAsync(_topic, new Message<Null, string> { Value = json });
        }

        producer.Flush(TimeSpan.FromSeconds(10));
        Console.WriteLine($"All records sent to Kafka topic '{_topic}' on {_bootstrapServers}");
    }
}
