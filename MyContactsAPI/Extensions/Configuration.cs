namespace MyContactsAPI.Extensions
{
    public static class Configuration
    {
        public static SecretsConfiguration Secrets { get; set; } = new();
        public static EmailConfiguration Email { get; set; } = new();
        public static SendGridConfiguration SendGrid { get; set; } = new();
        public static DatabaseConfiguration Database { get; set; } = new();

        public class DatabaseConfiguration
        {
            public string ConnectionString { get; set; } = string.Empty;
        }

        public class EmailConfiguration
        {
            public string DefaultFromName { get; set; } = string.Empty;
            public string DefaultFromEmail { get; set; } = string.Empty;            
            public string DefaultFromHost { get; set; } = string.Empty;
            public int DefaultFromPort { get; set; }
            public string DefaultFromPassword { get; set; } = string.Empty;
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
}
