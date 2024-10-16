using Microsoft.AspNetCore.Mvc;
using Microsoft.OpenApi.Models;
using Quran_Sunnah_BackendAI.Configurations;
using Quran_Sunnah_BackendAI.Interfaces;
using Quran_Sunnah_BackendAI.Middleware;
using Quran_Sunnah_BackendAI.Providers;
using Quran_Sunnah_BackendAI.Services;
using Supabase;
using System.Net;

var builder = WebApplication.CreateBuilder(args);

// Bind the configuration settings
builder.Services.Configure<QuranSunnahProviderOptions>(builder.Configuration.GetSection("ProviderSettings"));

// Register the providers
builder.Services.AddTransient<IEmailServices,EmailServices>();
builder.Services.AddTransient<ISupabaseDatabaseServices,SupabaseDatabaseServices>();
builder.Services.AddTransient<IQuranSunnahBackendAPI,OpenAIProvider>();
builder.Services.AddTransient<IQuranSunnahBackendAPI,AzadProvider>();

// Add services to the container.
builder.Services.AddControllers();

var app = builder.Build();

app.UseApiKeyAuthentication();
app.UseCors(policy =>
{
    policy.WithOrigins("https://www.quran-sunnah-ai.com")  // Allow specific origin
          .AllowAnyMethod()
          .AllowAnyHeader()
          .AllowCredentials(); // If using credentials like cookies or tokens
});

// Ensure UseAuthentication comes after UseCors
app.UseAuthentication();
app.UseAuthorization(); 

app.UseHttpsRedirection();

app.UseAuthorization();

if (!app.Environment.IsDevelopment())
{
    app.UseHttpsRedirection();
    app.UseHsts();
}

app.MapControllers();

app.Run();
