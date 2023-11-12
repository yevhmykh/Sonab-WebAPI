using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Net.Http.Headers;
using Microsoft.OpenApi.Models;
using Serilog;
using Sonab.WebAPI.Contexts;
using Sonab.WebAPI.Extensions.Program;
using Sonab.WebAPI.Hubs;

const string allowOrigins = "_allowOrigins";

var builder = WebApplication.CreateBuilder(args);

if (builder.Environment.IsDevelopment())
{
    Directory.SetCurrentDirectory(AppDomain.CurrentDomain.BaseDirectory);
}

builder.Host.UseSerilog((hostContext, services, configuration) =>
{
    configuration.ReadFrom.Configuration(builder.Configuration);
});

// Add services to the container.
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("ContextSQLite")));

builder.Services.AddMemoryCache();

builder.Services.AddUtils();
builder.Services.AddRepositories();
builder.Services.AddSubServices();
builder.Services.AddMainServices();

builder.Services.AddCors(options =>
{
    options.AddPolicy(allowOrigins, policy =>
    {
        policy.WithOrigins(builder.Configuration["AllowedCors"])
            .WithHeaders(HeaderNames.Authorization)
            .WithHeaders(HeaderNames.ContentType)
            .AllowAnyMethod()
            .AllowCredentials()
            .WithHeaders(HeaderNames.XRequestedWith)
            .WithHeaders("x-signalr-user-agent");
    });
});

builder.Services.AddControllers();
builder.Services.AddSignalR();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo
    {
        Version = "v1",
        Title = "Sonab API",
        Description = "Sonab Web API server",
    });
    options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Description = @"JWT Authorization header using the Bearer scheme. \r\n\r\n 
                      Enter 'Bearer' [space] and then your token in the text input below.
                      \r\n\r\nExample: 'Bearer 12345abcdef'",
        Name = "Authorization",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer"
    });

    options.AddSecurityRequirement(new OpenApiSecurityRequirement()
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                },
                Scheme = "oauth2",
                Name = "Bearer",
                In = ParameterLocation.Header,
            },
            new List<string>()
        }
    });
});

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(options =>
{
    options.Authority = builder.Configuration["Auth0:Authority"];
    options.Audience = builder.Configuration["Auth0:Audience"];
    options.Events = new JwtBearerEvents
    {
        OnMessageReceived = context =>
        {
            var accessToken = context.Request.Query["access_token"];

            // If the request is for our hub...
            var path = context.HttpContext.Request.Path;
            if (!string.IsNullOrEmpty(accessToken) &&
                (path.StartsWithSegments("/Hub")))
            {
                // Read the token out of the query string
                context.Token = accessToken;
            }
            return Task.CompletedTask;
        }
    };
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

using (var scope = app.Services.CreateScope())
{
    AppDbContext context = scope.ServiceProvider.GetRequiredService<AppDbContext>();

    Directory.CreateDirectory(Path.Combine(Directory.GetCurrentDirectory(), "Data", "Database"));
    context.Database.EnsureCreated();
}

app.UseHttpsRedirection();
app.UseCors(allowOrigins);

app.UseAuthentication();
app.UseAuthorization();

app.UseSonabExceptionHandler();

app.MapHub<NotificationHub>("/Hub");
app.MapControllers();

app.Run();
