using docs_project.Models;

namespace docs_project.Readers;

public class CsvDataReader
{
    private readonly string _filePath;

    public CsvDataReader(string filePath)
    {
        _filePath = filePath;
    }

    public IEnumerable<DeathCause> Read()
    {
        return File.ReadLines(_filePath)
            .Skip(1)
            .Where(line => !string.IsNullOrWhiteSpace(line))
            .Select(ParseLine);
    }

    private static DeathCause ParseLine(string line)
    {
        var fields = SplitCsvLine(line);
        return new DeathCause
        {
            Year                 = fields.ElementAtOrDefault(0) ?? "",
            LeadingCause         = fields.ElementAtOrDefault(1) ?? "",
            Sex                  = fields.ElementAtOrDefault(2) ?? "",
            RaceEthnicity        = fields.ElementAtOrDefault(3) ?? "",
            Deaths               = fields.ElementAtOrDefault(4) ?? "",
            DeathRate            = fields.ElementAtOrDefault(5) ?? "",
            AgeAdjustedDeathRate = fields.ElementAtOrDefault(6) ?? ""
        };
    }

    private static List<string> SplitCsvLine(string line)
    {
        var fields = new List<string>();
        var current = new System.Text.StringBuilder();
        bool inQuotes = false;

        foreach (char c in line)
        {
            if (c == '"')
            {
                inQuotes = !inQuotes;
            }
            else if (c == ',' && !inQuotes)
            {
                fields.Add(current.ToString().Trim());
                current.Clear();
            }
            else
            {
                current.Append(c);
            }
        }

        fields.Add(current.ToString().Trim());
        return fields;
    }
}
