using ContactsManage.Data;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using System.Text;
using static MyContactsAPI.Extensions.Configuration;

namespace MyContactsAPI.Extensions;

public static class BuilderExtension
{
    public static void AddConfiguration(this WebApplicationBuilder builder)
    {
        Configuration.Database.ConnectionString = GetConnectionString(builder);

        Configuration.Secrets = GetSecretsConfiguration();

        Configuration.Email = GetEmailConfiguration();
    }

    public static void AddDatabase(this WebApplicationBuilder builder)
    {
        builder.Services.AddDbContext<DataContext>(options =>
            options.UseNpgsql(
                Configuration.Database.ConnectionString,
                b => b.MigrationsAssembly("MyContactsAPI")));
    }

    public static void AddJwtAuthentication(this WebApplicationBuilder builder)
    {
        builder.Services
            .AddAuthentication(x =>
            {
                x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                x.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(x =>
            {
                x.RequireHttpsMetadata = false;
                x.SaveToken = true;
                x.TokenValidationParameters = new TokenValidationParameters
                {
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(Configuration.Secrets.JwtPrivateKey)),
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true
                };
            });

        builder.Services.ConfigureApplicationCookie(options =>
        {
            options.Cookie.Name = "AuthToken";
            options.Cookie.HttpOnly = true;
            options.SlidingExpiration = false;
        });
    }

    private static string GetConnectionString(WebApplicationBuilder builder)
    {
        string connectionString = builder.Environment.IsDevelopment() ?
        Environment.GetEnvironmentVariable("LOCAL_DB_CONNECTION") ?? string.Empty :
        Environment.GetEnvironmentVariable("HOST_DB_CONNECTION") ?? string.Empty;

        return connectionString;
    }

    private static SecretsConfiguration GetSecretsConfiguration()
    {
        string secretsConfig = Environment.GetEnvironmentVariable("SECRETS_CONFIG") ?? string.Empty;
        var secrets = JsonConvert.DeserializeObject<SecretsConfiguration>(secretsConfig);

        return secrets ?? new SecretsConfiguration();
    }

    private static EmailConfiguration GetEmailConfiguration()
    {
        string emailConfig = Environment.GetEnvironmentVariable("EMAIL_CONFIG") ?? string.Empty;
        var emailSettings = JsonConvert.DeserializeObject<EmailConfiguration>(emailConfig);

        return new EmailConfiguration
        {
            Name = emailSettings?.Name ?? string.Empty,
            FromEmail = emailSettings?.FromEmail ?? string.Empty,
            Host = emailSettings?.Host ?? string.Empty,
            Port = int.TryParse(emailSettings?.Port.ToString(), out int port) ? port : 0,
            Password = emailSettings?.Password ?? string.Empty
        };
    }
}

