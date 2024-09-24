using RaspiTempLogger.Services;

public class FakeBmeSensorMode : IBmeSensorModel
{
    public Task<BmeMeasDataDto> ReadSensor(CancellationToken cancellationToken = default)
    {
        return Task.FromResult(
            new BmeMeasDataDto(
                TempDegC: 20.0 + 10.0 * Random.Shared.NextDouble(),
                PressMbar: 1000.0 + 100.0 * Random.Shared.NextDouble(),
                HumP: 40.0 + 20.0 * Random.Shared.NextDouble()
            )
        );
    }
}