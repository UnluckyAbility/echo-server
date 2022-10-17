using System.Net.Sockets;

namespace AsioEchoServer
{
	internal class EchoService
	{
		public Task Task { get; }

		private readonly SocketDispatcher _socketListener;
		private readonly RequestManager _requestManager;
		private readonly int _socketBufferSize;
		private readonly CancellationToken _token;

		public EchoService(SocketDispatcher socketListener, RequestManager requestManager, int socketBufferSize, CancellationToken token = default)
		{
			_socketListener = socketListener;
			_requestManager = requestManager;
			_socketBufferSize = socketBufferSize;
			_token = token;
			Task = Task.Factory.StartNew(DoEcho);
		}

		private async Task DoEcho()
		{
			while (!_token.IsCancellationRequested)
			{
				var socket = await _socketListener.GetSocket(_token);
				var requestId = Guid.NewGuid();
				await _requestManager.TryAddRequest(requestId, Task.Run(async () => await EchoResponse(requestId, socket)));
			}
		}

		private async Task<Guid> EchoResponse(Guid requestId, Socket socket)
		{
			using var taskSocket = socket;

			// NOTE: Naive content handling - return back no more than fixed amount of bytes
			var buffer = new byte[_socketBufferSize];
			var receivedCount = _socketBufferSize;

			while (receivedCount > 0 && !_token.IsCancellationRequested)
				try
				{
					receivedCount = await taskSocket.ReceiveAsync(buffer, SocketFlags.None, _token);
					await taskSocket.SendAsync(buffer.AsMemory(0, receivedCount), receivedCount > buffer.Length ? SocketFlags.Truncated : SocketFlags.None, _token);
				}
				catch (SocketException)
				{
					break;
				}


			return requestId;
		}
	}
}
