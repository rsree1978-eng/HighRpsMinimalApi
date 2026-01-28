using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Hosting;

var builder = WebApplication.CreateBuilder(args);

builder.WebHost.ConfigureKestrel(options =>
{
    options.Limits.MaxConcurrentConnections = 100_000;
});

// 🧠 In-memory cache
builder.Services.AddMemoryCache();

var app = builder.Build();

var random = Random.Shared;
const string PriceCacheKey = "price_cache_key";
var cacheTtl = TimeSpan.FromSeconds(1);

// 🔹 PRICE ENDPOINT (cached + 50ms async delay)
app.MapGet("/price", async (
    IMemoryCache cache,
    CancellationToken ct) =>
{
    // Simulate downstream I/O latency (non-blocking)
    await Task.Delay(50, ct);

    if (!cache.TryGetValue(PriceCacheKey, out int price))
    {
        price = random.Next(10, 1000);
        cache.Set(PriceCacheKey, price, cacheTtl);
    }

    return Results.Ok(new { price });
});

// 🔹 CALCULATOR ENDPOINT (50ms async delay)
app.MapGet("/calculate", async (
    int a,
    int b,
    string op,
    CancellationToken ct) =>
{
    await Task.Delay(50, ct);

    return op.ToLower() switch
    {
        "add" => Results.Ok(a + b),
        "sub" => Results.Ok(a - b),
        "mul" => Results.Ok(a * b),
        "div" when b != 0 => Results.Ok((double)a / b),
        _ => Results.BadRequest("Invalid operation")
    };
});

// 🔹 Health check
app.MapGet("/health", () => Results.Ok("Healthy"));

app.Run();