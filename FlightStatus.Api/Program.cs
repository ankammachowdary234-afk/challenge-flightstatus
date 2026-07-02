using FlightStatus.Api.Providers;
using FlightStatus.Api.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddTransient<IFlightStatusProvider, AeroTrackProvider>();
builder.Services.AddTransient<IFlightStatusProvider, QuickFlightProvider>();
builder.Services.AddTransient<FlightStatusService>();

builder.Services.AddCors(options =>
    options.AddDefaultPolicy(p => p.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod()));

var app = builder.Build();
app.UseCors();

app.MapGet("/flights/status", async (string? flightNumber, string? date,
    FlightStatusService svc, CancellationToken ct) =>
{
    if (string.IsNullOrWhiteSpace(flightNumber))
        return Results.BadRequest(new { error = "flightNumber is required." });

    if (string.IsNullOrWhiteSpace(date))
        return Results.BadRequest(new { error = "date is required." });

    if (!DateOnly.TryParseExact(date, "yyyy-MM-dd", out var parsedDate))
        return Results.BadRequest(new { error = "date must be in yyyy-MM-dd format." });

    var result = await svc.GetStatusAsync(flightNumber, parsedDate, ct);
    return Results.Ok(result);
});

app.Run();

public partial class Program { }
