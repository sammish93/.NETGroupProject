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
        public int TotalItems { get; set; }
        // A collection of books from a single search.
        public IList<V1Book>? Books { get; set; }

        /*
        public V1BooksDto(string jsonString) 
        {
            JObject jObject = JObject.Parse(jsonString);

            IList<JToken> jTokens = jObject["items"].Children().ToList();

            IList<V1Book> books = new List<V1Book>();
            foreach (JToken jToken in jTokens)
            {
                string name = jToken.

                books.Add(new V1Book(
                    )
            }
            JsonConvert.DeserializeObject<V1Book>(jsonString);
        }
        */
    }
}