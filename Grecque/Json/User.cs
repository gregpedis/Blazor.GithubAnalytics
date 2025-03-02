using Newtonsoft.Json;
using System.Globalization;

namespace GithubAnalytics.Json;

public class User
{
	public string Id { get; set; }

	[JsonProperty("login")]
	public string Name { get; set; }

	[JsonProperty("avatar_url")]
	public string AvatarUrl { get; set; }

	public string Url { get; set; }

	[JsonProperty("created_at")]
	public DateTime CreatedAt { get; set; }
}
