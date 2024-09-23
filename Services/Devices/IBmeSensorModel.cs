namespace RaspiTempLogger.Services
{
    public interface IBmeSensorModel
	{
		Task<BmeMeasDataDto> ReadSensor(CancellationToken cancellationToken = default);
	}

	public record BmeMeasDataDto(
		double TempDegC,
		double PressMbar,
		double HumP
	);
}
