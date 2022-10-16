namespace AsioEchoServer.Configuration
{
	internal class ConfigurationService
	{
		private const string MaxSocketCountVar = "ECHO_SERVER_MAX_SOCKET_COUNT";
		private const int DefaultMaxSocketCount = 500;

		private const string MaxThreadCountVar = "ECHO_SERVER_MAX_THREAD_COUNT";
		private const int DefaultMaxThreadCount = 50;

		private const string ServerPortVar = "ECHO_SERVER_PORT";
		private const int DefaultPort = 11111;

		private const string SocketBufferSizeVar = "ECHO_SERVER_SOCKET_BUFFER_SIZE";
		private const int DefaultSocketBufferSize = 512;

		private const string TaskPollingIntervalVar = "ECHO_SERVER_POLLING_INTERVAL_MS";
		private const int DefaultTaskPollingInterval = 32;

		public Config GetConfiguration()
		{
			return new Config()
			{
				MaxSocketCount = GetMaxSocketCount(),
				MaxThreadCount = GetMaxThreadCount(),
				Port = GetPort(),
				SocketBufferSize = GetBufferSize(),
				TaskPollingInterval = GetInterval(),

			};
		}

		private static int GetMaxSocketCount() => GetIntSystemVariable(MaxSocketCountVar) ?? DefaultMaxSocketCount;

		private static int GetMaxThreadCount() => GetIntSystemVariable(MaxThreadCountVar) ?? DefaultMaxThreadCount;

		private static int GetPort() => GetIntSystemVariable(ServerPortVar) ?? DefaultPort;

		private static int GetBufferSize() => GetIntSystemVariable(SocketBufferSizeVar) ?? DefaultSocketBufferSize;

		private static int GetInterval() => GetIntSystemVariable(TaskPollingIntervalVar) ?? DefaultTaskPollingInterval;



		private static int? GetIntSystemVariable(string variable)
		{
			var envVar = Environment.GetEnvironmentVariable(variable);
			if (!string.IsNullOrEmpty(envVar))
			{
				return Convert.ToInt32(envVar);
			}
			return null;
		}
	}
}
