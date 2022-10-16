namespace AsioEchoServer.Configuration
{
	internal class ConfigurationService
	{
		private const string ServerPortVar = "ECHO_SERVER_PORT";
		private const int DefaultPort = 11111;

		private const string TaskPollingIntervalVar = "ECHO_SERVER_POLING_INTERVAL_MS";
		private const int DefaultTaskPollingInterval = 32;

		public Config GetConfiguration()
		{
			return new Config()
			{
				Port = GetPort(),
				TaskPollingInterval = GetInterval(),
			};
		}

		private static int GetPort() => GetIntSystemVariable(ServerPortVar) ?? DefaultPort;

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
