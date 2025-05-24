using Microsoft.EntityFrameworkCore;
using FeedbackAPI.Models;
using SharedModels.Models;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "Feedback API", Version = "v1" });
});

// Configurar DbContext
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection") ?? 
        "Data Source=feedback.db"));

// Configurar HttpClient para JsonPlaceholder
builder.Services.AddHttpClient("JsonPlaceholder", client =>
    client.BaseAddress = new Uri("https://jsonplaceholder.typicode.com/"));

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "Feedback API v1"));
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

// Crear la base de datos si no existe
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    try
    {
        var context = services.GetRequiredService<ApplicationDbContext>();
        context.Database.EnsureCreated();
    }
    catch (Exception ex)
    {
        var logger = services.GetRequiredService<ILogger<Program>>();
        logger.LogError(ex, "Error al crear la base de datos");
    }
}

app.Run();
