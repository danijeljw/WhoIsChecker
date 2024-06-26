using System;
using System.IO;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace WhoIsChecker.Services
{
    public class WhoisService
    {
        public async Task<(string? expirationDate, string message)> QueryWhoisServer(string domain, string whoisServer)
        {
            int port = 43; // Default WHOIS port

            try
            {
                using (var tcpClient = new TcpClient(whoisServer, port))
                using (var networkStream = tcpClient.GetStream())
                using (var writer = new StreamWriter(networkStream, Encoding.ASCII) { NewLine = "\r\n", AutoFlush = true })
                using (var reader = new StreamReader(networkStream, Encoding.ASCII))
                {
                    await writer.WriteLineAsync(domain);
                    string response = await reader.ReadToEndAsync();

                    string? expirationDate = ExtractExpirationDate(response);
                    if (expirationDate != null)
                    {
                        DateTime expiryDateUtc = DateTime.Parse(expirationDate, null, System.Globalization.DateTimeStyles.AdjustToUniversal);
                        DateTime expiryDateLocal = expiryDateUtc.ToLocalTime();
                        TimeSpan daysUntilExpiry = expiryDateLocal - DateTime.Now;

                        return (expiryDateLocal.ToString(), $"Days until Expiration: {daysUntilExpiry.Days}");
                    }
                    else
                    {
                        return (null, "Expiration date not found.");
                    }
                }
            }
            catch (Exception ex)
            {
                return (null, $"Error querying WHOIS server: {ex.Message}");
            }
        }

        private static string? ExtractExpirationDate(string whoisResponse)
        {
            string[] lines = whoisResponse.Split(new[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries);
            foreach (var line in lines)
            {
                if (line.Trim().StartsWith("Registry Expiry Date:", StringComparison.OrdinalIgnoreCase))
                {
                    return line.Split(new[] { ':' }, 2)[1].Trim();
                }
            }
            return null;
        }
    }
}
