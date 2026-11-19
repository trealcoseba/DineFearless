using DineFearless.Components;
using DineFearless.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

// Connect to Supabase using appsettings.json
var url = builder.Configuration["Supabase:Url"]!;
var key = builder.Configuration["Supabase:Key"]!;
var supabase = new Supabase.Client(url, key,
    new Supabase.SupabaseOptions { AutoConnectRealtime = false });
await supabase.InitializeAsync();
builder.Services.AddSingleton(supabase);

// Register your existing services
builder.Services.AddScoped<UserDietaryProfile>();
builder.Services.AddScoped<SafePlateService>();
builder.Services.AddScoped<FavoritesService>();
builder.Services.AddScoped<ModalService>();
builder.Services.AddScoped<RestaurantDataService>();
builder.Services.AddScoped<AuditDataService>();

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