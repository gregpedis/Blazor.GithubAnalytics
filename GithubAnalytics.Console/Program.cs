using GithubAnalytics;
using Newtonsoft.Json;

var username = "gregory-paidis-sonarsource";
var token = ""; // DEBUG

using var service = new HttpService(token, ConsoleLogger.Instance);
await service.TestConnection();
var userStatistics = await service.GetUserStatistics(username);

userStatistics.PullRequests.Clear(); // DEBUG: too much noise
Console.WriteLine(JsonConvert.SerializeObject(userStatistics, Formatting.Indented));

