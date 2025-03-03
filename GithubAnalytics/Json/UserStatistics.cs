namespace GithubAnalytics.Json;

public class UserStatistics
{
	public User UserDetails { get; }

	public int TotalPullRequests { get; }

	public int TotalFiles { get; }
	public int TotalFilesAdded { get; }
	public int TotalFilesDeleted { get; }
	public int TotalFilesModified { get; }

	public int TotalAdditions { get; }
	public int TotalDeletions { get; }
	public int TotalChanges { get; }

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
	public int Id { get; }
	public string Title { get; }
	public int Number { get; }
	public string State { get; }
	public string Url { get; }
	public string RepositoryUrl { get; }
	public DateTime CreatedDate { get; }
	public DateTime ClosedDate { get; }

	public List<PullRequestFile> Files { get; }

	public int TotalAdditions { get; }
	public int TotalDeletions { get; }
	public int TotalChanges { get; }

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
