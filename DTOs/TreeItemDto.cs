namespace InfinitAssessment.DTOs
{
    public class TreeItemDto
    {
        public string FileName { get; set; }
        public string FileExtension { get; set; }
        public bool IsFolder { get; set; }
        public long SizeInKb { get; set; }
        public string Url { get; set; }
    }
}
