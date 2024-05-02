using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using AirlineWeb.Data;
using Microsoft.EntityFrameworkCore;
using AutoMapper;
using AirlineWeb.MessageBus;
using Microsoft.AspNetCore.Authentication.OAuth;
using Microsoft.AspNetCore.Authentication.OAuth.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Threading.Tasks;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
//builder.Services.AddSingleton<IMessageBusClient MessageBusClient> ();
builder.Services.AddSingleton<IMessageBusClient, MessageBusClient>();


builder.Services.AddAuthentication(options =>
    {
        options.DefaultScheme = "Cookies"; // Set the default scheme to Cookies
        options.DefaultChallengeScheme = "GitHub"; // Set the default challenge scheme to GitHub
    })
    .AddCookie("Cookies") // Add cookie authentication
    .AddOAuth("GitHub", options =>
    {
        options.ClientId = "Ov23liyNLN0GZkYwbvRe";
        options.ClientSecret = "693577f696b5b6fc388587b920a4f5ec082f3208";
        options.CallbackPath = "/signin-github";
        options.AuthorizationEndpoint = "https://github.com/login/oauth/authorize";
        options.TokenEndpoint = "https://github.com/login/oauth/access_token";
        options.UserInformationEndpoint = "https://api.github.com/user";
        options.SaveTokens = true;

        options.ClaimActions.MapJsonKey("urn:github:login", "login");
        options.ClaimActions.MapJsonKey("urn:github:id", "id");
        options.ClaimActions.MapJsonKey("urn:github:name", "name");
        options.ClaimActions.MapJsonKey("urn:github:url", "html_url");
        options.ClaimActions.MapJsonKey("urn:github:avatar", "avatar_url");

        options.Events = new OAuthEvents
        {
            OnCreatingTicket = context =>
            {
                // You can customize what happens when the GitHub user is authenticated
                return Task.CompletedTask;
            }
        };
    });





builder.Services.AddDbContext<AirlineDbContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("AirlineConnection"));
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}


app.UseHttpsRedirection();

app.UseStaticFiles();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();

