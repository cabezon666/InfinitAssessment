using InfinitAssessment.Models;

namespace InfinitAssessment.Services
{
    public interface IGitHubService
    {
        Task<CountResult> AnalyzeRepositoryAsync(string owner, string repo);
    }
}
