using System.ComponentModel.DataAnnotations;

namespace RaspiTempLogger.Services
{
    public record BmeConfig
	{
		[Required]
		public string? SensorName { get; set; }

		[Range(1, int.MaxValue)]
		public int I2CBusId { get; set; }

		[Range(1, 255)]
		public int I2CAddress { get; set; }
	}
}
