var builder = WebApplication.CreateBuilder(args);
builder.Services.AddScoped<Biometrico.Web.Datos.ConexionBD>();

// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

var app = builder.Build();

//prueba de endpoint
app.MapGet("/api/prueba-conexion", async (Biometrico.Web.Datos.ConexionBD conexionBD) =>
{
    try
    {
        await conexionBD.ProbarConexionAsync();
        return Results.Ok("Conexión exitosa con SQL Server y BioReloj.");
    }
    catch (Exception ex)
    {
        return Results.Problem($"Error de conexión: {ex.Message}");
    }
});

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

var summaries = new[]
{
    "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
};

app.MapGet("/weatherforecast", () =>
{
    var forecast = Enumerable.Range(1, 5).Select(index =>
        new WeatherForecast
        (
            DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
            Random.Shared.Next(-20, 55),
            summaries[Random.Shared.Next(summaries.Length)]
        ))
        .ToArray();
    return forecast;
})
.WithName("GetWeatherForecast");

app.Run();

record WeatherForecast(DateOnly Date, int TemperatureC, string? Summary)
{
    public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);
}
