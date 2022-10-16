using AsioEchoServer.Configuration;

namespace AsioEchoServer
{
	internal class Program
	{
		static async Task Main()
		{
			var config = new ConfigurationService().GetConfiguration();
			var tokenSource = new CancellationTokenSource();

			var listener = new SocketDispatcher(config.Port, tokenSource.Token);
			var requestManager = new RequestManager(config.TaskPollingInterval, tokenSource.Token);

			var echoService = new EchoService(listener, requestManager, tokenSource.Token);
			await Task.WhenAll(listener.Task, echoService.Task, requestManager.Task);
		}
	}
}