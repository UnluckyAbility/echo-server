using System.Net.Sockets;

namespace AsioEchoServer
{
	internal class EchoService
	{
		public Task Task { get; }

		private readonly SocketListener _socketListener;
		private readonly RequestManager _requestManager;
		private readonly CancellationToken _token;

		public EchoService(SocketListener socketListener, RequestManager requestManager, CancellationToken token = default)
		{
			_socketListener = socketListener;
			_requestManager = requestManager;
			_token = token;
			Task = Task.Factory.StartNew(DoEcho);
		}

		private async Task DoEcho()
		{
			while (!_token.IsCancellationRequested)
			{
				var socket = await _socketListener.GetSocket(_token);
				var requestId = Guid.NewGuid();
				_requestManager.TryAddRequest(requestId, Task.Run(async () => await EchoResponse(requestId, socket)));	
			}
		}

		private async Task<Guid> EchoResponse(Guid requestId, Socket socket)
		{
			using var taskSocket = socket;
			Console.WriteLine($"{requestId}\tConnected");

			// NOTE: Naive content handling - return back no more than fixed amount of bytes
			var buffer = new byte[512];
			Console.WriteLine($"{requestId}\tReceiving request...");
			var receivedCount = await taskSocket!.ReceiveAsync(buffer, SocketFlags.None, _token);

			Console.WriteLine($"{requestId}\tGot request, sending back...");
			await taskSocket.SendAsync(buffer, receivedCount > buffer.Length ? SocketFlags.Truncated : SocketFlags.None, _token);
			Console.WriteLine($"{requestId}\tDone");

			return requestId;
		}
	}
}
