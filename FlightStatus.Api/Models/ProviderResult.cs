namespace FlightStatus.Api.Models;

public record ProviderResult(
    string FlightNumber,
    DateOnly Date,
    string RawStatus,
    DateTime ScheduledDeparture,
    DateTime? ActualDeparture,
    DateTime ScheduledArrival,
    DateTime? ActualArrival,
    DateTime LastUpdatedUtc,
    string ProviderName,
    string? Terminal,
    string? Gate,
    string? DelayReason
);
