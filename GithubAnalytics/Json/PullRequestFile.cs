namespace GithubAnalytics.Json;

public class PullRequestFile
{
	public string Filename { get; set; }

	public int Additions { get; set; }
	public int Deletions { get; set; }
	public int Changes { get; set; }

	//  added | modified | ???
	public string Status { get; set; }

	public string State => Status; // TODO: Make this enum
}

