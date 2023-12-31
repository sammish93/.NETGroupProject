using System.Text;
using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Hiof.DotNetCourse.V2023.Group14.ClassLibrary.Classes.V1;
using System.ComponentModel.DataAnnotations;
using Hiof.DotNetCourse.V2023.Group14.APICommunicatorService.Configuration;
using Microsoft.Extensions.Options;

namespace Hiof.DotNetCourse.V2023.Group14.APICommunicatorService.Controllers.V1
{
    [ApiController]
    [Route("api/1.0/books")]
    public class V1BookController : ControllerBase
    {
        private readonly V1BooksDto _books = new();
        private readonly ILogger<V1BookController> _logger;
        // Accesses settings from appsettings.json. Example:
        // string exampleString = _settings.Value.DefaultSettings;
        private readonly IOptions<ApiCommunicatorSettings> _settings;

        public V1BookController(ILogger<V1BookController> logger, IOptions<ApiCommunicatorSettings> settings)
        {
            _logger = logger;
            _settings = settings;
        }

        [HttpGet("getBookIsbn")]
        public async Task<ActionResult> Get(string isbn)
        {
            var response = await CallApi("isbn", isbn, null, null);

            if ((isbn.Length == 13) || (isbn.Length == 10))
            {
                if (!CheckResponse(response))
                    return NotFound("No book found.");
                else
                    return Ok(response);

            }
            return BadRequest("ISBN must be 10 or 13 digits.");
        }

        [HttpGet("getBookTitle")]
        public async Task<IActionResult> GetByTitle(string title, int? maxResults, string? langRestrict)
        {
            var response = await CallApi("intitle", title, maxResults, langRestrict);

            if (maxResults > 40 || maxResults < 1)
            {
                return BadRequest("The maximum amount of results must be larger than 0, and equal to or fewer than 40.");
            }
            else if (!CheckResponse(response))
            {
                return NotFound("No book found.");
            }

            return Ok(response);
        }

        [HttpGet("getBookAuthor")]
        public async Task<IActionResult> GetByAuthors(string authors, int? maxResults, string? langRestrict)
        {

            var response = await CallApi("inauthor", authors, maxResults, langRestrict);

            if (maxResults > 40 || maxResults < 1)
            {
                return BadRequest("The maximum amount of results must be larger than 0, and equal to or fewer than 40.");
            }
            else if (!CheckResponse(response))
            {
                return NotFound("No book found.");
            }


            return Ok(response);

        }

        [HttpGet("getBookCategory")]
        public async Task<IActionResult> GetBySubject(string subject, int? maxResults, string? langRestrict)
        {
            var response = await CallApi("categories", subject, maxResults, langRestrict);

            if (maxResults > 40 || maxResults < 1)
            {
                return BadRequest("The maximum amount of results must be larger than 0, and equal to or fewer than 40.");
            }
            else if (!CheckResponse(response))
            {
                return NotFound("No book found.");
            }


            return Ok(response);

        }


        // Execute API calls and return response as a string.
        private async Task<string> CallApi(string endpoint, string query, int? maxResults, string? langRestrict)
        {
            var url = GetUrl(endpoint, query, maxResults, langRestrict);
            HttpClient client = new HttpClient();
            HttpResponseMessage responseMessage;
            try
            {
                responseMessage = await client.GetAsync(url);
                responseMessage.EnsureSuccessStatusCode();
            }
            catch (HttpRequestException ex)
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
                var bookData = JsonConvert.DeserializeObject<V1BooksDto>(response);

                if (bookData != null && bookData.TotalItems != 0)
                    return true;
            }
            return false;
        }

        // Returns URL for googleapis with specified search and endpoint
        private static String GetUrl(string search, string endpoint, int? maxResults = null, string? langRestrict = null)
        {
            StringBuilder url = new StringBuilder();
            url.Append("https://www.googleapis.com/books/v1/volumes?q=");

            url.Append($"{search}:");
            url.Append(endpoint);


            if (maxResults != null)
            {
                url.Append("&maxResults=");
                url.Append((int)maxResults.Value);
            }
            if (langRestrict != null)
            {
                url.Append("&langRestrict=");
                url.Append(langRestrict);
            }



            return url.ToString();

        }
    }
}