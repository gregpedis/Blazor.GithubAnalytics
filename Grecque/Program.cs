using Grecque.Json;
using Grecque.Plots;
using Newtonsoft.Json;
using Plotly.NET;
using System.Diagnostics;

var issues = await FetchIssues();
var result = Aggregate(issues);
Visualize(result.Dates, result.Created, result.Closed);

Console.WriteLine($"Current open issues: {result.Created[^1] - result.Closed[^1]}");
Console.WriteLine($"Current closed issues: {result.Closed[^1]}");
Console.WriteLine($"Current total issues: {result.Created[^1]}");

static async Task<List<Issue>> FetchIssues()
{
    var owner = "sonarsource";
    var repo = "sonar-dotnet";
    var per_page = 100;

    // - state: "open" || "closed" || "all"                 -> all 
    // - per_page: how many per page (default=30)           -> 100 (max)
    // - page: current page                                 -> increment per loop
    var baseUrl = $"https://api.github.com/repos/{owner}/{repo}/issues?state=all&per_page={per_page}&page=";
    var token = "";

    using var client = new HttpClient();
    client.DefaultRequestHeaders.Add("Authorization", $"Bearer {token}");
    client.DefaultRequestHeaders.Add("User-Agent", Guid.NewGuid().ToString());

    var sw = Stopwatch.StartNew();

    var pageNumber = 1; // total is around 82
    var result = new List<Issue>();
    List<Issue> current = null;
    do
    {
        var response = await client.GetStringAsync(baseUrl + pageNumber);
        current = JsonConvert.DeserializeObject<List<Issue>>(response);
        result.AddRange(current.Where(x => !x.IsPullRequest));

        Console.WriteLine($"[Network - {sw.Elapsed}] Page: {pageNumber}");
        pageNumber++;
    }
    while (current.Count > 0);

    return result;
}

static Aggregation Aggregate(List<Issue> issues)
{
    var dates = new List<DateTime>();               // x axis
    var createdCountsIncremental = new List<int>(); // y axis1
    var closedCountsIncremental = new List<int>();  // y axis2

    var totalCreated = 0;
    var totalClosed = 0;

    var createdGrouped = issues.GroupBy(x => x.CreatedAt.Date, x => x).ToDictionary(x => x.Key, x => x.Count());
    var closedGrouped = issues.Where(x => x.ClosedAt.HasValue).GroupBy(x => x.ClosedAt.Value.Date).ToDictionary(x => x.Key, x => x.Count());

    var xFrom = issues.MinBy(x => x.CreatedAt).CreatedAt.Date;
    var xTo = issues.MaxBy(x => x.CreatedAt).CreatedAt.Date;

    var sw = Stopwatch.StartNew();
    for (int i = 0; i <= (xTo - xFrom).TotalDays; i++)
    {
        var currentDate = xFrom.AddDays(i).Date;
        totalCreated += createdGrouped.ContainsKey(currentDate) ? createdGrouped[currentDate] : 0;
        totalClosed += closedGrouped.ContainsKey(currentDate) ? closedGrouped[currentDate] : 0;

        dates.Add(currentDate);
        createdCountsIncremental.Add(totalCreated);
        closedCountsIncremental.Add(totalClosed);

        Console.WriteLine($"[Aggregation - {sw.Elapsed}] Day: {currentDate}");
    }

    return new Aggregation(dates, createdCountsIncremental, closedCountsIncremental);
}

static void Visualize<T>(IEnumerable<T> dates, IEnumerable<int> created, IEnumerable<int> closed)
    where T : IConvertible
{
    var res = Visualization.combinedChart(dates, created, closed, "created", "closed");
    res.SaveHtml("here.html", true);
}

record Aggregation(List<DateTime> Dates, List<int> Created, List<int> Closed);
