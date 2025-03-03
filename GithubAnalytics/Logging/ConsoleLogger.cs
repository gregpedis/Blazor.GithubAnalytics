namespace GithubAnalytics.Logging;

public class ConsoleLogger : ILogger
{
	private ConsoleLogger() { }
	public static ConsoleLogger Instance { get; } = new();

	public void Log(string prefix, string message) =>
		Console.WriteLine($"[{prefix}] {message}");
}
