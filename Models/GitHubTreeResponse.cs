namespace InfinitAssessment.Models
{
    public class GitHubTreeResponse
    {
        public string Sha { get; set; }
        public string Url { get; set; }
        public List<TreeItem> Tree { get; set; }
        public bool Truncated { get; set; }
    }

    public class TreeItem
    {
        public string Path { get; set; }
        public string Mode { get; set; }
        public string Type { get; set; }
        public string Sha { get; set; }
        public long? Size { get; set; }
        public string Url { get; set; }
    }

    public class GitHubFileResponse
    {
        public string Content { get; set; } = "";
        public string Encoding { get; set; } = "";
    }

    public class CountResult
    {
        public long NumberOfLetters { get; set; } = 0;
        public long NumberOfJsFiles { get; set; } = 0;
        public long NumberOfTsFiles { get; set; } = 0;
        public List<LQ> LetterAndQuantity { get; set; } = new List<LQ>();
    }

    public class LQ
    {
        public string Letter { get; set; }
        public long Quantity { get; set; }
    }

}
