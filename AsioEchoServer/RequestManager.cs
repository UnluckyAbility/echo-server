using System.Collections.Concurrent;

namespace AsioEchoServer
{
	internal class RequestManager
	{
		public Task Task { get; }

		private readonly ConcurrentDictionary<Guid, Task<Guid>> _requests = new();
		private readonly int _pollingDelay;
		private readonly int _maxRequestCount;
		private readonly CancellationToken _token;

		public RequestManager(int pollingDelayMs, int maxRequestCount,  CancellationToken token = default)
		{
			_pollingDelay = pollingDelayMs;
			_maxRequestCount = maxRequestCount;
			_token = token;
			Task = Task.Factory.StartNew(async () => await CompleteRequests(), TaskCreationOptions.LongRunning).Result;
		}

		public async Task<bool> TryAddRequest(Guid requestId, Task<Guid> request, CancellationToken token = default)
		{
			while (!token.IsCancellationRequested)
			{
				var reqCount = _requests.Count;
				if (reqCount < _maxRequestCount)
				{
					return _requests.TryAdd(requestId, request);
				}
				else
				{
					await Task.Delay(1, token);
				}
			}
			return false;
		}

		private async Task CompleteRequests()
		{
			while (!_token.IsCancellationRequested)
			{
				var currentTasks = _requests.Values.ToArray();
				if (currentTasks.Length == 0)
				{
					await Task.Delay(_pollingDelay, _token);
					continue;
				}

				var completed = await Task.WhenAny(currentTasks);
				if (!_requests.TryRemove(completed.Result, out var _))
				{
					Console.WriteLine($"Failed to remove request task '{completed.Result}'");
				}
			}
		}
	}
}
