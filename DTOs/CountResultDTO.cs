namespace InfinitAssessment.DTOs
{
    public class AnalyzeRepositoryResponseDto
    {
        public int NumberOfJsFiles { get; set; }
        public int NumberOfTsFiles { get; set; }
        public long TotalLetters { get; set; }
        public List<LetterCountDto> LetterCounts { get; set; } = new();
    }

    public class LetterCountDto
    {
        public string Letter { get; set; }
        public long Quantity { get; set; }
    }
}
