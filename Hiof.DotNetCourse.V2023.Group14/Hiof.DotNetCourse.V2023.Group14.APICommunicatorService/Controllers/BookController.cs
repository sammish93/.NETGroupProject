using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;


namespace Hiof.DotNetCourse.V2023.Group14.APICommunicatorService.Controllers
{
    [ApiController]
    [Route("books")]
    public class BookController : ControllerBase
    {
        private static readonly BookDTO books = new();
        private readonly ILogger<BookController> _logger;

        public BookController(ILogger<BookController> logger)
        {
            _logger = logger;
        }

        [HttpGet("GetBookISBN")]
        public async Task<string> Get(string ISBN)
        {
            string errorMessage = "ISBN must be 10 or 13 digits.";

            if (ISBN.Length == 13 || ISBN.Length == 10)
                return await CallAPI("isbn", ISBN);
            else
            {
                Console.WriteLine(errorMessage);
                return errorMessage;
            }
        }

        [HttpGet("GetBookTitle")]
        public async Task<string?> GetByTitle(string Title)
        {
            var response = await CallAPI("intitle", Title);

            if (response != null)
            {
                var bookData = JsonConvert.DeserializeObject<Exists>(response);

                if (bookData != null && bookData.totalItems != 0)
                    return response;
                else
                    return "The title do not exists";
            }
            return response;
        }

        [HttpGet("GetBookAuthor")]
        public async Task<string> GetByAuthors(string Authors)
        {
            return await CallAPI("inauthor", Authors);
         
        }

        [HttpGet("GetBookCategories")]
        public async Task<string> GetBySubject(string Subject)
        {
            return await CallAPI("categories", Subject);
        
        }

        // Method that executes the API calls.
        private async Task<string> CallAPI(string endpoint, string query)
        {
            var url = GetUrl(endpoint, query);
            HttpClient client = new HttpClient();
            HttpResponseMessage responseMessage;
            try
            {
                responseMessage = await client.GetAsync(url);
                responseMessage.EnsureSuccessStatusCode();
            }
            catch(HttpRequestException ex)
            {
                _logger.LogError(ex, "Request error");
                return "Request error";
            }
            var responseBody = await responseMessage.Content.ReadAsStringAsync();
            return responseBody;
        }

        // Returns URL for googleapis with specified search and endparameter
        private static String GetUrl(string search, string parameter)
        {
            return $"https://www.googleapis.com/books/v1/volumes?q={search}:"
                    + parameter;
        }

    }
}