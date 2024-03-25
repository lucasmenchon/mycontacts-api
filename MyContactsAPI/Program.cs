using ContactsManage.Data;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.IdentityModel.Tokens;
using MyContactsAPI.Extensions;
using MyContactsAPI.Helper;
using MyContactsAPI.Interfaces;
using MyContactsAPI.Repositories;
using MyContactsAPI.Services;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

builder.AddConfiguration();
builder.AddDatabase();
builder.AddJwtAuthentication();

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();

builder.Services.TryAddScoped<IContactRepository, ContactRepository>();
builder.Services.TryAddScoped<IUserRepository, UserRepository>();
builder.Services.TryAddScoped<IUserPasswordService, UserPasswordService>();
builder.Services.TryAddScoped<IEmail, EmailModel>();

builder.Services.AddTransient<IJwtTokenService, JwtTokenService>();

builder.Services.AddAuthorization();

var app = builder.Build();

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
