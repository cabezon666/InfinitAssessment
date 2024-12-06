using InfinitAssessment.DTOs;

namespace InfinitAssessment.Services
{
    public interface IGitHubService
    {
        Task<AnalyzeRepositoryResponseDto> AnalyzeRepositoryAsync(string owner, string repo);
    }
}
