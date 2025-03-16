namespace GithubAnalytics.Json;

public class PullRequestFile
{
	public string Filename { get; set; }

	public long Additions { get; set; }
	public long Deletions { get; set; }
	public long Changes { get; set; }

	public string Status { get; set; }

	public PullRequestFileStatus StatusEnum =>
		Status switch
		{
			"added" => PullRequestFileStatus.Added,
			"removed" => PullRequestFileStatus.Deleted,
			"modified" => PullRequestFileStatus.Modified,
			"renamed" => PullRequestFileStatus.Modified,
			_ => PullRequestFileStatus.Unspecified,
		};
}

public enum PullRequestFileStatus
{
	Added,
	Deleted,
	Modified,
	Unspecified,
}

