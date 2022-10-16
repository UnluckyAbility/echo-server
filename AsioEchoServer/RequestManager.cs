using System.Collections.Concurrent;

namespace AsioEchoServer
{
	internal class RequestManager
	{
		public Task Task { get; }

		private readonly ConcurrentDictionary<Guid, Task<Guid>> _requests = new();
		private readonly int _pollingDelay;
		private readonly CancellationToken _token;

		public RequestManager(int pollingDelayMs, CancellationToken token = default)
		{
			_pollingDelay = pollingDelayMs;
			_token = token;
			Task = Task.Factory.StartNew(async () => await CompleteRequests(), TaskCreationOptions.LongRunning).Result;
		}

		public bool TryAddRequest(Guid requestId, Task<Guid> request)
		{
			return _requests.TryAdd(requestId, request);
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
				if (_requests.TryRemove(completed.Result, out var _))
				{
					Console.WriteLine($"Request task '{completed.Result}' removed");
				}
				else
				{
					Console.WriteLine($"Failed to remove request task '{completed.Result}'");
				}
			}
		}
	}
}
