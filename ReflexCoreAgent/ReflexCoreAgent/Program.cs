using ReflexCoreAgent.Applications;
using ReflexCoreAgent.Interfaces;

var builder = WebApplication.CreateBuilder(args);

// 🔧 Register services & implementations
builder.Services.AddSingleton<IMemoryStore, MemoryStore>();
builder.Services.AddSingleton<ITranslator, Translator>();
builder.Services.AddSingleton<ILineMessenger, LineMessenger>();
builder.Services.AddSingleton<IAgentOrchestrator, AgentOrchestrator>();

// ✅ Add controllers + OpenAPI (Swagger)
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// 🖥️ Add Razor Pages for UI
builder.Services.AddRazorPages();

var app = builder.Build();

// 🌐 Enable Swagger in development
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.UseDefaultFiles();
app.UseStaticFiles();

app.MapRazorPages();

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();
app.Run();
