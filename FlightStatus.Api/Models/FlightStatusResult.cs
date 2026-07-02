namespace FlightStatus.Api.Models;

public record FlightStatusResult(
    string FlightNumber,
    string Date,
    UnifiedStatus Status,
    DateTime? ScheduledDeparture,
    DateTime? ActualDeparture,
    DateTime? ScheduledArrival,
    DateTime? ActualArrival,
    string? Terminal,
    string? Gate,
    string? DelayReason,
    string? Message,
    DateTime LastUpdatedUtc,
    string ProviderName
);
