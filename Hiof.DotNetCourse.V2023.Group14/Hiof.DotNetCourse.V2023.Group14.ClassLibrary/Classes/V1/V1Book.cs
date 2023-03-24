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
            var language = jsonObject.volumeInfo["language"];
            if (language != null)
            {
                Language = language;
            }

            // Book title.
            var title = jsonObject.volumeInfo["title"];
            if (title != null)
            {
                Title = title;
            }

            // Authors as a List. One book can have several authors.
            JArray jArrayAuthors = jsonObject.volumeInfo["authors"];
            var authors = new List<string>();
            if (jArrayAuthors.IsNullOrEmpty())
            {
                authors.Add("Unknown");
            } else
            {
                foreach (string author in jArrayAuthors)
                {
                    authors.Add(author);
                }
            }
            Authors = authors;

            // Publisher.
            var publisher = jsonObject.volumeInfo["publisher"];
            if (publisher != null)
            {
                Publisher = publisher;
            }

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
            try
            {
                PublishedDate = DateOnly.Parse(dateBefore);
            } catch (Exception ex)
            {
                PublishedDate = DateOnly.Parse("1900-01");
            }
            

            // Description.
            var description = jsonObject.volumeInfo["description"];
            if (description != null)
            {
                Description = description;
            }

            // Page Count.
            var pageCount = jsonObject.volumeInfo["pageCount"];
            if (pageCount == null)
            {
                PageCount = 1;
            } else
            {
                PageCount = pageCount;
            }
            

            // Categories/Genres. One book can have several categories.
            JArray jArrayCategories = jsonObject.volumeInfo["categories"];
            var categories = new List<string>();
            if (jArrayCategories.IsNullOrEmpty())
            {
                categories.Add("Unknown");
            } else
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
            if (jArrayImageLinks == null)
            {
                imageLinks.Add("smallThumbnail", "http://books.google.com/books/content?id=WU9iAAAAcAAJ&printsec=frontcover&img=1&zoom=5&edge=curl&source=gbs_api");
                imageLinks.Add("thumbnail", "http://books.google.com/books/content?id=WU9iAAAAcAAJ&printsec=frontcover&img=1&zoom=1&edge=curl&source=gbs_api");
            } else
            {
                string smallThumbnail = jArrayImageLinks.smallThumbnail;
                string thumbnail = jArrayImageLinks.thumbnail;
                imageLinks.Add("smallThumbnail", smallThumbnail);
                imageLinks.Add("thumbnail", thumbnail);
            }


            ImageLinks = imageLinks;
        }
    }
}
