namespace Hiof.DotNetCourse.V2023.Group14.APICommunicatorService
{
    public class BookDTO
    {
        public string? kind { get; set; }
        public int totalItems { get; set; }
        public List<Items>? items { get; set; }

    }

    public class Items
    {
        public string? kind { get; set; }
        public string? id { get; set; }
        public VolumeInfo? volumeInfo { get; set; }
    }

    public class VolumeInfo
    {
        public string? title { get; set; }
        public List<string>? author { get; set; }
    }
}