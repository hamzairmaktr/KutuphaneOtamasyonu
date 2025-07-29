using Blazored.LocalStorage;
using IKitaplik.BlazorUI.Components;
using IKitaplik.BlazorUI.Helpers;
using IKitaplik.BlazorUI.Services.Abstract;
using IKitaplik.BlazorUI.Services.Concrete;
using Microsoft.AspNetCore.Components.Authorization;
using MudBlazor.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddMudServices();

builder.Services.AddAuthorizationCore();
builder.Services.AddScoped<JwtAuthenticationStateProvider>();
builder.Services.AddScoped<AuthenticationStateProvider>(provider => provider.GetRequiredService<JwtAuthenticationStateProvider>());
builder.Services.AddScoped<MessageBoxService>();
builder.Services.AddHttpClient();
builder.Services.AddScoped<IBookService, BookService>();
builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<IWriterService, WriterService>();
builder.Services.AddScoped<ICategoryService, CategoryService>();
builder.Services.AddScoped<IStudentService, StudentService>();
builder.Services.AddScoped<IDepositService, DepositService>();
builder.Services.AddScoped<IDonationService, DonationService>();
builder.Services.AddScoped<IMovementService, MovementService>();

// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

builder.Services.AddBlazoredLocalStorage();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseAntiforgery();
app.MapStaticAssets();


app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

app.Run();
