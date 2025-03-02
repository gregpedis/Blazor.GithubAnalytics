using Newtonsoft.Json;

namespace GithubAnalytics.Json;

public class PullRequestBatch
{
    [JsonProperty("total_count")]
    public int TotalCount { get; set; }

    [JsonProperty("incomplete_results")]
    public bool IncompleteResults { get; set; }

    [JsonProperty("items")]
    public List<PullRequest> PullRequests { get; set; }
}

public class PullRequest
{
    public int Id { get; set; }
}
