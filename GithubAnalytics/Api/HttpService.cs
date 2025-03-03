using GithubAnalytics.Json;
using GithubAnalytics.Logging;
using Newtonsoft.Json;
using System.Diagnostics;
using System.Globalization;

namespace GithubAnalytics.Api;

#pragma warning disable S1075 // URIs should not be hardcoded
public sealed class HttpService : IDisposable
{
	const string DATE_MASK = "yyyy-MM-ddTHH:mm:ssZ";
	const int PER_PAGE = 30;
	private const string BASE_URL = "https://api.github.com";

	private readonly ILogger _logger;
	private readonly HttpClient _client;
	private readonly JsonSerializerSettings _jsonSettings;

	public HttpService(string token, ILogger logger)
	{
		_logger = logger;
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
	///  Get the user statistics for the specified user, or the current user if no userId is specified.
	/// </summary>
	public async Task<UserStatistics> GetUserStatistics(string userId = null)
	{
		var user = await (userId is null ? GetUserInfo() : GetUserInfo(userId));
		var issues = await GetPullRequests(user.Name);

		var pullRequests = new List<PullRequest>();
		foreach (var issue in issues)
		{
			var files = await GetPullRequestFiles(issue);
			pullRequests.Add(new(issue, files));
		}
		return new(user, pullRequests);
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
	public async Task<List<Issue>> GetPullRequests(string username)
	{
		var prs = new List<Issue>();

		var page = 1;
		PullRequestBatch batch;
		do
		{
			batch = await ExecuteGet<PullRequestBatch>($"search/issues?per_page={PER_PAGE}&page={page}&q=is:pr+author:{username}");
			prs.AddRange(batch.Issues);
			page++;
		}
		while (batch.Issues.Count > 0);

		return prs;
	}

	/// <summary>
	/// Gets the files of a specific Pull Request.
	/// </summary>
	public async Task<List<PullRequestFile>> GetPullRequestFiles(Issue issue) =>
		await ExecuteGet<List<PullRequestFile>>(issue.FilesUrl[BASE_URL.Length..]);

	private async Task<T> ExecuteGet<T>(string endpoint)
	{
		var sw = Stopwatch.StartNew();
		_logger.Log("HTTP", $"Executing GET at '{endpoint}'.");

		var result = await _client.GetAsync(endpoint);

		_logger.Log("HTTP", $"Finished  GET at '{endpoint}' in {sw.Elapsed}. Result: {result.StatusCode}.");
		var value = await result.Content.ReadAsStringAsync();
		return JsonConvert.DeserializeObject<T>(value, _jsonSettings);
	}

	public void Dispose() =>
		_client.Dispose();
}
