using docs_project.Application.Interfaces.Repositories;
using docs_project.Application.Interfaces.Services;
using docs_project.Application.Services;
using docs_project.Infrastructure.Csv;
using docs_project.Infrastructure.Data;
using docs_project.Infrastructure.Repositories;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

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

builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.LoginPath = "/Account/Login";
        options.AccessDeniedPath = "/Account/Login";
    });

builder.Services.AddAuthorization();

builder.Services.AddControllersWithViews()
    .AddRazorOptions(options =>
    {
        options.ViewLocationFormats.Clear();
        options.ViewLocationFormats.Add("/Presentation/Views/{1}/{0}.cshtml");
        options.ViewLocationFormats.Add("/Presentation/Views/Shared/{0}.cshtml");
    })
    .AddJsonOptions(o =>
    {
        o.JsonSerializerOptions.ReferenceHandler = System.Text.Json.Serialization.ReferenceHandler.IgnoreCycles;
        o.JsonSerializerOptions.MaxDepth = 64;
    });

var app = builder.Build();

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseAuthentication();
app.UseAuthorization();

app.Map("/", () =>
{
    return Results.Redirect("/Home");
});

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
