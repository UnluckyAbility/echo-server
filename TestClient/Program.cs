using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace TestClient
{
	internal class Program
	{
		static async Task Main(string[] args)
		{
			var threadCount = 5000;
			var tasks = new Task[threadCount];
			for (var i = 0; i < threadCount; ++i)
			{
				tasks[i] = Task.Run(async () =>
				{
					var requestId = Guid.NewGuid();

					using var endpointSocket = new Socket(SocketType.Stream, ProtocolType.Tcp);
					var ipe = new IPEndPoint(IPAddress.Parse("127.0.0.1"), 11111);
					await endpointSocket.ConnectAsync(ipe);

					var rnd = new Random();
					var payloadSize = rnd.Next(20, 256);
					var payload = GenerateString(payloadSize);
					var payloadBuffer = System.Text.Encoding.UTF8.GetBytes(payload);

					Console.WriteLine($"{requestId}\tSending the '{payload}' bytes...");
					await endpointSocket.SendAsync(payloadBuffer, SocketFlags.None);
					Console.WriteLine($"{requestId}\tSended\n");

					Console.WriteLine($"{requestId}\tReceiving echo response...");
					var responseBytes = new byte[payloadBuffer.Length];
					var responseLength = await endpointSocket.ReceiveAsync(responseBytes, SocketFlags.None);
					Console.WriteLine($"{requestId}\tReceived response with size of '{responseLength}', request size was '{payloadBuffer.Length}'");

					var response = System.Text.Encoding.UTF8.GetString(responseBytes);
					Assert.AreEqual(payload, response, $"{requestId}\tResponse != request");

					Console.WriteLine($"{requestId}\tResponse: '{response}'");
				});
			}
			await Task.WhenAll(tasks);
		}

		static string GenerateString(int length)
		{
			var builder = new StringBuilder(length);
			var chars = "0123456789ABCDEF";
			var rnd = new Random();
			for (int i = 0; i < length; ++i)
			{
				builder.Append(chars[rnd.Next(chars.Length)]);
			}
			return builder.ToString();
		}
	}
}