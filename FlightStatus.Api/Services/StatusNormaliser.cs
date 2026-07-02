using FlightStatus.Api.Models;

namespace FlightStatus.Api.Services;

public static class StatusNormaliser
{
    public static UnifiedStatus Normalise(ProviderResult result)
    {
        var status = result.RawStatus.ToUpperInvariant() switch
        {
            "ON_TIME" or "ONTIME"   => UnifiedStatus.OnTime,
            "DELAYED" or "DELAY"    => UnifiedStatus.Delayed,
            "CANCELLED" or "CANCEL" => UnifiedStatus.Cancelled,
            "DIVERTED" or "DIVERT"  => UnifiedStatus.Diverted,
            "UNKNOWN"               => UnifiedStatus.Unknown,
            _                       => ComputeFromTimes(result)
        };
        return status;
    }

    private static UnifiedStatus ComputeFromTimes(ProviderResult r)
    {
        var departure = r.ActualDeparture ?? r.ScheduledDeparture;
        var depDiff = Math.Abs((departure - r.ScheduledDeparture).TotalMinutes);

        if (r.ActualArrival.HasValue)
        {
            var arrDiff = Math.Abs((r.ActualArrival.Value - r.ScheduledArrival).TotalMinutes);
            if (arrDiff > 15) return UnifiedStatus.Delayed;
        }

        return depDiff <= 15 ? UnifiedStatus.OnTime : UnifiedStatus.Delayed;
    }
}
