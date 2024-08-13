using Microsoft.AspNetCore.Mvc;
using Microsoft.OpenApi.Models;
using Quran_Sunnah_BackendAI.Configurations;
using Quran_Sunnah_BackendAI.Interfaces;
using Quran_Sunnah_BackendAI.Middleware;
using Quran_Sunnah_BackendAI.Providers;
using Quran_Sunnah_BackendAI.Services;
using System.Net;

var builder = WebApplication.CreateBuilder(args);

// Bind the configuration settings
builder.Services.Configure<QuranSunnahProviderOptions>(builder.Configuration.GetSection("ProviderSettings"));

// Register the providers
builder.Services.AddTransient<IQuranSunnahBackendAPI,OpenAIProvider>();
builder.Services.AddTransient<IQuranSunnahBackendAPI,AzadProvider>();

// Add services to the container.
builder.Services.AddControllers();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
//builder.Services.AddEndpointsApiExplorer();

//builder.Services.AddSwaggerGen(options =>
//{
//    options.SwaggerDoc("v1", new OpenApiInfo
//    {
//        Version = "v1",
//        Title = "Quran-Sunnah AI Backend API",
//        Description = "List of REST APIs that integrate with OpenAI to ask questions based on source of Quran & Hadith",
//        TermsOfService = new Uri("https://example.com/terms"),
//        Contact = new OpenApiContact
//        {
//            Name = "Syafisham",
//            Email = "syafishamsalleh@gmail.com"
//        },

//    });

//    options.AddSecurityDefinition("ApiKey", new OpenApiSecurityScheme
//    {
//        Description = "ApiKey must appear in header",
//        Type = SecuritySchemeType.ApiKey,
//        Name = "XApiKey",
//        In = ParameterLocation.Header,
//        Scheme = "ApiKeyScheme"
//    });
//    var key = new OpenApiSecurityScheme()
//    {
//        Reference = new OpenApiReference
//        {
//            Type = ReferenceType.SecurityScheme,
//            Id = "ApiKey"
//        },
//        In = ParameterLocation.Header
//    };
//    var requirement = new OpenApiSecurityRequirement
//                    {
//                             { key, new List<string>() }
//                    };
//    options.AddSecurityRequirement(requirement);
//});

//we comment it out first, since for now, we didnt 
//builder.Services.AddAWSLambdaHosting(LambdaEventSource.HttpApi);

//var MyAllowSpecificOrigins = "MyCorsPolicy";
//var allowedOrigins = new string[] { "http://localhost:300", "http://localhost:8080" , "https://www.quran-sunnah-ai.com/" };
//builder.Services.AddCors(options =>
//{
//    options.AddPolicy(name: MyAllowSpecificOrigins,
//        policy =>
//        {
//            policy.WithOrigins(allowedOrigins)
//              .AllowAnyHeader().AllowAnyMethod();
//        });
//});

//builder.Services.AddSingleton<IAPIHttpClientWrapper, AIHttpClientWrapper>();


//builder.Services.AddSingleton<IMongoDbServices, MongoDbServices>();
var app = builder.Build();

app.UseApiKeyAuthentication();
app.UseCors();
app.UseApiKeyAuthentication();
app.UseHttpsRedirection();

app.UseAuthorization();

if (!app.Environment.IsDevelopment())
{
    app.UseHttpsRedirection();
    app.UseHsts();
}

app.MapControllers();

app.Run();
