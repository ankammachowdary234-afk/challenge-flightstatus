using FlightStatus.Api.Models;
using FlightStatus.Api.Providers;

namespace FlightStatus.Api.Services;

public class FlightStatusService
{
    private readonly IEnumerable<IFlightStatusProvider> _providers;

    public FlightStatusService(IEnumerable<IFlightStatusProvider> providers)
    {
        _providers = providers;
    }

    public async Task<FlightStatusResult> GetStatusAsync(string flightNumber, DateOnly date, CancellationToken ct = default)
    {
        var tasks = _providers.Select(p => p.GetStatusAsync(flightNumber, date, ct));
        var results = await Task.WhenAll(tasks);

        var validResults = results.Where(r => r is not null).Cast<ProviderResult>().ToList();

        if (validResults.Count == 0)
        {
            return new FlightStatusResult(
                flightNumber, date.ToString("yyyy-MM-dd"),
                UnifiedStatus.Unknown,
                null, null, null, null,
                null, null, null,
                "No flight data available from any provider.",
                DateTime.UtcNow, "None");
        }

        var best = validResults.OrderByDescending(r => r.LastUpdatedUtc).First();
        var unified = StatusNormaliser.Normalise(best);

        return new FlightStatusResult(
            best.FlightNumber,
            best.Date.ToString("yyyy-MM-dd"),
            unified,
            best.ScheduledDeparture,
            best.ActualDeparture,
            best.ScheduledArrival,
            best.ActualArrival,
            best.Terminal,
            best.Gate,
            best.DelayReason,
            null,
            best.LastUpdatedUtc,
            best.ProviderName);
    }
}
