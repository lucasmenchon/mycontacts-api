using Microsoft.Extensions.DependencyInjection.Extensions;
using MyContactsAPI.Extensions;
using MyContactsAPI.Interfaces;
using MyContactsAPI.Repositories;
using MyContactsAPI.Services;

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
builder.Services.TryAddScoped<IUserPasswordService, UserPasswordService>();
builder.Services.TryAddScoped<IEmailService, EmailService>();
builder.Services.TryAddScoped<IUserLoginService, UserLoginService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
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
