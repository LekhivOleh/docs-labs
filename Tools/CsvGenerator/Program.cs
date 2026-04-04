namespace CsvGenerator
{
    internal static class Program
    {
        private readonly static List<string> names = new()
        {
            "Oleh", "Jane", "Nazar", "Danylo", "Vadym", "Joe", "Marty", "Dave", "Ozzy",
            "Anna", "Maria", "Olena", "Iryna", "Petro", "Sergiy", "Ivan", "Maksym", "Katia",
            "Lena", "Tom", "Jerry", "Alice", "Bob", "Carlos", "Sofia", "Luca", "Mateo",
            "Emma", "Noah", "Liam", "Mia", "Amelia", "Lucas", "Chloe", "Artem", "Yulia"
        };

        private readonly static List<string> mailProviders = new()
        {
            "gmail.com", "yahoo.com", "ukr.net", "hotmail.com", "outlook.com", "example.com",
            "proton.me", "aol.com", "live.com", "edu.ua"
        };
        private static async Task Main(string[] args)
        {
            var argsList = args.ToList();
            var output = argsList.FirstOrDefault() ?? "users.csv";
            var lines = int.TryParse(argsList.ElementAtOrDefault(1), out var n) ? n : 1000;

            using var sw = new StreamWriter(output);
            var rand = new Random();
            await sw.WriteLineAsync("Username,Email,Password");

            var used = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
            int generated = 0;

            while (generated < lines)
            {
                string username;
                int attempts = 0;
                do
                {
                    var name = names[rand.Next(names.Count)];
                    var suffix = rand.Next(1000000);
                    username = $"{name}{suffix}";
                    attempts++;

                    if (attempts > 1000)
                    {
                        // In case I get f.e. Alice + random number 1000 times, that already is used in email (which will never happen, but better be safe then sorry)
                        username = string.Concat(name, Guid.NewGuid().ToString("N").AsSpan(0, 8));
                        break;
                    }
                }
                while (!used.Add(username));

                var provider = mailProviders[rand.Next(mailProviders.Count)];
                if (string.IsNullOrWhiteSpace(provider))
                {
                    provider = "example.com";
                }
                var email = $"{username}@{provider}";

                var password = "pass" + rand.Next(10000);
                await sw.WriteLineAsync($"{username},{email},{password}");
                generated++;
            }

            Console.WriteLine($"Generated {lines} rows into {output}");
        }
    }
}
