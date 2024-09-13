using InfluxDB.Client;

namespace RaspiTempLogger.Services;

public interface IInfluxDbConnector
{
	void Write(Action<WriteApi> action);

	Task<T> QueryAsync<T>(Func<QueryApi, Task<T>> action);
}