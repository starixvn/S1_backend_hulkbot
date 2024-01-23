using HulkBot;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddHealthChecks();
builder.Services
    .Configure<OpenAIOptions>(builder.Configuration.GetSection(OpenAIOptions.OpenAI))
    .Configure<ChatDatabaseSettings>(builder.Configuration.GetSection("ChatDatabase"))
    .Configure<Elastic>(builder.Configuration.GetSection("Elastic"));

builder.Services
    .AddScoped<IChatRepository, ChatRepository>()
    .AddScoped<IKnowledgeRepository, KnowledgeRepository>();

builder.Services.AddControllers();

var app = builder.Build();

// Configure the HTTP request pipeline.
app.MapHealthChecks("/health");

app.UseAuthorization();

app.MapControllers();

app.Run();
