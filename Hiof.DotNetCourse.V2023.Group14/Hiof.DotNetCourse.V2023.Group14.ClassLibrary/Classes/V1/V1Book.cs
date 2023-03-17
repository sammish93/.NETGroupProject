using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hiof.DotNetCourse.V2023.Group14.ClassLibrary.Classes.V1
{
    public class V1Book
    {
        // Books published before 2008 don't have an ISBN 13 number.
        // Important that ISBNs are string in the case of one beginning with 0.
        public IDictionary<string, string>? IndustryIdentifiers { get; }
        public string Language { get; }
        public string Title { get; }
        // One book can have many authors.
        public IList<string> Authors { get; }
        public string Publisher { get; }
        public DateOnly PublishedDate { get; }
        public string Description { get; }
        public int PageCount { get; }
        public IList<string>? Categories { get; }
        public IDictionary<string, string> ImageLinks { get; }


        public V1Book(IDictionary<string, string> industryIdentifiers, string language, string title, IList<string> authors, string publisher, DateOnly publishedDate, string description, int pageCount, IList<string> categories, IDictionary<string, string> imageLinks)
        {
            IndustryIdentifiers = industryIdentifiers;
            Language = language;
            Title = title;
            Authors = authors;
            Publisher = publisher;
            PublishedDate = publishedDate;
            Description = description;
            PageCount = pageCount;
            Categories = categories;
            ImageLinks = imageLinks;
        }

        // Serialises a V1Book object based on a Json string from Google Books API.
        public V1Book(string jsonString)
        {
            // Creates a Json object.
            dynamic? jsonObject = JsonConvert.DeserializeObject(jsonString);

            // Assigns the relevant ISBN numbers to an IndustryIdentifiers Dictionary.
            var jArrayIndustryIdentifiers = jsonObject?.volumeInfo["industryIdentifiers"];
            var industryIdentifiers = new Dictionary<string, string>();
            if (jArrayIndustryIdentifiers != null)
            {
                foreach (JObject jObjectIndustryIdentifier in jArrayIndustryIdentifiers)
                {
                    string? type = (string)jObjectIndustryIdentifier["type"];
                    string? identifier = (string)jObjectIndustryIdentifier["identifier"];
                    if (type != null && identifier != null)
                    {
                        industryIdentifiers.Add(type, identifier);
                    }
                }
            }
            
            IndustryIdentifiers = industryIdentifiers;

            // Publication language.
            Language = jsonObject.volumeInfo["language"];

            // Book title.
            Title = jsonObject.volumeInfo["title"];

            // Authors as a List. One book can have several authors.
            var jArrayAuthors = jsonObject.volumeInfo["authors"];
            var authors = new List<string>();
            foreach (string author in jArrayAuthors)
            {
                authors.Add(author);
            }
            Authors = authors;

            // Publisher.
            Publisher = jsonObject.volumeInfo["publisher"];

            // Date of publish. DateOnly.Parse() requires both a year and a month. If only a year is supplied then "-01" is automatically added to the string.
            string dateBefore = jsonObject.volumeInfo["publishedDate"];
            if (dateBefore.IsNullOrEmpty())
            {
                dateBefore = "1234";
            }
            if (!dateBefore.IsNullOrEmpty() && dateBefore.Length == 4)
            {
                dateBefore = dateBefore + "-01";
            } 
            PublishedDate = DateOnly.Parse(dateBefore);

            // Description.
            Description = jsonObject.volumeInfo["description"];

            // Page Count.
            PageCount = jsonObject.volumeInfo["pageCount"];

            // Categories/Genres. One book can have several categories.
            var jArrayCategories = jsonObject.volumeInfo["categories"];
            var categories = new List<string>();
            if (jArrayCategories != null)
            {
                foreach (string category in jArrayCategories)
                {
                    categories.Add(category);
                }
            }
            
            Categories = categories;

            // Thumbnails for usage with the Gui.
            var jArrayImageLinks = jsonObject.volumeInfo["imageLinks"];
            var imageLinks = new Dictionary<string, string>();
            string smallThumbnail = jArrayImageLinks.smallThumbnail;
            string thumbnail = jArrayImageLinks.thumbnail;
            imageLinks.Add("smallThumbnail", smallThumbnail);
            imageLinks.Add("thumbnail", thumbnail);
            ImageLinks = imageLinks;
        }
    }
}
