using ContactsManage.Data;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace MyContactsAPI.Extensions;

public static class BuilderExtension
{
    public static void AddConfiguration(this WebApplicationBuilder builder)
    {
        Configuration.Database.ConnectionString =
    builder.Environment.IsDevelopment()
        ? builder.Configuration.GetConnectionString("LocalDb") ?? throw new InvalidOperationException("Connection string not found for LocalDb")
        : builder.Configuration.GetConnectionString("HostDb") ?? throw new InvalidOperationException("Connection string not found for HostDb");

        Configuration.Secrets.ApiKey =
            builder.Configuration["Secrets:ApiKey"] ?? string.Empty;
        Configuration.Secrets.JwtPrivateKey =
            builder.Configuration["Secrets:JwtPrivateKey"] ?? string.Empty;
        Configuration.Secrets.PasswordSaltKey =
            builder.Configuration["Secrets:PasswordSaltKey"] ?? string.Empty;

        Configuration.SendGrid.ApiKey =
            builder.Configuration["SendGrid:ApiKey"] ?? string.Empty;

        Configuration.Email.DefaultFromName =
            builder.Configuration["Email:DefaultFromName"] ?? string.Empty;
        Configuration.Email.DefaultFromEmail =
            builder.Configuration["Email:DefaultFromEmail"] ?? string.Empty;
        Configuration.Email.DefaultFromHost =
            builder.Configuration["Email:DefaultFromHost"] ?? string.Empty;
        Configuration.Email.DefaultFromPort =
            int.TryParse(builder.Configuration["Email:DefaultFromPort"], out int port) ? port : 0;
        Configuration.Email.DefaultFromPassword =
            builder.Configuration["Email:DefaultFromPassword"] ?? string.Empty;
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

        builder.Services.AddAuthorization();
    }
}

