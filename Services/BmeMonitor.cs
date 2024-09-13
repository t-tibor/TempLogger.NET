using System.ComponentModel.DataAnnotations;
using Iot.Device.Bmxx80;
using System.Device.I2c;
using Quartz;
using InfluxDB.Client.Api.Domain;
using InfluxDB.Client.Writes;
using Microsoft.Extensions.Options;

namespace RaspiTempLogger.Services
{
	public class BmeMonitor(
		ILogger<BmeMonitor> logger,
		IOptions<BmeMonitorConfig> configOptions,
		IInfluxDbConnector influxDbConnector,
		IMqttConnector mqttConnector)
		: IJob
	{
		private readonly BmeMonitorConfig _config = configOptions.Value;

		public async Task Execute(IJobExecutionContext context)
		{
			try
			{
				logger.LogDebug("Reading data from BME sensor with name: {SensorName}.", _config.SensorName );

				var i2CSettings = new I2cConnectionSettings(_config.I2CBusId, _config.I2CAddress);
				using var i2CDevice = I2cDevice.Create(i2CSettings);
				using var bme80 = new Bme280(i2CDevice);

				// set higher sampling
				bme80.TemperatureSampling = Sampling.LowPower;
				bme80.PressureSampling = Sampling.UltraHighResolution;
				bme80.HumiditySampling = Sampling.Standard;

				// Perform a synchronous measurement
				var readResult = await bme80.ReadAsync();

				double temp = readResult.Temperature?.DegreesCelsius ?? double.NaN;
				double press = readResult.Pressure?.Millibars ?? double.NaN;
				double hum = readResult.Humidity?.Percent ?? double.NaN;

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

				logger.LogTrace("Temperature: {Temperature}\u00B0C",
					readResult.Temperature?.DegreesCelsius.ToString("0.#"));
				logger.LogTrace("Pressure: {Pressure}hPa", readResult.Pressure?.Hectopascals.ToString("0.##"));
				logger.LogTrace("Relative humidity: {Humidity}%", readResult.Humidity?.Percent.ToString("0.#"));
			}
			catch (Exception e)
			{
				Console.WriteLine(e);
				throw;
			}
		}
	}

	public record BmeMonitorConfig
	{
		[Required]
		public string? SensorName { get; set; }

		[Range(1, int.MaxValue)]
		public int I2CBusId { get; set; }

		[Range(1, 255)]
		public int I2CAddress { get; set; }
	}
}
