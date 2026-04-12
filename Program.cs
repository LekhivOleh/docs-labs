using docs_project.Application.Interfaces.Repositories;
using docs_project.Application.Interfaces.Services;
using docs_project.Application.Services;
using docs_project.Infrastructure.Csv;
using docs_project.Infrastructure.Data;
using docs_project.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlite("Data Source=app.db"));

builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IMessageRepository, MessageRepository>();
builder.Services.AddScoped<IChatRepository, ChatRepository>();
builder.Services.AddScoped<ICsvReader, CsvReader>();
// Application services
builder.Services.AddScoped<IUserService>(sp => new UserService(sp.GetRequiredService<IUserRepository>(), sp.GetRequiredService<ICsvReader>()));
builder.Services.AddScoped<IMessageService>(sp => new MessageService(sp.GetRequiredService<IMessageRepository>(), sp.GetRequiredService<IChatRepository>()));
builder.Services.AddScoped<IChatService>(sp => new ChatService(sp.GetRequiredService<IChatRepository>(), sp.GetRequiredService<IUserRepository>()));
builder.Services.AddScoped<IAuthService, docs_project.Application.Services.AuthService>();

builder.Services.AddControllers()
    .AddJsonOptions(o =>
    {
        o.JsonSerializerOptions.ReferenceHandler = System.Text.Json.Serialization.ReferenceHandler.IgnoreCycles;
        o.JsonSerializerOptions.MaxDepth = 64;
    });

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.UseSwagger();
    app.UseSwaggerUI();
}

// Simple token middleware for demo: expects Authorization: Bearer {token}
app.Use(async (context, next) =>
{
    if (context.Request.Headers.TryGetValue("Authorization", out var val))
    {
        var s = val.ToString();
        if (s.StartsWith("Bearer ", StringComparison.OrdinalIgnoreCase))
        {
            var token = s.Substring(7).Trim();
            try
            {
                var decoded = System.Text.Encoding.UTF8.GetString(Convert.FromBase64String(token));
                var parts = decoded.Split(':');
                if (Guid.TryParse(parts[0], out var userId))
                {
                    context.Items["UserId"] = userId;
                }
            }
            catch { }
        }
    }
    await next();
});

app.Map("/", () =>
{
    return "Pong";
});

app.MapControllers();

app.UseHttpsRedirection();

app.Run();
