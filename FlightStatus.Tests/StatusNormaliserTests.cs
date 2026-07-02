using FlightStatus.Api.Models;
using FlightStatus.Api.Services;

namespace FlightStatus.Tests;

public class StatusNormaliserTests
{
    private static ProviderResult Make(string rawStatus, DateTime scheduled, DateTime? actual = null) =>
        new("TST001", new DateOnly(2024,6,15), rawStatus,
            scheduled, actual,
            scheduled.AddHours(3), null,
            DateTime.UtcNow, "Test", null, null, null);

    [Theory]
    [InlineData("ON_TIME",  UnifiedStatus.OnTime)]
    [InlineData("DELAYED",  UnifiedStatus.Delayed)]
    [InlineData("CANCELLED",UnifiedStatus.Cancelled)]
    [InlineData("DIVERTED", UnifiedStatus.Diverted)]
    [InlineData("UNKNOWN",  UnifiedStatus.Unknown)]
    [InlineData("ontime",   UnifiedStatus.OnTime)]
    [InlineData("delay",    UnifiedStatus.Delayed)]
    [InlineData("cancel",   UnifiedStatus.Cancelled)]
    [InlineData("divert",   UnifiedStatus.Diverted)]
    public void Normalise_KnownRawStatus_ReturnsExpected(string raw, UnifiedStatus expected)
    {
        var r = Make(raw, new DateTime(2024,6,15,8,0,0,DateTimeKind.Utc));
        Assert.Equal(expected, StatusNormaliser.Normalise(r));
    }

    [Fact]
    public void Normalise_ActualWithin15Min_ReturnsOnTime()
    {
        var sched = new DateTime(2024,6,15,8,0,0,DateTimeKind.Utc);
        var r = Make("SOMETHING_ELSE", sched, sched.AddMinutes(10));
        Assert.Equal(UnifiedStatus.OnTime, StatusNormaliser.Normalise(r));
    }

    [Fact]
    public void Normalise_ActualBeyond15Min_ReturnsDelayed()
    {
        var sched = new DateTime(2024,6,15,8,0,0,DateTimeKind.Utc);
        var r = Make("SOMETHING_ELSE", sched, sched.AddMinutes(20));
        Assert.Equal(UnifiedStatus.Delayed, StatusNormaliser.Normalise(r));
    }

    [Fact]
    public void Normalise_ExactlyAtBoundary15Min_ReturnsOnTime()
    {
        var sched = new DateTime(2024,6,15,8,0,0,DateTimeKind.Utc);
        var r = Make("SOMETHING_ELSE", sched, sched.AddMinutes(15));
        Assert.Equal(UnifiedStatus.OnTime, StatusNormaliser.Normalise(r));
    }
}
