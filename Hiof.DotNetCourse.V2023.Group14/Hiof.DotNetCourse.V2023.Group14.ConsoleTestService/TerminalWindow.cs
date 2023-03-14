using Hiof.DotNetCourse.V2023.Group14.APICommunicatorService;
using Hiof.DotNetCourse.V2023.Group14.APICommunicatorService.Controllers.V1;
using Hiof.DotNetCourse.V2023.Group14.ClassLibrary.Classes.V1;
using Hiof.DotNetCourse.V2023.Group14.ClassLibrary.Classes.V1.Security;
using Hiof.DotNetCourse.V2023.Group14.ClassLibrary.Enums.V1;
using Microsoft.AspNetCore.Mvc;
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
            uris.Add("https://localhost:7027/api/1.0/books/getBookIsbn?isbn=9781119797203");
            uris.Add("https://localhost:7027/api/1.0/books/getBookIsbn?isbn=999");
            uris.Add("https://localhost:7027/api/1.0/books/getNOTHING?isbn=THROWSERROR404");
            uris.Add("https://DOESNOTEXIST/api/1.0/books/getBookIsbn?isbn=THROWSERROR443");

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


            // Gets the top 10 book results from John Steinbeck and prints them to the console.
            try
            {
                using HttpResponseMessage responseMessage = await client.GetAsync("https://localhost:7027/api/1.0/books/getBookAuthor?authors=john%20steinbeck");
                responseMessage.EnsureSuccessStatusCode();
                var json = await responseMessage.Content.ReadAsStringAsync();
                var bookSearch = new V1BooksDto(json);
                Console.WriteLine("The total amount of books from this search is: " + bookSearch.TotalItems);
                foreach (V1Book book in bookSearch.Books)
                {
                    Console.WriteLine("The book title is: " + book.Title);
                    Console.WriteLine("It has " + book.PageCount + " pages.");
                }
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
    }
}
