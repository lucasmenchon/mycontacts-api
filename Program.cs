using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Options;
using MyContactsAPI.Extensions;
using MyContactsAPI.Interfaces;
using MyContactsAPI.Repositories;
using MyContactsAPI.Services;
using MyContactsAPI.Swagger;
using Swashbuckle.AspNetCore.SwaggerGen;

var builder = WebApplication.CreateBuilder(args);
var config = builder.Configuration;

builder.AddConfiguration();
builder.AddDatabase();
builder.AddJwtAuthentication();

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
builder.Services.TryAddScoped<IContactRepository, ContactRepository>();
builder.Services.TryAddScoped<IUserRepository, UserRepository>();
builder.Services.TryAddScoped<IUserPasswordRepository, UserPasswordRepository>();
builder.Services.TryAddScoped<IEmailService, EmailService>();
builder.Services.TryAddScoped<IAuthRepository, AuthRepository>();
builder.Services.TryAddScoped<JwtTokenService>();
builder.Services.AddTransient<IConfigureOptions<SwaggerGenOptions>, ConfigureSwaggerOptions>();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.Run();
