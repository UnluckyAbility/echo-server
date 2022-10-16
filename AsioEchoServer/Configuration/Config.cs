namespace AsioEchoServer.Configuration
{
	internal class Config
	{
		public int Port { get; set; }

		public int TaskPollingInterval { get; set; }

		public int MaxThreadCount { get; set; }

		public int MaxSocketCount { get; set; }

		public int SocketBufferSize { get; set; }
	}
}
