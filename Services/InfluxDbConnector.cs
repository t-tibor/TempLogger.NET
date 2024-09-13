using System.ComponentModel.DataAnnotations;
using InfluxDB.Client;
using Microsoft.Extensions.Options;

namespace RaspiTempLogger.Services;

public class InfluxDbConnector(IOptions<InfluxDbConfig> configOptions) : IInfluxDbConnector
{
	public void Write(Action<WriteApi> action)
	{
		using var client = new InfluxDBClient(configOptions.Value.Url, configOptions.Value.User, configOptions.Value.Password);
		using var write = client.GetWriteApi();
		action(write);
	}

	public async Task<T> QueryAsync<T>(Func<QueryApi, Task<T>> action)
	{
		using var client = new InfluxDBClient(configOptions.Value.Url, configOptions.Value.User, configOptions.Value.Password);
		var query = client.GetQueryApi();
		return await action(query);
	}
}

public record InfluxDbConfig
{
	[Required]
	[Url]
	public string? Url { get; set; }

	[Required]
	public string? User { get; set; }

	[Required]
	public string? Password { get; set; }
}