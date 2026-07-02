using FlightStatus.Api.Models;

namespace FlightStatus.Api.Providers;

public class QuickFlightProvider : IFlightStatusProvider
{
    public string ProviderName => "QuickFlight";

    private static readonly Dictionary<string, ProviderResult> _data = new(StringComparer.OrdinalIgnoreCase)
    {
        // Merge: AeroTrack is OLDER, QuickFlight is NEWER -> QuickFlight wins
        ["QF100"] = new("QF100", new DateOnly(2024,6,15), "ontime",
            new DateTime(2024,6,15,6,0,0,DateTimeKind.Utc), new DateTime(2024,6,15,6,3,0,DateTimeKind.Utc),
            new DateTime(2024,6,15,9,0,0,DateTimeKind.Utc), null,
            new DateTime(2024,6,15,6,0,0,DateTimeKind.Utc), "QuickFlight", null, null, null),

        // Merge: AeroTrack is NEWER -> AeroTrack wins (QF200 QuickFlight has older timestamp)
        ["QF200"] = new("QF200", new DateOnly(2024,6,15), "ontime",
            new DateTime(2024,6,15,7,0,0,DateTimeKind.Utc), null,
            new DateTime(2024,6,15,10,0,0,DateTimeKind.Utc), null,
            new DateTime(2024,6,15,6,0,0,DateTimeKind.Utc), "QuickFlight", null, null, null),

        // QuickFlight only
        ["QF300"] = new("QF300", new DateOnly(2024,6,15), "delay",
            new DateTime(2024,6,15,8,0,0,DateTimeKind.Utc), new DateTime(2024,6,15,8,30,0,DateTimeKind.Utc),
            new DateTime(2024,6,15,11,0,0,DateTimeKind.Utc), null,
            new DateTime(2024,6,15,8,15,0,DateTimeKind.Utc), "QuickFlight", null, null, null),
    };

    public Task<ProviderResult?> GetStatusAsync(string flightNumber, DateOnly date, CancellationToken ct = default)
    {
        _data.TryGetValue(flightNumber, out var result);
        return Task.FromResult(result);
    }
}
