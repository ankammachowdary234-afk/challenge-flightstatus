using FlightStatus.Api.Models;
using FlightStatus.Api.Providers;
using FlightStatus.Api.Services;
using Moq;

namespace FlightStatus.Tests;

public class FlightStatusServiceTests
{
    private static readonly DateOnly TestDate = new(2024, 6, 15);

    private static ProviderResult MakeResult(string flight, string raw, DateTime lastUpdated, string provider) =>
        new(flight, TestDate, raw,
            new DateTime(2024,6,15,8,0,0,DateTimeKind.Utc), null,
            new DateTime(2024,6,15,11,0,0,DateTimeKind.Utc), null,
            lastUpdated, provider, null, null, null);

    [Fact]
    public async Task BothProviders_ReturnResult_PrefersNewerLastUpdated()
    {
        var older = MakeResult("QF200", "ON_TIME", new DateTime(2024,6,15,6,0,0,DateTimeKind.Utc), "QuickFlight");
        var newer = MakeResult("QF200", "DELAYED", new DateTime(2024,6,15,7,45,0,DateTimeKind.Utc), "AeroTrack");

        var p1 = new Mock<IFlightStatusProvider>();
        p1.Setup(p => p.GetStatusAsync("QF200", TestDate, default)).ReturnsAsync(older);

        var p2 = new Mock<IFlightStatusProvider>();
        p2.Setup(p => p.GetStatusAsync("QF200", TestDate, default)).ReturnsAsync(newer);

        var svc = new FlightStatusService(new[] { p1.Object, p2.Object });
        var result = await svc.GetStatusAsync("QF200", TestDate);

        Assert.Equal(UnifiedStatus.Delayed, result.Status);
        Assert.Equal("AeroTrack", result.ProviderName);
    }

    [Fact]
    public async Task OnlyOneProvider_ReturnsResult_UsesThatResult()
    {
        var r = MakeResult("QF300", "delay", new DateTime(2024,6,15,8,15,0,DateTimeKind.Utc), "QuickFlight");

        var p1 = new Mock<IFlightStatusProvider>();
        p1.Setup(p => p.GetStatusAsync("QF300", TestDate, default)).ReturnsAsync((ProviderResult?)null);

        var p2 = new Mock<IFlightStatusProvider>();
        p2.Setup(p => p.GetStatusAsync("QF300", TestDate, default)).ReturnsAsync(r);

        var svc = new FlightStatusService(new[] { p1.Object, p2.Object });
        var result = await svc.GetStatusAsync("QF300", TestDate);

        Assert.Equal(UnifiedStatus.Delayed, result.Status);
        Assert.Equal("QuickFlight", result.ProviderName);
    }

    [Fact]
    public async Task NeitherProvider_ReturnsResult_StatusIsUnknown()
    {
        var p1 = new Mock<IFlightStatusProvider>();
        p1.Setup(p => p.GetStatusAsync(It.IsAny<string>(), It.IsAny<DateOnly>(), default)).ReturnsAsync((ProviderResult?)null);

        var p2 = new Mock<IFlightStatusProvider>();
        p2.Setup(p => p.GetStatusAsync(It.IsAny<string>(), It.IsAny<DateOnly>(), default)).ReturnsAsync((ProviderResult?)null);

        var svc = new FlightStatusService(new[] { p1.Object, p2.Object });
        var result = await svc.GetStatusAsync("XX999", TestDate);

        Assert.Equal(UnifiedStatus.Unknown, result.Status);
        Assert.NotNull(result.Message);
    }

    [Fact]
    public async Task AeroTrackOnly_OnTimeFlight_ReturnsOnTime()
    {
        var r = MakeResult("AA100", "ON_TIME", new DateTime(2024,6,15,7,50,0,DateTimeKind.Utc), "AeroTrack");

        var p1 = new Mock<IFlightStatusProvider>();
        p1.Setup(p => p.GetStatusAsync("AA100", TestDate, default)).ReturnsAsync(r);

        var p2 = new Mock<IFlightStatusProvider>();
        p2.Setup(p => p.GetStatusAsync("AA100", TestDate, default)).ReturnsAsync((ProviderResult?)null);

        var svc = new FlightStatusService(new[] { p1.Object, p2.Object });
        var result = await svc.GetStatusAsync("AA100", TestDate);

        Assert.Equal(UnifiedStatus.OnTime, result.Status);
        Assert.Equal("AeroTrack", result.ProviderName);
    }
}
