using Hiof.DotNetCourse.V2023.Group14.ClassLibrary.Enums.V1;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hiof.DotNetCourse.V2023.Group14.ClassLibrary.Classes.V1
{
    [Table("library_entries", Schema = "dbo")]
    public class V1LibraryEntry
    {
        [Key]
        [Column("id", TypeName = "char(36)")]
        public Guid Id { get; set; }
        [Required(ErrorMessage = "{0} is required")]
        [Column("user_id", TypeName = "nvarchar(36)")]
        // Choosing to use a User Guid instead of UserName here because UserNames can be changed.
        public Guid UserId { get; set; }
        [Column("isbn_10", TypeName = "nvarchar(10)")]
        [StringLength(10)]
        public string? LibraryEntryISBN10 { get; set; }
        [Column("isbn_13", TypeName = "nvarchar(13)")]
        [StringLength(13)]
        public string? LibraryEntryISBN13 { get; set; }
        [Column("title", TypeName = "nvarchar(500)")]
        public string? Title { get; set; }
        // A book can have several authors, but this isn't always practical to display on the GUI, especially when there are many books being shown.
        [Column("main_author", TypeName = "nvarchar(500)")]
        public string? MainAuthor { get; set; }
        [Column("rating")]
        [Range(1, 10)]
        public int? Rating { get; set; }
        // Type 'DateOnly' isn't supported in SQL, thus 'DateTime'. Datetime also works parsing only a date string (time is set to 00:00:00).
        [Column("date_read", TypeName = "datetime")]
        public DateTime? DateRead { get; set; }
        [Column("reading_status", TypeName = "nvarchar(20)")]
        [Required(ErrorMessage = "{0} is required")]
        public ReadingStatus ReadingStatus { get; set; }

        public V1LibraryEntry(V1User user, V1Book book, int? rating, DateTime? dateRead, ReadingStatus readingStatus)
        {
            Id = Guid.NewGuid();
            UserId = user.Id;

            string? isbn10 = book.IndustryIdentifiers["ISBN_10"];
            string? isbn13 = book.IndustryIdentifiers["ISBN_13"];
            if (!isbn10.IsNullOrEmpty())
            {
                LibraryEntryISBN10 = isbn10;
            }
            if (!isbn13.IsNullOrEmpty())
            {
                LibraryEntryISBN13 = isbn13;
            }

            Title = book.Title;
            MainAuthor = book.Authors[0];

            Rating = rating;
            DateRead = dateRead;
            ReadingStatus = readingStatus;
        }

        public V1LibraryEntry()
        {

        }
    }
}
