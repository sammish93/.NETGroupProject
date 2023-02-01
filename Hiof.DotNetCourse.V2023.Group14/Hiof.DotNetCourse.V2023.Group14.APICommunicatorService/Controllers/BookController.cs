using Microsoft.AspNetCore.Mvc;
using static System.Net.WebRequestMethods;

namespace Hiof.DotNetCourse.V2023.Group14.APICommunicatorService.Controllers
{
    [ApiController]
    [Route("books")]
    public class BookController : ControllerBase
    {
        private static BooksDataTransferObject books = new();
      

        private readonly ILogger<BookController> _logger;

        public BookController(ILogger<BookController> logger)
        {
            _logger = logger;
        }

        [HttpGet("v1/volumes/")]
        public async Task<object> Get(string q)
        {
            using var client = new HttpClient();
            var url = "https://www.googleapis.com/books/v1/volumes?q=search";
            HttpResponseMessage responseMessage = await client.GetAsync(url);
            responseMessage.EnsureSuccessStatusCode();
            var responseBody = await responseMessage.Content.ReadAsStringAsync();

            return responseBody;
        }
    }
}