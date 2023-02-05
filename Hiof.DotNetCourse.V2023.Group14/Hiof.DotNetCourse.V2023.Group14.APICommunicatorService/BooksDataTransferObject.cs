namespace Hiof.DotNetCourse.V2023.Group14.APICommunicatorService
{
    public class BookDTO
    {
        public int ISBN { get; set; }
        public string? Title{ get; set; }  
        public string? Author { get; set; }
        public string? Publisher { get; set; }
        public int PageCount { get; set; }
        public string? PrintType { get; set; }
        public string? Categories { get; set; }
        public string? Language { get; set; }
    }

    // Used to validate when the API return nothing
    public class Exists
    {
        public string? kind { get; set; }
        public int totalItems { get; set; }
    }
}