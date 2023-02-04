using Microsoft.AspNetCore.Mvc;

namespace Hiof.DotNetCourse.V2023.Group14.APICommunicatorService.Controllers
{
    [ApiController]
    [Route("books")]
    public class BookController : ControllerBase
    {
        private static readonly BooksDataTransferObject books = new();

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
            var url = "";
            var responseBody = "";

            if (ISBN.Length < 13)
            {
                Console.WriteLine("ISBN must be 13 digits");
            }
            else
            {
                url = GetUrl("isbn", ISBN);
                HttpResponseMessage responseMessage = await client.GetAsync(url);

                ExceptionHandler(responseMessage);
                responseBody = await responseMessage.Content.ReadAsStringAsync();

            }

            return responseBody;
        }

        [HttpGet("GetBookTitle")]
        public async Task<string> GetByTitle(string Title)
        {
            var url = GetUrl("intitle", Title);
            HttpResponseMessage responseMessage = await client.GetAsync(url);

            ExceptionHandler(responseMessage);
            var responseBody = await responseMessage.Content.ReadAsStringAsync();

            return responseBody;
        }

        [HttpGet("GetBookAuthor")]
        public async Task<string> GetByAuthors(string Authors)
        {
            var url = GetUrl("inauthor", Authors);
            HttpResponseMessage responseMessage = await client.GetAsync(url);

            ExceptionHandler(responseMessage);
            var responseBody = await responseMessage.Content.ReadAsStringAsync();

            return responseBody;
        }

        [HttpGet("GetBookCategories")]
        public async Task<string> GetBySubject(string Subject)
        {
            var url = GetUrl("categories", Subject);
            HttpResponseMessage responseMessage = await client.GetAsync(url);

            ExceptionHandler(responseMessage);
            var responseBody = await responseMessage.Content.ReadAsStringAsync();

            return responseBody;
        }

        // Returns URL for googleapis with specified search and endparameter
        private static String GetUrl(string search, string parameter)
        {
            return $"https://www.googleapis.com/books/v1/volumes?q={search}:" + parameter;
        }

        // Handle exceptions on bad requests.
        private static void ExceptionHandler(HttpResponseMessage response)
        {
            try
            {
                response.EnsureSuccessStatusCode();
            }
            catch(HttpRequestException exception)
            {
                Console.WriteLine("Exception caught: {0}" + exception);
            }

        }

    }
}