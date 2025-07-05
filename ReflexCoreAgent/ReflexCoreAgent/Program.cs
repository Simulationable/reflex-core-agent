using Microsoft.EntityFrameworkCore;
using PdfSharpCore.Fonts;
using ReflexCoreAgent.Applications;
using ReflexCoreAgent.Helpers;
using ReflexCoreAgent.Infrastructure.Data;
using ReflexCoreAgent.Infrastructure.Repositories;
using ReflexCoreAgent.Interfaces;
using ReflexCoreAgent.Interfaces.Repositories;
using ReflexCoreAgent.Interfaces.Services;
using System;

var connectionString = Environment.GetEnvironmentVariable("POSTGRES_CONNECTION");

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseNpgsql(connectionString));


// 🔧 Register services & implementations
builder.Services.AddHttpClient(); 
builder.Services.AddHttpContextAccessor();

GlobalFontSettings.FontResolver = new CustomFontResolver();

builder.Services.AddScoped<ITranslator, Translator>();
builder.Services.AddScoped<ILineMessenger, LineMessenger>();
builder.Services.AddScoped<IAgentOrchestrator, AgentOrchestrator>();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IInteractionService, InteractionService>();
builder.Services.AddScoped<IAgentService, AgentService>();
builder.Services.AddScoped<IKnowledgeService, KnowledgeService>();
builder.Services.AddScoped<IEmbeddingService, DeterministicEmbeddingService>();
builder.Services.AddScoped<IModerationFilter, ModerationFilter>();
builder.Services.AddScoped<ICalendarService, CalendarService>();
builder.Services.AddScoped<IPdfService, PdfService>();
builder.Services.AddScoped<INotificationService, NotificationService>();
builder.Services.AddScoped<ICompanyProfileService, CompanyProfileService>();
builder.Services.AddScoped<ITimeParser, SimpleThaiTimeParser>();

builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IUserInteractionRepository, UserInteractionRepository>();
builder.Services.AddScoped<IAgentRepository, AgentRepository>();
builder.Services.AddScoped<IModerationRuleRepository, ModerationRuleRepository>();
builder.Services.AddScoped<ILlamaRequestConfigRepository, LlamaRequestConfigRepository>();
builder.Services.AddScoped<IKnowledgeRepository, KnowledgeRepository>();
builder.Services.AddScoped<ICompanyProfileRepository, CompanyProfileRepository>();
builder.Services.AddScoped<IAppointmentRepository, AppointmentRepository>();

// ✅ Add controllers + OpenAPI (Swagger)
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// 🖥️ Add Razor Pages for UI
builder.Services.AddRazorPages();

builder.Services.AddAuthentication("Cookies")
    .AddCookie("Cookies", options =>
    {
        options.LoginPath = "/Login/Index";
    });

builder.Services.AddSession();

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
app.UseSession();

using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();

    db.Database.Migrate();

    await DbInitializer.SeedAsync(db);
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();
app.Run();
