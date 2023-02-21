using Hiof.DotNetCourse.V2023.Group14.APICommunicatorService;
using Hiof.DotNetCourse.V2023.Group14.APICommunicatorService.Controllers.V1;
using Hiof.DotNetCourse.V2023.Group14.ClassLibrary.Classes.V1;
using Hiof.DotNetCourse.V2023.Group14.ClassLibrary.Classes.V1.Security;
using Hiof.DotNetCourse.V2023.Group14.ClassLibrary.Enums.V1;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using NuGet.Protocol;
using System;
using System.Collections;
using System.Net;
using System.Runtime.InteropServices;
using System.Security.Policy;

namespace Hiof.DotNetCourse.V2023.Group14.ConsoleTestService
{
    internal class TerminalWindow
    {
        // Used to retrieve DTOs from another of our microservices.
        static readonly HttpClient client = new HttpClient();
        static async Task Main(string[] args)
        {
            
            var user = new V1User("sammish", "sam@samland.no", "Afdkjfsd453kgfFGk43", "sam", "davies", "Norway", "Aalesund", "EN", UserRole.Admin);

            Console.WriteLine("User Information");
            Console.WriteLine("Username: " + user.UserName);
            Console.WriteLine("Hashed Password: " + user.Password);
            Console.WriteLine("Role: " + user.Role);
            Console.WriteLine("Registration Date: " + user.RegistrationDate); 
           

            
            // Testing encryption of password.
            var (hash, salt) = V1PasswordEncryption.Encrypt("Leon");

            Console.WriteLine($"\nPassword hash: {hash}");
            Console.WriteLine($"Generated Salt: {Convert.ToHexString(salt)}");
      

            // Test to se if the password is the same by passing the same
            // hash and salt.
            var verify = V1PasswordEncryption.Verify("Leon", hash, salt);
            Console.WriteLine($"Same password?: {verify}");


            // Fetching a JSON object from the APICommunicatorService
            // Creating 4 URIs to iterate through. Only the first one is valid
            var uris = new ArrayList();
            uris.Add("https://localhost:7027/api/1.0/GetBookIsbn?isbn=9781119797203");
            uris.Add("https://localhost:7027/api/1.0/GetBookIsbn?isbn=999");
            uris.Add("https://localhost:7027/api/1.0/GetNOTHING?isbn=THROWSERROR404");
            uris.Add("https://DOESNOTEXIST/api/1.0/GetBookIsbn?isbn=THROWSERROR443");

            // Iterates through the 4 URIs above
            foreach (string uri in uris)
            {
                // Printing an existing Json object (or catches an exception and prints the code) to the console.
                Console.WriteLine("\nResponse for the URI: " + uri + "\n");
                try
                {
                    using HttpResponseMessage responseMessage = await client.GetAsync(uri);
                    responseMessage.EnsureSuccessStatusCode();
                    var json = await responseMessage.Content.ReadAsStringAsync();
                    Console.WriteLine(json);
                }
                catch (HttpRequestException ex)
                {
                    Console.WriteLine(ex.Message);
                    if (ex.StatusCode == HttpStatusCode.BadRequest)
                    {
                        Console.WriteLine("ISBN must be 10 or 13 digits");
                    }
                    else if (ex.StatusCode == HttpStatusCode.NotFound)
                    {
                        Console.WriteLine("No book found.");
                    }
                }
            }


            string jsonString = @"{
              ""kind"": ""books#volume"",
              ""id"": ""fo4rzdaHDAwC"",
              ""etag"": ""Gi5SJsSPtSc"",
              ""selfLink"": ""https://www.googleapis.com/books/v1/volumes/fo4rzdaHDAwC"",
              ""volumeInfo"": {
                ""title"": ""Harry Potter and the Sorcerer's Stone"",
                ""authors"": [
                  ""J. K. Rowling""
                ],
                ""publisher"": ""Arthur A. Levine Books"",
                ""publishedDate"": ""1998"",
                ""description"": ""Rescued from the outrageous neglect of his aunt and uncle, a young boy with a great destiny proves his worth while attending Hogwarts School for Witchcraft and Wizardry."",
                ""industryIdentifiers"": [
                  {
                    ""type"": ""ISBN_10"",
                    ""identifier"": ""059035342X""
                  },
                  {
                    ""type"": ""ISBN_13"",
                    ""identifier"": ""9780590353427""
                  }
                ],
                ""readingModes"": {
                  ""text"": false,
                  ""image"": false
                },
                ""pageCount"": 324,
                ""printType"": ""BOOK"",
                ""categories"": [
                  ""Juvenile Fiction""
                ],
                ""averageRating"": 4.5,
                ""ratingsCount"": 1736,
                ""maturityRating"": ""NOT_MATURE"",
                ""allowAnonLogging"": false,
                ""contentVersion"": ""0.8.4.0.preview.0"",
                ""panelizationSummary"": {
                  ""containsEpubBubbles"": false,
                  ""containsImageBubbles"": false
                },
                ""imageLinks"": {
                  ""smallThumbnail"": ""http://books.google.com/books/content?id=fo4rzdaHDAwC&printsec=frontcover&img=1&zoom=5&source=gbs_api"",
                  ""thumbnail"": ""http://books.google.com/books/content?id=fo4rzdaHDAwC&printsec=frontcover&img=1&zoom=1&source=gbs_api""
                },
                ""language"": ""en"",
                ""previewLink"": ""http://books.google.no/books?id=fo4rzdaHDAwC&dq=isbn:9780590353427&hl=&cd=1&source=gbs_api"",
                ""infoLink"": ""http://books.google.no/books?id=fo4rzdaHDAwC&dq=isbn:9780590353427&hl=&source=gbs_api"",
                ""canonicalVolumeLink"": ""https://books.google.com/books/about/Harry_Potter_and_the_Sorcerer_s_Stone.html?hl=&id=fo4rzdaHDAwC""
              },
              ""saleInfo"": {
                ""country"": ""NO"",
                ""saleability"": ""NOT_FOR_SALE"",
                ""isEbook"": false
              },
              ""accessInfo"": {
                ""country"": ""NO"",
                ""viewability"": ""NO_PAGES"",
                ""embeddable"": false,
                ""publicDomain"": false,
                ""textToSpeechPermission"": ""ALLOWED"",
                ""epub"": {
                  ""isAvailable"": false
                },
                ""pdf"": {
                  ""isAvailable"": false
                },
                ""webReaderLink"": ""http://play.google.com/books/reader?id=fo4rzdaHDAwC&hl=&source=gbs_api"",
                ""accessViewStatus"": ""NONE"",
                ""quoteSharingAllowed"": false
              },
              ""searchInfo"": {
                ""textSnippet"": ""Rescued from the outrageous neglect of his aunt and uncle, a young boy with a great destiny proves his worth while attending Hogwarts School for Witchcraft and Wizardry.""
              }
            }";


            var book = new V1Book(jsonString);

            Console.WriteLine(book.Title);

            DateOnly thing = DateOnly.Parse("2021-07");

            Console.WriteLine(thing);




        }
    }
}