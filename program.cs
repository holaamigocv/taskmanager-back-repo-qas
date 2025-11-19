using Microsoft.EntityFrameworkCore;
using taskmanager_back_repo_qas.Data;

var builder = WebApplication.CreateBuilder(args);

// Read environment variables for DB connection
var host = Environment.GetEnvironmentVariable("DB_HOST");
var user = Environment.GetEnvironmentVariable("DB_USER");
var pass = Environment.GetEnvironmentVariable("DB_PASS");
var name = Environment.GetEnvironmentVariable("DB_NAME");

var conn = $"Host={host};Username={user};Password={pass};Database={name}";

// Register EF Core + PostgreSQL
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseNpgsql(conn));

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

app.MapControllers();

app.Run();
