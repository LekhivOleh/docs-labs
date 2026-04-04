using System.IO;
using docs_project.Application.Dto.Read;
using docs_project.Application.Interfaces.Repositories;

namespace docs_project.Infrastructure.Csv
{
    public class CsvReader : ICsvReader
    {
        public async Task<IEnumerable<CsvRecord>> ReadAllAsync(Stream stream)
        {
            var list = new List<CsvRecord>();
            using var reader = new StreamReader(stream);

            var header = await reader.ReadLineAsync();
            if (header is null) return list;

            while (!reader.EndOfStream)
            {
                var line = await reader.ReadLineAsync();
                if (string.IsNullOrWhiteSpace(line))
                    continue;
                var parts = line.Split(',');

                if (parts.Length < 3)
                    continue;

                var rec = new CsvRecord
                {
                    Username = parts[0].Trim(),
                    Email = parts[1].Trim(),
                    Password = parts[2].Trim()
                };
                list.Add(rec);
            }

            return list;
        }
    }
}
