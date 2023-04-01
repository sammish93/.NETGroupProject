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
        
        public string ImageSource { get; set; }
        public V1LibraryEntry Entry { get; }
      

      
        public V1LibraryEntryWithImage(Guid id, string title, string mainAuthor,int? rating, ReadingStatus readingStatus, string imageSource)
        {
            Id = id;
            Title = title;
            MainAuthor = mainAuthor;
            Rating = rating;
            ReadingStatus = readingStatus;
            ImageSource = imageSource;
        }

    }
}
