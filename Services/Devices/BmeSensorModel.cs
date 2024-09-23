using Iot.Device.Bmxx80;
using System.Device.I2c;
using Microsoft.Extensions.Options;

namespace RaspiTempLogger.Services
{
    public class BmeSensorModel: IBmeSensorModel
    {
        private readonly I2cConnectionSettings i2CSettings;
		private readonly I2cDevice i2CDevice;
		private readonly Bme280 bme280;
        private readonly ILogger<BmeSensorModel> logger;
        private readonly BmeConfig config;

        public BmeSensorModel(
        ILogger<BmeSensorModel> logger,
        IOptions<BmeConfig> configOptions)
        {
			this.logger = logger;
            this.config = configOptions.Value;

			i2CSettings = new I2cConnectionSettings(config.I2CBusId, config.I2CAddress);
			i2CDevice = I2cDevice.Create(i2CSettings);
			bme280 = new Bme280(i2CDevice);
        }
		

        public async Task<BmeMeasDataDto> ReadSensor(CancellationToken cancellationToken)
		{
			logger.LogDebug("Reading data from BME sensor with name: {SensorName}.", config.SensorName );

			try
			{
				// set higher sampling
				bme280.TemperatureSampling = Sampling.LowPower;
				bme280.PressureSampling = Sampling.UltraHighResolution;
				bme280.HumiditySampling = Sampling.Standard;

				// Perform a synchronous measurement
				var readResult = await bme280.ReadAsync();

				double temp = readResult.Temperature?.DegreesCelsius ?? double.NaN;
				double press = readResult.Pressure?.Millibars ?? double.NaN;
				double hum = readResult.Humidity?.Percent ?? double.NaN;

				logger.LogTrace("Temperature: {Temperature}\u00B0C",
					readResult.Temperature?.DegreesCelsius.ToString("0.#"));
				logger.LogTrace("Pressure: {Pressure}hPa", readResult.Pressure?.Hectopascals.ToString("0.##"));
				logger.LogTrace("Relative humidity: {Humidity}%", readResult.Humidity?.Percent.ToString("0.#"));
				return new BmeMeasDataDto(temp, press, hum);
			}
			catch (Exception e)
			{
				Console.WriteLine(e);
				throw;
			}
		}
	}
}
