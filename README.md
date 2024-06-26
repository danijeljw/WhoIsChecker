# WhoIsChecker Library

`WhoIsChecker` is a .NET library for checking the expiration date of domains using WHOIS servers. This library supports multiple target frameworks including .NET Standard 2.1, .NET 6.0, and .NET 7.0.

## Features

- Query WHOIS information for domains.
- Extract and convert the domain expiration date to local system time.
- Calculate the number of days until a domain expires.
- Automatically download and use the latest WHOIS server list.

## Getting Started

### Installation

To install `WhoIsChecker`, add the NuGet package to your project:

```sh
dotnet add package WhoIsChecker
```

### Usage

Here's an example of how to use the `WhoIsChecker` library:

1. Load WHOIS Servers: Load the WHOIS servers list from a JSON file or download it if it doesn't exist or is outdated.

1. Query WHOIS Server: Query the WHOIS server for a domain's expiration date.

    ```csharp
    using System;
    using System.Threading.Tasks;
    using WhoIsChecker.Data;
    using WhoIsChecker.Services;

    class Program
    {
        static async Task Main(string[] args)
        {
            if (args.Length == 0)
            {
                Console.WriteLine("Please provide a domain as an argument.");
                return;
            }

            string domain = args[0];
            string tld = GetTld(domain);

            var whoisServers = await DomainWhoisServers.LoadAsync();

            if (whoisServers.TryGetValue(tld, out string? whoisServer))
            {
                if (whoisServer == null)
                {
                    Console.WriteLine("NO WHOIS SERVER FOUND");
                }
                else
                {
                    var whoisService = new WhoisService();
                    var (expirationDate, message) = await whoisService.QueryWhoisServer(domain, whoisServer);
                    if (expirationDate != null)
                    {
                        Console.WriteLine($"Expiration Date (Local Time): {expirationDate}");
                    }
                    Console.WriteLine(message);
                }
            }
            else
            {
                Console.WriteLine("TLD not found in the list.");
            }
        }

        private static string GetTld(string domain)
        {
            var parts = domain.Split('.');
            return parts[^1].ToLower();
        }
    }
    ```

### Building the Library

To build the library, navigate to the project directory and run the following command:

```sh
dotnet build --configuration Release
```

### Packaging the Library

To create a NuGet package, use the following command:

```sh
dotnet pack --configuration Release
```

This will generate a `.nupkg` file in the `bin/Release` directory.

### Publishing the Library

To publish the NuGet package, use the following command:

```sh
dotnet nuget push bin/Release/WhoIsChecker.1.0.0.nupkg --api-key YOUR_API_KEY --source https://api.nuget.org/v3/index.json
```

Replace `YOUR_API_KEY` wityh your actual NuGet API key.

## License

This project is licensed under the MIT License. See the [LICENSE](LICENSE) file for details.

## Contributing

Contributions are welcome! Please open an issue or submit a pull request on GitHub.

## Acknowledgements

Special thanks to all the contributors and open-source projects that made this library possible.

## Contact

For more information, please visit the project page.
