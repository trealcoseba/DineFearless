using DineFearless.Components;
using DineFearless.Services;

var builder = WebApplication.CreateBuilder(args);

// Add Blazor services
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

// Register DineFearless Custom Services (OOP State & Strategy)
builder.Services.AddScoped<UserDietaryProfile>();
builder.Services.AddScoped<SafePlateService>();
builder.Services.AddScoped<FavoritesService>();
builder.Services.AddScoped<ModalService>();
builder.Services.AddSingleton<RestaurantDataService>();

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseAntiforgery();

app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

app.Run();