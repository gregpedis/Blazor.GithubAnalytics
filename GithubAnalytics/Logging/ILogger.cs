namespace GithubAnalytics.Logging;

public interface ILogger
{
	void Log(string prefix, string message);
}
