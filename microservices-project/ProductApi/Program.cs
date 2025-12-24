using ProductApi.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddScoped<IProductService, ProductService>();

builder.Logging.ClearProviders();
builder.Logging.AddConsole();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// API Key Authentication Middleware
app.Use(async (context, next) =>
{
    // Skip authentication for health check endpoint
    if (context.Request.Path.StartsWithSegments("/api/product/health"))
    {
        await next();
        return;
    }

    var apiKey = Environment.GetEnvironmentVariable("API_KEY");
    
    // If API_KEY is configured, validate it
    if (!string.IsNullOrEmpty(apiKey))
    {
        var requestApiKey = context.Request.Headers["X-API-Key"].FirstOrDefault();
        
        if (string.IsNullOrEmpty(requestApiKey))
        {
            context.Response.StatusCode = 401;
            await context.Response.WriteAsJsonAsync(new { error = "API Key is required. Include X-API-Key header." });
            return;
        }

        if (requestApiKey != apiKey)
        {
            context.Response.StatusCode = 403;
            await context.Response.WriteAsJsonAsync(new { error = "Invalid API Key" });
            return;
        }
    }

    await next();
});

app.UseAuthorization();
app.MapControllers();

var port = Environment.GetEnvironmentVariable("PORT") ?? "8080";
app.Urls.Add($"http://0.0.0.0:{port}");

Console.WriteLine($"ProductApi started on port {port}");
Console.WriteLine($"API Key authentication: {(!string.IsNullOrEmpty(Environment.GetEnvironmentVariable("API_KEY")) ? "Enabled" : "Disabled")}");
Console.WriteLine($"Database: {Environment.GetEnvironmentVariable("DB_HOST") ?? "In-Memory (no database configured)"}");
app.Run();
