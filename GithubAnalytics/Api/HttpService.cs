using GithubAnalytics.Json;
using Newtonsoft.Json;
using System.Diagnostics;
using System.Globalization;

namespace GithubAnalytics.Api;

public class HttpService : IDisposable
{
	const string DATE_MASK = "yyyy-MM-ddTHH:mm:ssZ";
	const int PER_PAGE = 30;
	private const string BASE_URL = "https://api.github.com";

	private readonly JsonSerializerSettings _jsonSettings;
	private readonly HttpClient _client;
	private readonly List<string> _logs;

	public HttpService(string token, List<string> logs)
	{
		_logs = logs;

		_client = new() { BaseAddress = new Uri(BASE_URL) };
		_client.DefaultRequestHeaders.Add("Authorization", $"Bearer {token}");
		_client.DefaultRequestHeaders.Add("User-Agent", Guid.NewGuid().ToString());

		_jsonSettings = new()
		{
			MissingMemberHandling = MissingMemberHandling.Ignore,
			DateFormatString = DATE_MASK,
			Culture = CultureInfo.InvariantCulture,
		};
	}

	/// <summary>
	/// Get the authenticated user's profile information.
	/// </summary>
	public async Task<User> GetUserInfo() =>
		await ExecuteGet<User>("user");

	/// <summary>
	/// Get a user's profile information.
	/// </summary>
	public async Task<User> GetUserInfo(string userId) =>
		await ExecuteGet<User>($"user/{userId}");

	/// <summary>
	/// Gets all Pull Requests that the user is an author of.
	/// </summary>
	public async Task<List<PullRequest>> GetPullRequests(string userId)
	{
		var prs = new List<PullRequest>();

		var page = 1;
		PullRequestBatch batch;
		do
		{
			batch = await ExecuteGet<PullRequestBatch>($"search/issues?per_page={PER_PAGE}&page={page}&q=is:pr+author:{userId}");
			prs.AddRange(batch.PullRequests);
			page++;
		}
		while (batch.PullRequests.Count > 0);

		return prs;
	}

	/// <summary>
	/// Gets the details of a specific Pull Request.
	/// </summary>
	public async Task<PullRequest> GetPullRequestDetails(string owner, string repo, int pull_number) =>
		await ExecuteGet<PullRequest>($"repos/{owner}/{repo}/pulls{pull_number}/files");

	private async Task<T> ExecuteGet<T>(string endpoint)
	{
		var sw = Stopwatch.StartNew();
		Log("HTTP", $"Executing GET at '{endpoint}'.");

		var result = await _client.GetAsync(endpoint);

		Log("HTTP", $"Finished  GET at '{endpoint}' in {sw.Elapsed}. Result: {result.StatusCode}.");
		var value = await result.Content.ReadAsStringAsync();
		return JsonConvert.DeserializeObject<T>(value, _jsonSettings);
	}

	private void Log(string prefix, string message)
	{
		var log = $"[{prefix}] {message}";
		_logs.Add(log);
		Console.WriteLine(log);
	}

	public void Dispose()
	{
		GC.SuppressFinalize(this);
		_client.Dispose();
	}
}
