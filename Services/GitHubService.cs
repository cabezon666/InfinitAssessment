using InfinitAssessment.Models;
using System.Text;

namespace InfinitAssessment.Services
{
    public class GitHubService : IGitHubService
    {
        private readonly HttpClient _httpClient;
        private readonly IConfiguration _configuration;

        public GitHubService(HttpClient httpClient, IConfiguration configuration)
        {
            _httpClient = httpClient;
            _configuration = configuration;
        }

        public async Task<CountResult> AnalyzeRepositoryAsync(string owner, string repo)
        {
            var treeResponse = await GetRepositoryTreeAsync(owner, repo);
            
            var mapResults = await Task.WhenAll(treeResponse.Tree
                .Where(item => item.Type == "blob" && (item.Path.EndsWith(".js") || item.Path.EndsWith(".ts")))
                .Select(async item =>
                {
                    var content = await GetFileContentAsync(owner, repo, item.Path);
                    if (string.IsNullOrEmpty(content)) return null;

                    var decodedContent = Encoding.UTF8.GetString(Convert.FromBase64String(content));
                    var letterCounts = new Dictionary<char, long>();

                    foreach (var ch in decodedContent.ToLower())
                    {
                        if (char.IsLetter(ch))
                        {
                            if (!letterCounts.ContainsKey(ch))
                                letterCounts[ch] = 0;
                            letterCounts[ch]++;
                        }
                    }
                    return new
                    {
                        FileType = item.Path.EndsWith(".js") ? "js" : "ts",
                        LetterCounts = letterCounts,
                        TotalLetters = letterCounts.Values.Sum()
                    };
                }));
           
            var validResults = mapResults.Where(result => result != null);
           
            var countResult = new CountResult
            {
                NumberOfJsFiles = validResults.Count(r => r.FileType == "js"),
                NumberOfTsFiles = validResults.Count(r => r.FileType == "ts"),
                NumberOfLetters = validResults.Sum(r => r.TotalLetters)
            };

            var aggregatedLetterCounts = validResults
                .SelectMany(r => r.LetterCounts)
                .GroupBy(kv => kv.Key)
                .ToDictionary(
                    g => g.Key,
                    g => g.Sum(kv => kv.Value)
                );

            countResult.LetterAndQuantity = aggregatedLetterCounts
                .OrderByDescending(l => l.Value)
                .Select(l => new LQ
                {
                    Letter = l.Key.ToString(),
                    Quantity = l.Value
                })
                .ToList();

            return countResult;
        }

        private async Task<GitHubTreeResponse> GetRepositoryTreeAsync(string owner, string repo)
        {
            var url = $"{_configuration["GitHub:ApiBase"]}/repos/{owner}/{repo}/git/trees/main?recursive=1";

            _httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", _configuration["GitHub:Token"]);

            var response = await _httpClient.GetFromJsonAsync<GitHubTreeResponse>(url);
            if (response == null)
            {
                throw new Exception("Error on GitHubTreeResponse, repo OK?");
            }

            return response;
        }

        private async Task<string> GetFileContentAsync(string owner, string repo, string path)
        {
            var url = $"{_configuration["GitHub:ApiBase"]}/repos/{owner}/{repo}/contents/{path}";

            if (!url.EndsWith(".js") && !url.EndsWith(".ts"))
                return string.Empty;

            _httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", _configuration["GitHub:Token"]);

            var response = await _httpClient.GetFromJsonAsync<GitHubFileResponse>(url);
            if (response == null || string.IsNullOrEmpty(response.Content))
            {
                throw new Exception($"Error on obtain file with path: {path}.");
            }

            return response.Content;
        }
    }
}
