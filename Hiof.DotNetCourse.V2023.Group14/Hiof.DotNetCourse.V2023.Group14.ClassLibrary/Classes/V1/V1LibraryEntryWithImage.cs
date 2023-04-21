using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Hiof.DotNetCourse.V2023.Group14.ClassLibrary.Enums.V1;


namespace Hiof.DotNetCourse.V2023.Group14.ClassLibrary.Classes.V1
{
    public class V1LibraryEntryWithImage : V1LibraryEntry
    {
        
        public string Thumbnail { get; set; }


        public V1LibraryEntryWithImage(V1LibraryEntry entry, string thumbnail)
        {
            Id = entry.Id;
            UserId = entry.UserId;
            LibraryEntryISBN10 = entry.LibraryEntryISBN10;
            LibraryEntryISBN13 = entry.LibraryEntryISBN13;
            Title = entry.Title;
            MainAuthor = entry.MainAuthor;
            Rating = entry.Rating;
            DateRead = entry.DateRead;
            ReadingStatus = entry.ReadingStatus;

            Thumbnail = thumbnail;
        }

    }
}
