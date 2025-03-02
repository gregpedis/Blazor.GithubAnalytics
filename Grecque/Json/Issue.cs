using Newtonsoft.Json;
using System.Globalization;

namespace GithubAnalytics.Json;

public class Issue
{
	const string dateMask = "yyyy-MM-ddTHH:mm:ssZ";

	[JsonProperty]
	private string Created_At { get; set; }
	[JsonProperty]
	private string Closed_At { get; set; }
	[JsonProperty]
	private List<Label> Labels { get; set; }
	[JsonProperty]
	private PullRequest Pull_Request { get; set; }

	// "open" || "closed"
	public string State { get; set; }
	// "completed" || "reopened" || "not_planned"
	public string State_Reason { get; set; }

	public DateTime CreatedAt => DateTime.ParseExact(Created_At, dateMask, CultureInfo.InvariantCulture);

	public DateTime? ClosedAt => string.IsNullOrWhiteSpace(Closed_At) ? null : DateTime.ParseExact(Closed_At, dateMask, CultureInfo.InvariantCulture);

	public string Type => Labels.Find(x => x.Name.StartsWith("Type:"))?.Name?.Replace("Type:", "")?.Trim();

	public bool IsPullRequest => Pull_Request is not null;
}
