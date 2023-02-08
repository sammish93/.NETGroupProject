using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;



namespace Hiof.DotNetCourse.V2023.Group14.APICommunicatorService.Controllers
{
    [ApiController]
    [Route("books")]
    public class BookController : ControllerBase
    {
        private readonly BookDTO books = new();
        private readonly ILogger<BookController> _logger;

        public BookController(ILogger<BookController> logger)
        {
            _logger = logger;
        }

        [HttpGet("GetBookISBN")]
        public async Task<string> Get(string ISBN)
        {
            string message = "ISBN must be 10 or 13 digits.";
            var response = await CallAPI("isbn", ISBN);

            if (ISBN.Length == 13 || ISBN.Length == 10)
            {
                if (!CheckResponse(response))
                    return ErrorMessage("ISBN");
                else
                    return response;
                    
            }
            Console.WriteLine(message);
            return message;
        }

        [HttpGet("GetBookTitle")]
        public async Task<string?> GetByTitle(string title)
        {
            var response = await CallAPI("intitle", title);
            if (!CheckResponse(response))
                return ErrorMessage("Title");

            return response;
        }

        [HttpGet("GetBookAuthor")]
        public async Task<string> GetByAuthors(string authors)
        {
            var response = await CallAPI("inauthor", authors);
            if (!CheckResponse(response))
                return ErrorMessage("Author");

            return response;
         
        }

        [HttpGet("GetBookCategories")]
        public async Task<string> GetBySubject(string subject)
        {
            var response = await CallAPI("categories", subject);
            if (!CheckResponse(response))
                return ErrorMessage("Category");

            return response;
        
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
                return "HTTP error status code: " + ex;
            }
            var responseBody = await responseMessage.Content.ReadAsStringAsync();
            return responseBody;
        }

        // Checks if the API response returns any books or not.
        private static bool CheckResponse(string response)
        {
            if (response != null)
            {
                var bookData = JsonConvert.DeserializeObject<BookDTO>(response);

                if (bookData != null && bookData.totalItems != 0)
                    return true;
            }
            return false;
        }

        // Returns URL for googleapis with specified search and endpoint
        private static String GetUrl(string search, string endpoint)
        {
            return $"https://www.googleapis.com/books/v1/volumes?q={search}:"
                    + endpoint;
        }

        // Message to deliver in swagger.
        private static string ErrorMessage(string subject)
        {
            return $"{subject} do not exists";
        }
    }
}