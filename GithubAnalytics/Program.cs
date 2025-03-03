using GithubAnalytics.Api;
using GithubAnalytics.Logging;
using Newtonsoft.Json;

var token =; // DEBUG
using var service = new HttpService(token, ConsoleLogger.Instance);
var userStatistics = await service.GetUserStatistics();

Console.WriteLine(JsonConvert.SerializeObject(userStatistics, Formatting.Indented));
