using System.Text.RegularExpressions;
using Azure.Core;
using Hiof.DotNetCourse.V2023.Group14.ClassLibrary.Classes.V1;
using Hiof.DotNetCourse.V2023.Group14.ClassLibrary.Classes.V1.Security;
using Hiof.DotNetCourse.V2023.Group14.ClassLibrary.Enums.V1;
using Hiof.DotNetCourse.V2023.Group14.LibraryCollectionService.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

namespace Hiof.DotNetCourse.V2023.Group14.LibraryCollectionService.Controllers.V1
{
    [ApiController]
    [Route("api/1.0/libraries")]
    public class V1LibraryCollectionController : ControllerBase
    {
        private readonly LibraryCollectionContext _libraryCollectionContext;

        public V1LibraryCollectionController(LibraryCollectionContext libraryCollectionContext)
        {
            _libraryCollectionContext = libraryCollectionContext;
        }

        [HttpPost]
        [Route("entry")]
        public async Task<ActionResult> CreateEntry(V1LibraryEntry libraryEntry)
        {
            if (libraryEntry != null)
            {
                // Checks to see if there exists at least one ISBN number, and it is of adequate length.
                if (libraryEntry.LibraryEntryISBN13.IsNullOrEmpty() && libraryEntry.LibraryEntryISBN10.IsNullOrEmpty())
                {
                    return BadRequest("The book you are trying to add does not have a valid ISBN");
                }

                if (!libraryEntry.LibraryEntryISBN10.IsNullOrEmpty() && libraryEntry.LibraryEntryISBN10?.Length != 10)
                {
                    return BadRequest("The ISBN10 of the book is of an invalid format.");
                } else if (!libraryEntry.LibraryEntryISBN13.IsNullOrEmpty() && libraryEntry.LibraryEntryISBN13?.Length != 13)
                {
                    return BadRequest("The ISBN13 of the book is of an invalid format.");
                }

                // Rating must be between 1 and 10.
                if (libraryEntry.Rating != null)
                {
                    if (libraryEntry.Rating < 1 || libraryEntry.Rating > 10)
                        return BadRequest("The book rating must be between 1 and 10.");
                }

                await _libraryCollectionContext.LibraryEntries.AddAsync(libraryEntry);
                await _libraryCollectionContext.SaveChangesAsync();

                return Ok();
            }
            
            return BadRequest("You failed to supply a valid library entry.");
        }

        [HttpGet("getEntries")]
        public async Task<IActionResult> GetAllEntries()
        {
            var libEntries = from library in _libraryCollectionContext.LibraryEntries 
                            select library;

            if (libEntries.IsNullOrEmpty())
            {
                return NotFound("No libraries exist.");
            }
            else
            {
                return Ok(libEntries);
            }
        }

        // Returns a specific user's library, complete with a count of all items currently in their library.
        [HttpGet("getUserLibrary")]
        public async Task<IActionResult> GetUserLibrary(Guid userId)
        {
            var libraries = from library in _libraryCollectionContext.LibraryEntries
                            where library.UserId == userId
                            select library;

            if (libraries.IsNullOrEmpty())
            {
                return NotFound("This user either does not exist, or has no entries in their library.");
            }
            else
            {
                var libCollection = new V1LibraryCollection();
                libCollection.UserId = userId;
                libCollection.Entries = new List<V1LibraryEntry>();
                libCollection.ItemsRead = 0;

                foreach (var libEntry in libraries)
                {
                    libCollection.Entries.Add(libEntry);
                    if (libEntry.ReadingStatus == ReadingStatus.Completed)
                    {
                        libCollection.ItemsRead++;
                    }
                }

                libCollection.Items = libCollection.Entries.Count;

                return Ok(libCollection);
            }
        }

        [HttpDelete("deleteEntry")]
        public async Task<ActionResult> DeleteEntry(Guid entryId)
        {
            var libraryEntry = await _libraryCollectionContext.LibraryEntries.FirstOrDefaultAsync(l => l.Id == entryId);

            if (libraryEntry == null)
            {
                return NotFound("An entry with the id '" + entryId + "' was not found.");
            }
            else
            {
                _libraryCollectionContext.LibraryEntries.Remove(libraryEntry);
                await _libraryCollectionContext.SaveChangesAsync();
            }

            return Ok();
        }

        // Deletes a user's library, and all items in that library.
        [HttpDelete("deleteUserLibrary")]
        public async Task<ActionResult> DeleteUserLibrary(Guid userId)
        {
            var libraries = from library in _libraryCollectionContext.LibraryEntries
                            where library.UserId == userId
                            select library;

            if (libraries.IsNullOrEmpty())
            {
                return NotFound("This user either does not exist, or has no entries in their library.");
            }
            else
            {
                foreach (var library in libraries)
                {
                    _libraryCollectionContext.LibraryEntries.Remove(library);
                }

                await _libraryCollectionContext.SaveChangesAsync();
            }

            return Ok();
        }

        // Changes the rating of an individual entry.
        [HttpPut("changeRating")]
        public async Task<ActionResult> ChangeRating(Guid entryId, int rating)
        {
            var libraryEntry = await _libraryCollectionContext.LibraryEntries.FirstOrDefaultAsync(l => l.Id == entryId);

            if (libraryEntry == null)
            {
                return NotFound("An entry with the id '" + entryId + "' was not found.");
            } else if (rating < 1 || rating > 10) 
            {
                // Rating must be between 1 and 10.
                return BadRequest("The book rating must be between 1 and 10.");
            } else
            {
                libraryEntry.Rating = rating;
                _libraryCollectionContext.SaveChanges();
            }

            return Ok();
        }

        // Changes the read date of an individual entry.
        [HttpPut("changeDateRead")]
        public async Task<ActionResult> ChangeDateRead(Guid entryId, DateTime dateTime)
        {
            var libraryEntry = await _libraryCollectionContext.LibraryEntries.FirstOrDefaultAsync(l => l.Id == entryId);

            if (libraryEntry == null)
            {
                return NotFound("An entry with the id '" + entryId + "' was not found.");
            }
            else
            {
                libraryEntry.DateRead = dateTime;
                _libraryCollectionContext.SaveChanges();
            }
            return Ok();
        }

        // Changes the reading status of an individual entry.
        [HttpPut("changeReadingStatus")]
        public async Task<ActionResult> ChangeReadingStatus(Guid entryId, ReadingStatus readingStatus)
        {
            var libraryEntry = await _libraryCollectionContext.LibraryEntries.FirstOrDefaultAsync(l => l.Id == entryId);

            if (libraryEntry == null)
            {
                return NotFound("An entry with the id '" + entryId + "' was not found.");
            }
            
            // A valid ReadingStatus enum is to be supplied (Completed, ToRead, Reading).
            if (!Enum.IsDefined(typeof(ReadingStatus), readingStatus))
            {
                return BadRequest("'" + readingStatus + "' is not a valid reading status.");
            } else
            {
                libraryEntry.ReadingStatus = readingStatus;
                _libraryCollectionContext.SaveChanges();
            }
            return Ok();
        }
    }
}