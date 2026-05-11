namespace docs_project.Fetchers;

public class DataFetcher
{
    private readonly string _sourceUrl;
    private readonly string _destinationPath;

    public DataFetcher(string sourceUrl, string destinationPath)
    {
        _sourceUrl = sourceUrl;
        _destinationPath = destinationPath;
    }

    public async Task FetchAndReplaceAsync()
    {
        Directory.CreateDirectory(Path.GetDirectoryName(_destinationPath)!);

        Console.WriteLine($"Fetching data from {_sourceUrl} ...");

        using var http = new HttpClient();
        using var response = await http.GetAsync(_sourceUrl, HttpCompletionOption.ResponseHeadersRead);
        response.EnsureSuccessStatusCode();

        await using var stream = await response.Content.ReadAsStreamAsync();
        await using var file = File.Create(_destinationPath);
        await stream.CopyToAsync(file);

        Console.WriteLine($"Saved to '{_destinationPath}'");
    }
}
