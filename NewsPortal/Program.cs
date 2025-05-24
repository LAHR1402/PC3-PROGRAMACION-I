using Microsoft.Extensions.DependencyInjection;
using SharedModels.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

// Configurar HttpClient para JsonPlaceholder
builder.Services.AddHttpClient("JsonPlaceholder", client =>
    client.BaseAddress = new Uri("https://jsonplaceholder.typicode.com/"));

// Configurar servicio de JsonPlaceholder
builder.Services.AddScoped<JsonPlaceholderService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
