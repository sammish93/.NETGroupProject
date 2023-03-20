using Newtonsoft.Json.Linq;
using Newtonsoft.Json;

namespace Hiof.DotNetCourse.V2023.Group14.ClassLibrary.Classes.V1
{
    // This class is used to create an object from a single search via the APICommunicatorService.
    public class V1BooksDto
    {
        // Google Books API uses the field 'kind' for a publication object/type. Books have the kind "books#volumes". 
        public string? Kind { get; set; }
        // The amount of results from a single API request.
        public int? TotalItems { get; set; }
        // A collection of books from a single search.
        public IList<V1Book>? Books { get; set; }


        // This constructor takes a Json object in string format and is hard-coded to accept only results following the Google Books API format.
        public V1BooksDto(string jsonString) 
        {
            // Creates a Json object.
            dynamic? jsonObject = JsonConvert.DeserializeObject(jsonString);

            Kind = jsonObject.kind;

            TotalItems = jsonObject.totalItems;

            // Creates a V1Book object for each book item from the search result.
            var books = new List<V1Book>();
            var jArrayBooks = jsonObject.items;
            foreach (JObject jObjectItem in jArrayBooks)
            {
                var book = new V1Book(jObjectItem.ToString());
                books.Add(book);
            }
            Books = books;
        }


        // Blank constructor needed for APICommunicatorService.V1BookController.
        public V1BooksDto()
        {

        }
    }
}