using Quartz;
using InfluxDB.Client.Api.Domain;
using InfluxDB.Client.Writes;

namespace RaspiTempLogger.Services
{



    public class BmeMonitor(
		ILogger<BmeMonitor> logger,
		IInfluxDbConnector influxDbConnector,
		IMqttConnector mqttConnector)
		: IJob
	{
		private readonly BmeMonitorConfig _config = configOptions.Value;

		public async Task Execute(IJobExecutionContext context)
		{

		}
/*
		private void backup()
		{
			influxDbConnector.Write(write =>
				{
					var point = PointData.Measurement(_config.SensorName)
						.Tag("runNum", "1")
						.Timestamp(DateTimeOffset.UtcNow, WritePrecision.Ms)
						.Field("Temperature", temp)
						.Field("Pressure", press)
						.Field("Humidity", hum);

					write.WritePoint(point);
				});

				await mqttConnector.Publish(builder => builder
						.WithTopic("TEst topic")
						.WithPayload(temp.ToString("0.##"))
						.Build()
				);
		}
		*/
	}
}
