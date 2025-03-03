using Newtonsoft.Json;

namespace GithubAnalytics.Json;

public class PullRequestBatch
{
    [JsonProperty("total_count")]
    public int TotalCount { get; set; }

    [JsonProperty("incomplete_results")]
    public bool IncompleteResults { get; set; }

    [JsonProperty("items")]
    public List<Issue> Issues { get; set; }
}

public class Issue
{
	public int Id { get; set; }
	public string Title { get; set; }
	public int Number { get; set; }
	// "open" || "closed"
	public string State { get; set; }

	[JsonProperty("url")]
	public string IssueUrl { get; set; }

	[JsonProperty("pull_request")]
	public PullRequestUrl PullRequest { get; set; }

	[JsonProperty("repository_url")]
	public string RepositoryUrl { get; set; }

	[JsonProperty("created_at")]
	public DateTime CreatedDate { get; set; }

	[JsonProperty("closed_at")]
	public DateTime ClosedDate { get; set; }

	public string FilesUrl => $"{PullRequest.Url}/files";
}

public class PullRequestUrl
{
	public string Url { get; set; }
}
