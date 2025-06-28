using IKitaplik.BlazorUI.Components;
using IKitaplik.BlazorUI.Helpers;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Server.ProtectedBrowserStorage;
using MudBlazor.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddScoped<ProtectedSessionStorage>();

builder.Services.AddMudServices();

builder.Services.AddScoped<AuthMessageHandler>();

builder.Services.AddHttpClient("DefaultClient", client =>
{
    client.BaseAddress = new Uri("http://localhost:5000/api/");
}).AddHttpMessageHandler<AuthMessageHandler>();

builder.Services.AddCascadingAuthenticationState();
builder.Services.AddHttpContextAccessor();


builder.Services.AddScoped<AuthenticationStateProvider, JwtAuthenticationStateProvider>();
builder.Services.AddAuthorizationCore();

builder.Services.AddScoped<JwtAuthenticationStateProvider>();
builder.Services.AddScoped<AuthenticationStateProvider>(provider => provider.GetRequiredService<JwtAuthenticationStateProvider>());


// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();


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
