using Microsoft.EntityFrameworkCore;
using StudentSystem.Data;
using System.Text.Json.Serialization;
using Microsoft.OpenApi.Models;
using StudentSystem.Middleware;
using StudentSystem.Interfaces;
using StudentSystem.Services;
using StudentSystem.Configuration;


var builder = WebApplication.CreateBuilder(args);

//Obtain connection string from app settings.
string connectionString = builder.Configuration.GetConnectionString("SqlLiteConnection") ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");

// Add services to the container.
builder.Services.AddDbContext<ApplicationDbContext>(options => options.UseSqlite(connectionString));

//Add HttpContextAccessor for DTO Generator Service
builder.Services.AddHttpContextAccessor();

//Prevent Json Cyclic exceptions due to the Entity Framework references, and omit null properties from JSON response.
builder.Services.AddControllers().AddJsonOptions(options => {
    //options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;

    //Omit null values from serialized response.
    options.JsonSerializerOptions.DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull;

    //Output enums as strings rather than ints in response.
    options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
}); 
builder.Services.AddEndpointsApiExplorer();

//Configure Swagger to allow API Key Authorisation.
builder.Services.AddSwaggerGen(settings => {
    settings.AddSecurityDefinition("API Key", new OpenApiSecurityScheme {
        Name = "x-api-key", //header to check for.
        Type = SecuritySchemeType.ApiKey,
        Scheme = "ApiKeyScheme",
        In = ParameterLocation.Header,
        Description = "API Key Header Requirement."
    });

    settings.AddSecurityRequirement(new OpenApiSecurityRequirement {
        {
            new OpenApiSecurityScheme {
                Reference = new OpenApiReference {
                    Type = ReferenceType.SecurityScheme,
                    Id = "API Key"
                },
                In = ParameterLocation.Header
            },
            new List<string>()
        }
    });
});

//Add CORS support
string allowedOrigins = "localhost";
builder.Services.AddCors(options => {
    options.AddPolicy(name: allowedOrigins, policy => {
        policy.WithOrigins("https://localhost", "http://localhost")
            .AllowAnyOrigin()
            .AllowAnyMethod()
            .AllowAnyHeader();
    });
});

//Constructor Dependency Injection for Applcation DbContext into controllers.
builder.Services.AddScoped<ApplicationDbContext>();
builder.Services.AddScoped<IDtoMappingService, DtoMapperService>();
builder.Services.AddScoped<IDtoGeneratorService, DtoGeneratorService>();
var app = builder.Build();

//Enable CORS
app.UseCors(allowedOrigins);

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment()) {
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseMiddleware<ApiKeyMiddleware>();
app.UseAuthorization();
app.MapControllers();
app.Run();
