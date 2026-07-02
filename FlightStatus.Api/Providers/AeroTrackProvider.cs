using FlightStatus.Api.Models;

namespace FlightStatus.Api.Providers;

public class AeroTrackProvider : IFlightStatusProvider
{
    public string ProviderName => "AeroTrack";

    private static readonly Dictionary<string, ProviderResult> _data = new(StringComparer.OrdinalIgnoreCase)
    {
        ["AA100"] = new("AA100", new DateOnly(2024,6,15), "ON_TIME",
            new DateTime(2024,6,15,8,0,0,DateTimeKind.Utc), new DateTime(2024,6,15,8,5,0,DateTimeKind.Utc),
            new DateTime(2024,6,15,11,0,0,DateTimeKind.Utc), null,
            new DateTime(2024,6,15,7,50,0,DateTimeKind.Utc), "AeroTrack", "T2", "G14", null),

        ["AA200"] = new("AA200", new DateOnly(2024,6,15), "DELAYED",
            new DateTime(2024,6,15,9,0,0,DateTimeKind.Utc), new DateTime(2024,6,15,9,45,0,DateTimeKind.Utc),
            new DateTime(2024,6,15,12,0,0,DateTimeKind.Utc), null,
            new DateTime(2024,6,15,8,55,0,DateTimeKind.Utc), "AeroTrack", "T1", "G3", "Air Traffic Control"),

        ["AA300"] = new("AA300", new DateOnly(2024,6,15), "CANCELLED",
            new DateTime(2024,6,15,10,0,0,DateTimeKind.Utc), null,
            new DateTime(2024,6,15,13,0,0,DateTimeKind.Utc), null,
            new DateTime(2024,6,15,9,30,0,DateTimeKind.Utc), "AeroTrack", "T3", "G20", "Crew unavailable"),

        ["AA400"] = new("AA400", new DateOnly(2024,6,15), "DIVERTED",
            new DateTime(2024,6,15,7,0,0,DateTimeKind.Utc), new DateTime(2024,6,15,7,0,0,DateTimeKind.Utc),
            new DateTime(2024,6,15,10,0,0,DateTimeKind.Utc), new DateTime(2024,6,15,10,30,0,DateTimeKind.Utc),
            new DateTime(2024,6,15,10,35,0,DateTimeKind.Utc), "AeroTrack", "T1", "G7", "Weather"),

        ["AA500"] = new("AA500", new DateOnly(2024,6,15), "UNKNOWN",
            new DateTime(2024,6,15,14,0,0,DateTimeKind.Utc), null,
            new DateTime(2024,6,15,17,0,0,DateTimeKind.Utc), null,
            new DateTime(2024,6,15,13,0,0,DateTimeKind.Utc), "AeroTrack", null, null, null),

        // Merge test: AeroTrack has OLDER timestamp than QuickFlight -> QuickFlight wins
        ["QF100"] = new("QF100", new DateOnly(2024,6,15), "ON_TIME",
            new DateTime(2024,6,15,6,0,0,DateTimeKind.Utc), new DateTime(2024,6,15,6,3,0,DateTimeKind.Utc),
            new DateTime(2024,6,15,9,0,0,DateTimeKind.Utc), null,
            new DateTime(2024,6,15,5,30,0,DateTimeKind.Utc), "AeroTrack", "T4", "G2", null),

        // Merge test: AeroTrack has NEWER timestamp -> AeroTrack wins
        ["QF200"] = new("QF200", new DateOnly(2024,6,15), "DELAYED",
            new DateTime(2024,6,15,7,0,0,DateTimeKind.Utc), new DateTime(2024,6,15,7,50,0,DateTimeKind.Utc),
            new DateTime(2024,6,15,10,0,0,DateTimeKind.Utc), null,
            new DateTime(2024,6,15,7,45,0,DateTimeKind.Utc), "AeroTrack", "T2", "G9", "Late inbound aircraft"),
    };

    public Task<ProviderResult?> GetStatusAsync(string flightNumber, DateOnly date, CancellationToken ct = default)
    {
        _data.TryGetValue(flightNumber, out var result);
        return Task.FromResult(result);
    }
}
