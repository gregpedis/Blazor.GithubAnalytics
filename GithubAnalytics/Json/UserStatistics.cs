namespace GithubAnalytics.Json;

public class UserStatistics
{
	public User UserDetails { get; }

	public long TotalPullRequests { get; }

	public long TotalFiles { get; }
	public long TotalFilesAdded { get; }
	public long TotalFilesDeleted { get; }
	public long TotalFilesModified { get; }

	public long TotalAdditions { get; }
	public long TotalDeletions { get; }
	public long TotalChanges { get; }

	public List<PullRequest> PullRequests { get; }

	public UserStatistics(User userDetails, List<PullRequest> pullRequests)
	{
		UserDetails = userDetails;

		PullRequests = pullRequests;
		TotalPullRequests = pullRequests.Count;
		PullRequests = new(); // DEBUG: too much noise.

		TotalFiles = pullRequests.Sum(x => x.Files.Count);
		TotalFilesAdded = pullRequests.Sum(x => x.Files.Count);		// TODO: filter with status
		TotalFilesDeleted = pullRequests.Sum(x => x.Files.Count);	// TODO: filter with status
		TotalFilesModified = pullRequests.Sum(x => x.Files.Count);	// TODO: filter with status

		TotalAdditions = pullRequests.Sum(x => x.TotalAdditions);
		TotalDeletions = pullRequests.Sum(x => x.TotalDeletions);
		TotalChanges = pullRequests.Sum(x => x.TotalChanges);
	}
}

public class PullRequest
{
	public long Id { get; }
	public string Title { get; }
	public long Number { get; }
	public string State { get; }
	public string Url { get; }
	public string RepositoryUrl { get; }
	public DateTime CreatedDate { get; }
	public DateTime? ClosedDate { get; }

	public List<PullRequestFile> Files { get; }

	public long TotalAdditions { get; }
	public long TotalDeletions { get; }
	public long TotalChanges { get; }

	public PullRequest(Issue issue, List<PullRequestFile> files)
	{
		Id = issue.Id;
		Title = issue.Title;
		Number = issue.Number;
		State = MapState(issue.State);
		Url = issue.PullRequest.Url;
		RepositoryUrl = issue.RepositoryUrl;
		CreatedDate = issue.CreatedDate;
		ClosedDate = issue.ClosedDate;
		Files = files;

		TotalAdditions = files.Sum(x => x.Additions);
		TotalDeletions = files.Sum(x => x.Deletions);
		TotalChanges = files.Sum(x => x.Changes);
	}

	// "open" || "closed"
	private string MapState(string state)  // TODO: make this enum
		=> state;
}
