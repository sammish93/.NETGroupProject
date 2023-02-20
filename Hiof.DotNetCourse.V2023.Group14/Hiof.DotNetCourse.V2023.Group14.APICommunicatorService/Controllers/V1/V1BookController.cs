using System.Text;
using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Hiof.DotNetCourse.V2023.Group14.ClassLibrary.Classes.V1;


namespace Hiof.DotNetCourse.V2023.Group14.APICommunicatorService.Controllers.V1
{
    [ApiController]
    [Route("api/1.0")]
    public class V1BookController : ControllerBase
    {
        private readonly V1BookDto _books = new();
        private readonly ILogger<V1BookController> _logger;

        public V1BookController(ILogger<V1BookController> logger)
        {
            _logger = logger;
        }

        [HttpGet("GetBookIsbn")]
        public async Task<IActionResult> Get(string isbn)
        {
            var response = await CallApi("isbn", isbn);

            if ((isbn.Length == 13) || (isbn.Length == 10))
            {
                if (!CheckResponse(response))
                    return NotFound("http 404: No book found.");
                else
                    return Ok("http 200:" + response);
                    
            }
            return BadRequest("http 400: ISBN must be 10 or 13 digits");
        }

        [HttpGet("GetBookTitle")]
        public async Task<IActionResult> GetByTitle(string title)
        {
            var response = await CallApi("intitle", title);
            if (!CheckResponse(response))
                return NotFound("http 404: No book found");

            return Ok("http 200:" + response);
        }

        [HttpGet("GetBookAuthor")]
        public async Task<IActionResult> GetByAuthors(string authors)
        {
            var response = await CallApi("inauthor", authors);
            if (!CheckResponse(response))
                return NotFound("http 404: No book found");

            return Ok("http: 200" + response);
         
        }

        [HttpGet("GetBookCategories")]
        public async Task<IActionResult> GetBySubject(string subject)
        {
            var response = await CallApi("categories", subject);
            if (!CheckResponse(response))
                return NotFound("http 404: No book found");

            return Ok("http 200:" + response);
        
        }

        // Execute API calls and return response as a string.
        private async Task<string> CallApi(string endpoint, string query)
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
                var bookData = JsonConvert.DeserializeObject<V1BookDto>(response);

                if (bookData != null && bookData.totalItems != 0)
                    return true;
            }
            return false;
        }

        // Returns URL for googleapis with specified search and endpoint
        private static String GetUrl(string search, string endpoint)
        {
            StringBuilder url = new StringBuilder();
            url.Append("https://www.googleapis.com/books/v1/volumes?q=");
            url.Append($"{search}:");
            url.Append(endpoint);

            return url.ToString();
        }
    }
}