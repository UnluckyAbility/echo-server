using System.Collections.Concurrent;
using System.Net;
using System.Net.Sockets;

namespace AsioEchoServer
{
	internal class SocketDispatcher
	{
		public Task Task { get; }

		private readonly TcpListener _listener;
		private ConcurrentQueue<Socket> _pendingSockets { get; } = new ConcurrentQueue<Socket>();
		private readonly CancellationToken _token;

		public SocketDispatcher(int port, CancellationToken token = default)
		{
			_listener = new TcpListener(IPAddress.Any, port);
			_listener.Start();
			_token = token;
			Task = Task.Factory.StartNew(async () => await AcceptAll(), TaskCreationOptions.LongRunning).Result;
		}

		public Task<Socket> GetSocket(CancellationToken token = default)
		{
			while (!token.IsCancellationRequested)
			{
				if (_pendingSockets.TryDequeue(out var socket))
				{
					return Task.FromResult(socket);
				}
			}
			return Task.FromCanceled<Socket>(token);
		}

		private async Task AcceptAll()
		{
			while (!_token.IsCancellationRequested)
			{
				var socket = await _listener.AcceptSocketAsync(_token);
				_pendingSockets.Enqueue(socket);
			}
		}
	}
}
