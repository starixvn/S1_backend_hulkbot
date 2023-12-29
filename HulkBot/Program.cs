using HulkBot;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddHealthChecks();
builder.Services.Configure<OpenAIOptions>(
    builder.Configuration.GetSection(OpenAIOptions.OpenAI));
builder.Services.Configure<ChatDatabaseSettings>(
    builder.Configuration.GetSection("ChatDatabase"));
// Add services to the container.
builder.Services.AddScoped<ChatService>();

builder.Services.AddControllers();

var app = builder.Build();

// Configure the HTTP request pipeline.
app.MapHealthChecks("/health");

app.UseAuthorization();

app.MapControllers();

app.Run();
