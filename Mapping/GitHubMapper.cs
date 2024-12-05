// --> OLD METHOD - I WANTED TO TRY THE DTO USING A MANUAL MAPPING

//using InfinitAssessment.DTOs;
//using InfinitAssessment.Models;

//namespace InfinitAssessment.Mapping
//{
//    public static class GitHubMapper
//    {
//        public static GitHubTreeDto MapToDto(GitHubTreeResponse response)
//        {
//            if (response == null) return null;

//            return new GitHubTreeDto
//            {
//                Sha = response.Sha,
//                Url = response.Url,
//                IsTruncated = response.Truncated,
//                TreeItems = response.Tree?.Select(item => new TreeItemDto
//                {
//                    FileName = System.IO.Path.GetFileName(item.Path),
//                    FileExtension = System.IO.Path.GetExtension(item.Path),
//                    IsFolder = item.Type == "tree",
//                    SizeInKb = item.Size.HasValue ? item.Size.Value / 1024 : 0,
//                    Url = item.Url
//                }).ToList()
//            };
//        }
//    }
//}
