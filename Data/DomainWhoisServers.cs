using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;

namespace WhoIsChecker.Data
{
    public static class DomainWhoisServers
    {
        private const string Url = "https://raw.githubusercontent.com/WooMai/whois-servers/master/list.json";
        private const string FilePath = "list.json";

        public static async Task<Dictionary<string, string?>> LoadAsync()
        {
            if (!File.Exists(FilePath) || File.GetCreationTime(FilePath).Date != DateTime.Now.Date)
            {
                if (File.Exists(FilePath))
                {
                    File.Delete(FilePath);
                }

                using (HttpClient client = new HttpClient())
                {
                    var json = await client.GetStringAsync(Url);
                    await File.WriteAllTextAsync(FilePath, json);
                }
            }

            var fileContent = await File.ReadAllTextAsync(FilePath);
            var whoisServers = JsonSerializer.Deserialize<Dictionary<string, string?>>(fileContent);

            if (whoisServers == null)
            {
                throw new InvalidOperationException("Failed to load WHOIS servers from the JSON file.");
            }

            return whoisServers;
        }
    }
}
