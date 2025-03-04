namespace GithubAnalytics.Json;

public class PullRequestFile
{
	public string Filename { get; set; }

	public long Additions { get; set; }
	public long Deletions { get; set; }
	public long Changes { get; set; }

	//  added | modified | ???
	public string Status { get; set; }

	public string State => Status; // TODO: Make this enum
}

