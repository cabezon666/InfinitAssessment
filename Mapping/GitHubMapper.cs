using InfinitAssessment.DTOs;

namespace InfinitAssessment.Mapping
{
    public static class GitHubMapping
    {
        public static AnalyzeRepositoryResponseDto MapToAnalyzeRepositoryResponseDto(
            int numberOfJsFiles,
            int numberOfTsFiles,
            long totalLetters,
            Dictionary<char, long> aggregatedLetterCounts)
        {
            return new AnalyzeRepositoryResponseDto
            {
                NumberOfJsFiles = numberOfJsFiles,
                NumberOfTsFiles = numberOfTsFiles,
                TotalLetters = totalLetters,
                LetterCounts = aggregatedLetterCounts
                    .OrderByDescending(l => l.Value)
                    .Select(l => new LetterCountDto
                    {
                        Letter = l.Key.ToString(),
                        Quantity = l.Value
                    })
                    .ToList()
            };
        }
    }
}
