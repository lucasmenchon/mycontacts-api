using Microsoft.Extensions.DependencyInjection.Extensions;
using MyContactsAPI.Extensions;
using MyContactsAPI.Interfaces;
using MyContactsAPI.Repositories;
using MyContactsAPI.Services;

var builder = WebApplication.CreateBuilder(args);

builder.AddConfiguration();
builder.AddDatabase();
builder.AddJwtAuthentication();

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();

builder.Services.TryAddScoped<IContactRepository, ContactRepository>();
builder.Services.TryAddScoped<IUserRepository, UserRepository>();
builder.Services.TryAddScoped<IUserPasswordService, UserPasswordService>();
builder.Services.TryAddScoped<IEmailService, EmailService>();
builder.Services.TryAddScoped<IUserLoginService, UserLoginService>();

builder.Services.AddTransient<IJwtTokenService, JwtTokenService>();

builder.Services.AddAuthorization();

var app = builder.Build();

app.UseMiddleware<JwtValidationMiddleware>();

app.UseAuthentication();

app.UseAuthorization();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.MapControllers();

app.Run();
