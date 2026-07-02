using FlightStatus.Api.Models;

namespace FlightStatus.Api.Providers;

public interface IFlightStatusProvider
{
    string ProviderName { get; }
    Task<ProviderResult?> GetStatusAsync(string flightNumber, DateOnly date, CancellationToken ct = default);
}
