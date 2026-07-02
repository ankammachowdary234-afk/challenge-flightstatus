using System.Text.Json.Serialization;

namespace FlightStatus.Api.Models;

[JsonConverter(typeof(JsonStringEnumConverter))]
public enum UnifiedStatus
{
    OnTime,
    Delayed,
    Cancelled,
    Diverted,
    Unknown
}
