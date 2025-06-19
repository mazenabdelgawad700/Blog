namespace Blog.Domain.Helpers
{
    public class EmailSettings
    {
        public string Host { get; set; } = null!;
        public int Port { get; set; }
        public bool UseSSL { get; set; }
        public string EmailAddress { get; set; } = null!;
        public string Password { get; set; } = null!;
        public string SenderHeader { get; set; } = null!;
    }
}
