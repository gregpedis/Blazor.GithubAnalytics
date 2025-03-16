namespace GithubAnalytics.Json;

#pragma warning disable S3604 // Member initializer values should not be redundant
public class UserStatistics(User userDetails, List<PullRequest> pullRequests)
{
	public User UserDetails { get; } = userDetails;

	public List<PullRequest> PullRequests { get; set; } = pullRequests;
	public long TotalPullRequests { get; } = pullRequests.Count;
	public int OpenPullRequests { get; } = pullRequests.Count(x => x.State == PullRequestState.Open);
	public int ClosedPullRequests { get; } = pullRequests.Count(x => x.State == PullRequestState.Closed);

	public long TotalFiles { get; } = pullRequests.Sum(x => x.Files.Count);
	public long TotalFilesAdded { get; } = pullRequests.Sum(x => x.Files.Count(x => x.StatusEnum == PullRequestFileStatus.Added));
	public long TotalFilesDeleted { get; } = pullRequests.Sum(x => x.Files.Count(x => x.StatusEnum == PullRequestFileStatus.Deleted));
	public long TotalFilesModified { get; } = pullRequests.Sum(x => x.Files.Count(x => x.StatusEnum == PullRequestFileStatus.Modified));

	public long TotalAdditions { get; } = pullRequests.Sum(x => x.TotalAdditions);
	public long TotalDeletions { get; } = pullRequests.Sum(x => x.TotalDeletions);
	public long TotalChanges { get; } = pullRequests.Sum(x => x.TotalChanges);
}

public class PullRequest(Issue issue, List<PullRequestFile> files)
{
	public long Id { get; } = issue.Id;
	public string Title { get; } = issue.Title;
	public long Number { get; } = issue.Number;
	public PullRequestState State { get; } = issue.StateEnum;
	public string Url { get; } = issue.PullRequest.Url;
	public string RepositoryUrl { get; } = issue.RepositoryUrl;
	public DateTime CreatedDate { get; } = issue.CreatedDate;
	public DateTime? ClosedDate { get; } = issue.ClosedDate;

	public List<PullRequestFile> Files { get; } = files;
	public long TotalAdditions { get; } = files.Sum(x => x.Additions);
	public long TotalDeletions { get; } = files.Sum(x => x.Deletions);
	public long TotalChanges { get; } = files.Sum(x => x.Changes);
}
