namespace MyContactsAPI.Extensions;
public static class Configuration
{
    public static SecretsConfiguration Secrets { get; set; } = new();
    public static EmailConfiguration Email { get; set; } = new();
    public static DatabaseConfiguration Database { get; set; } = new();

    public class DatabaseConfiguration
    {
        public string ConnectionString { get; set; } = string.Empty;
    }

    public class EmailConfiguration
    {
        public string Name { get; set; } = string.Empty;
        public string FromEmail { get; set; } = string.Empty;
        public string Host { get; set; } = string.Empty;
        public int Port { get; set; }
        public string Password { get; set; } = string.Empty;
    }

    public class SendGridConfiguration
    {
        public string ApiKey { get; set; } = string.Empty;
    }

    public class SecretsConfiguration
    {
        public string ApiKey { get; set; } = string.Empty;
        public string JwtPrivateKey { get; set; } = string.Empty;
        public string PasswordSaltKey { get; set; } = string.Empty;
    }
}

