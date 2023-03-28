using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using static System.Reflection.Metadata.BlobBuilder;

namespace Hiof.DotNetCourse.V2023.Group14.ClassLibrary.Classes.V1
{
    public class V1LibraryCollection
    {
        [Column(TypeName = "char(36)")]
        public Guid? UserId { get; set; }
        // Total number of books in a user's library.
        public int? Items { get; set; }
        // Total number of books that a user has completed in their library.
        public int? ItemsRead { get; set; }
        public IList<V1LibraryEntry>? Entries { get; set; }

        public V1LibraryCollection() { }

        public V1LibraryCollection(string jsonString) 
        {
            dynamic? jsonObject = JsonConvert.DeserializeObject(jsonString);

            UserId = jsonObject.userId;

            Items = jsonObject.items;

            ItemsRead = jsonObject.itemsRead;

            // Creates a V1Book object for each book item from the search result.
            var libraryEntries = new List<V1LibraryEntry>();
            var jArrayLibEntries = jsonObject.entries;
            foreach (JObject jObjectItem in jArrayLibEntries)
            {
                var libEntry = new V1LibraryEntry(jObjectItem.ToString());
                libraryEntries.Add(libEntry);
            }
            Entries = libraryEntries;
        }

        
    }
}
