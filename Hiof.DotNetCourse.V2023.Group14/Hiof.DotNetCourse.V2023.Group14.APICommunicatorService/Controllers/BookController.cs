using Microsoft.AspNetCore.Mvc;

namespace Hiof.DotNetCourse.V2023.Group14.APICommunicatorService.Controllers
{
    [ApiController]
    [Route("books")]
    public class BookController : ControllerBase
    {
        private static BooksDataTransferObject books = new();

        // Changed client to a global variable instead
        private static readonly HttpClient client = new();


        private readonly ILogger<BookController> _logger;

        public BookController(ILogger<BookController> logger)
        {
            _logger = logger;
        }

        [HttpGet("GetBookISBN")]
        public async Task<string> Get(string ISBN)
        {
            var url = "https://www.googleapis.com/books/v1/volumes?q=isbn:" + ISBN;
            HttpResponseMessage responseMessage = await client.GetAsync(url);
            responseMessage.EnsureSuccessStatusCode();
            var responseBody = await responseMessage.Content.ReadAsStringAsync();

            return responseBody;
        }

        [HttpGet("GetBookTitle")]
        public async Task<string> GetByTitle(string Title)
        {
            var url = "https://www.googleapis.com/books/v1/volumes?q=intitle:" + Title;
            HttpResponseMessage responseMessage = await client.GetAsync(url);
            responseMessage.EnsureSuccessStatusCode();
            var responseBody = await responseMessage.Content.ReadAsStringAsync();

            return responseBody;
        }

        [HttpGet("GetBookAuthor")]
        public async Task<string> GetByAuthors(string Authors)
        {
            var url = "https://www.googleapis.com/books/v1/volumes?q=inauthor:" + Authors;
            HttpResponseMessage responseMessage = await client.GetAsync(url);
            responseMessage.EnsureSuccessStatusCode();
            var responseBody = await responseMessage.Content.ReadAsStringAsync();

            return responseBody;
        }

        [HttpGet("GetBookCategories")]
        public async Task<string> GetBySubject(string Subject)
        {
            var url = "https://www.googleapis.com/books/v1/volumes?q=categories:" + Subject;
            HttpResponseMessage responseMessage = await client.GetAsync(url);
            responseMessage.EnsureSuccessStatusCode();
            var responseBody = await responseMessage.Content.ReadAsStringAsync();

            return responseBody;
        }

    }
}