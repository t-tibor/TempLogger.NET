using MQTTnet;
using MQTTnet.Client;

namespace RaspiTempLogger.Services
{
	public interface IMqttConnector
	{
		Task Publish(Func<MqttApplicationMessageBuilder, MqttApplicationMessage> action,
			CancellationToken cancellationToken = default);
	}

	public class MqttConnector: IMqttConnector
	{
		public async Task Publish(Func<MqttApplicationMessageBuilder, MqttApplicationMessage> action, CancellationToken cancellationToken = default)
		{
			var mqttFactory = new MqttFactory();

			using var mqttClient = mqttFactory.CreateMqttClient();

			var mqttClientOptions = new MqttClientOptionsBuilder()
				.WithTcpServer("broker.hivemq.com")
				.Build();

			await mqttClient.ConnectAsync(mqttClientOptions, cancellationToken);

			var applicationMessage = action(new MqttApplicationMessageBuilder());

			await mqttClient.PublishAsync(applicationMessage, cancellationToken);

			await mqttClient.DisconnectAsync(cancellationToken: cancellationToken);
		}
	}
}
