using Microsoft.AspNetCore.Mvc;
using Microsoft.OpenApi.Models;
using Quran_Sunnah_BackendAI.Middleware;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();

builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo
    {
        Version = "v1",
        Title = "Quran-Sunnah AI Backend API",
        Description = "List of REST APIs that integrate with OpenAI to ask questions based on source of Quran & Hadith",
        TermsOfService = new Uri("https://example.com/terms"),
        Contact = new OpenApiContact
        {
            Name = "Syafisham",
            Email = "syafishamsalleh@gmail.com"
        },

    });

    options.AddSecurityDefinition("ApiKey", new OpenApiSecurityScheme
    {
        Description = "ApiKey must appear in header",
        Type = SecuritySchemeType.ApiKey,
        Name = "XApiKey",
        In = ParameterLocation.Header,
        Scheme = "ApiKeyScheme"
    });
    var key = new OpenApiSecurityScheme()
    {
        Reference = new OpenApiReference
        {
            Type = ReferenceType.SecurityScheme,
            Id = "ApiKey"
        },
        In = ParameterLocation.Header
    };
    var requirement = new OpenApiSecurityRequirement
                    {
                             { key, new List<string>() }
                    };
    options.AddSecurityRequirement(requirement);
});

//we comment it out first, since for now, we didnt 
//builder.Services.AddAWSLambdaHosting(LambdaEventSource.HttpApi);

var MyAllowSpecificOrigins = "MyCorsPolicy";
var allowedOrigins = new string[] { "http://localhost:300" , "https://www.quran-sunnah-ai.com/" };
builder.Services.AddCors(options =>
{
    options.AddPolicy(name: MyAllowSpecificOrigins,
        policy =>
        {
            policy.WithOrigins(allowedOrigins)
              .AllowAnyHeader().AllowAnyMethod();
        });
});

builder.Services.AddControllers();
var app = builder.Build();

app.UseApiKeyAuthentication();
app.UseSwagger();
app.UseSwaggerUI();
app.UseCors();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
