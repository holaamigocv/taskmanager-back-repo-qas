using Microsoft.EntityFrameworkCore;
using taskmanager_back_repo_qas.Api.Data;

var builder = WebApplication.CreateBuilder(args);

// Read DB connection values from environment or Cloud Run secrets
var dbHost = Environment.GetEnvironmentVariable("DB_HOST") ?? "/cloudsql/" + (Environment.GetEnvironmentVariable("CLOUD_SQL_CONNECTION_NAME") ?? "");
var dbUser = Environment.GetEnvironmentVariable("DB_USER");
var dbPass = Environment.GetEnvironmentVariable("DB_PASS");
var dbName = Environment.GetEnvironmentVariable("DB_NAME") ?? "taskmanagerdb";

string connectionString;

// If DB_HOST was provided as /cloudsql/..., use host=localhost with proxy or special connector.
// For simplicity we'll use a standard connection string assuming the Cloud Run connector or proxy handles it:
if (!string.IsNullOrEmpty(dbHost) && dbHost.StartsWith("/cloudsql"))
{
    // Cloud SQL socket path - Npgsql supports host=/cloudsql/INSTANCE? This is usually handled by Cloud SQL connector.
    // Use host=127.0.0.1 when using the proxy; Cloud Run's --add-cloudsql-instances sets up the connector.
    connectionString = $"Host=10.20.0.3;Database={dbName};Username={dbUser};Password={dbPass};";
}
else
{
    // Use DB_HOST as hostname or socket path
    connectionString = $"Host={dbHost};Database={dbName};Username={dbUser};Password={dbPass};";
}

// Register DB context
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseNpgsql(connectionString));

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Enable swagger in non-production by default (you can add env gating)
app.UseSwagger();
app.UseSwaggerUI();

app.MapControllers();

app.Run();
