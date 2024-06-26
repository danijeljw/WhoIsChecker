namespace WhoIsChecker.Models
{
    public class DomainWhoisServer
    {
        public string Domain { get; set; } = null!;
        public string? WhoisServer { get; set; }
    }
}
