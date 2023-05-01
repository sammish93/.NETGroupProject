using System.Diagnostics;
using System.Runtime.InteropServices;
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

        private readonly ILogger<V1LibraryCollectionController> _logger;
        private readonly EventLog _eventLog;

        public V1LibraryCollectionController(LibraryCollectionContext libraryCollectionContext, ILogger<V1LibraryCollectionController> logger)
        {
            _libraryCollectionContext = libraryCollectionContext;
            _logger = logger;

            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            {
                _eventLog = new EventLog();
                _eventLog.Source = "V1LibraryCollectionService";
            }
        }

        [HttpPost]
        [Route("entry")]
        public async Task<ActionResult> CreateEntry(V1LibraryEntry libraryEntry)
        {
            string message = "";

            if (libraryEntry != null)
            {
                // Checks to see if there exists at least one ISBN number, and it is of adequate length.
                if (libraryEntry.LibraryEntryISBN13.IsNullOrEmpty() && libraryEntry.LibraryEntryISBN10.IsNullOrEmpty())
                {
                    message = "POST method 'entry' was called with an invalid ISBN.";
                    _logger.LogWarning(message);
                    if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows) && _eventLog != null)
                    {
                        _eventLog.WriteEntry(message, EventLogEntryType.Warning);
                    }

                    return BadRequest("The book you are trying to add does not have a valid ISBN.");
                }

                if (!libraryEntry.LibraryEntryISBN10.IsNullOrEmpty() && libraryEntry.LibraryEntryISBN10?.Length != 10)
                {
                    message = $"POST method 'entry' was called with an invalid ISBN10 - {libraryEntry.LibraryEntryISBN10}.";
                    _logger.LogWarning(message);
                    if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows) && _eventLog != null)
                    {
                        _eventLog.WriteEntry(message, EventLogEntryType.Warning);
                    }

                    return BadRequest("The ISBN10 of the book is of an invalid format.");
                } else if (!libraryEntry.LibraryEntryISBN13.IsNullOrEmpty() && libraryEntry.LibraryEntryISBN13?.Length != 13)
                {
                    message = $"POST method 'entry' was called with an invalid ISBN13 - {libraryEntry.LibraryEntryISBN13}.";
                    _logger.LogWarning(message);
                    if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows) && _eventLog != null)
                    {
                        _eventLog.WriteEntry(message, EventLogEntryType.Warning);
                    }

                    return BadRequest("The ISBN13 of the book is of an invalid format.");
                }

                // Rating must be between 1 and 10.
                if (libraryEntry.Rating != null)
                {
                    if (libraryEntry.Rating < 1 || libraryEntry.Rating > 10)
                    {
                        message = $"POST method 'entry' was called with a rating less than 1 or greater than 10 - ({libraryEntry.Rating}).";
                        _logger.LogWarning(message);
                        if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows) && _eventLog != null)
                        {
                            _eventLog.WriteEntry(message, EventLogEntryType.Warning);
                        }

                        return BadRequest("The book rating must be between 1 and 10.");
                    }
                }
                        

                await _libraryCollectionContext.LibraryEntries.AddAsync(libraryEntry);
                await _libraryCollectionContext.SaveChangesAsync();

                message = $"GET method 'entry' was called successfully, and an entry was created with the GUID {libraryEntry.Id}.";
                _logger.LogInformation(message);
                if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows) && _eventLog != null)
                {
                    _eventLog.WriteEntry(message, EventLogEntryType.Information);
                }

                return Ok();
            }

            message = "POST method 'entry' was unsuccessfully called with an invalid entry.";
            _logger.LogWarning(message);
            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows) && _eventLog != null)
            {
                _eventLog.WriteEntry(message, EventLogEntryType.Warning);
            }

            return BadRequest("You failed to supply a valid library entry.");
        }

        [HttpGet("getEntries")]
        public async Task<IActionResult> GetAllEntries()
        {
            var libEntries = await (from library in _libraryCollectionContext.LibraryEntries 
                            select library).ToListAsync();

            string message = "";

            if (libEntries.IsNullOrEmpty())
            {
                message = "GET method 'getEntries' was called but no libraries exist.";
                _logger.LogWarning(message);
                if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows) && _eventLog != null)
                {
                    _eventLog.WriteEntry(message, EventLogEntryType.Warning);
                }

                return NotFound("No libraries exist.");
            }
            else
            {
                message = $"GET method 'getEntries' was called successfully, and returned {libEntries.Count} result(s).";
                _logger.LogInformation(message);
                if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows) && _eventLog != null)
                {
                    _eventLog.WriteEntry(message, EventLogEntryType.Information);
                }

                return Ok(libEntries);
            }
        }

        // Returns a single entry.
        [HttpGet("getEntry")]
        public async Task<IActionResult> GetEntry(Guid entryId)
        {
            var entry = await _libraryCollectionContext.LibraryEntries.FirstOrDefaultAsync(l => l.Id == entryId);
            string message = "";

            if (entry == null)
            {
                message = $"GET method 'getEntry' was called but no library exists with the GUID {entryId}.";
                _logger.LogWarning(message);
                if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows) && _eventLog != null)
                {
                    _eventLog.WriteEntry(message, EventLogEntryType.Warning);
                }

                return NotFound("No entry exists.");
            }
            else
            {
                message = $"GET method 'getEntry' was called successfully, and returned a result with the GUID {entryId}.";
                _logger.LogInformation(message);
                if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows) && _eventLog != null)
                {
                    _eventLog.WriteEntry(message, EventLogEntryType.Information);
                }

                return Ok(entry);
            }
        }

        [HttpGet("getEntryFromSpecificUser")]
        public async Task<IActionResult> GetEntryFromSpecificUser(Guid userId, String isbn)
        {
            var entry = await (from library in _libraryCollectionContext.LibraryEntries
                        where library.UserId == userId
                        && (library.LibraryEntryISBN10 == isbn || library.LibraryEntryISBN13 == isbn)
                        select library).ToListAsync();

            string message = "";

            if (entry.IsNullOrEmpty())
            {
                message = $"GET method 'getEntryFromSpecificUser' was called but no entry exists with the ISBN of {isbn} from the user with the GUID of {userId}.";
                _logger.LogWarning(message);
                if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows) && _eventLog != null)
                {
                    _eventLog.WriteEntry(message, EventLogEntryType.Warning);
                }

                return NotFound("No entry exists.");
            }
            else
            {
                message = $"GET method 'getEntryFromSpecificUser' was called successfully, and returned a result with the ISBN of {isbn} from the " +
                    "user with the GUID of {userId}.";
                _logger.LogInformation(message);
                if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows) && _eventLog != null)
                {
                    _eventLog.WriteEntry(message, EventLogEntryType.Information);
                }

                return Ok(entry);
            }
        }

        // Returns a specific user's library, complete with a count of all items currently in their library.
        [HttpGet("getUserLibrary")]
        public async Task<IActionResult> GetUserLibrary(Guid userId)
        {
            var libraries = await (from library in _libraryCollectionContext.LibraryEntries
                            where library.UserId == userId
                            orderby library.DateRead descending
                            select library).ToListAsync();

            string message = "";

            if (libraries.IsNullOrEmpty())
            {
                message = $"GET method 'getUserLibrary' was called but no user exists with the GUID {userId}, or user exists but does not have a library.";
                _logger.LogWarning(message);
                if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows) && _eventLog != null)
                {
                    _eventLog.WriteEntry(message, EventLogEntryType.Warning);
                }

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

                message = $"GET method 'getUserLibrary' was called successfully, and returned a library with from the user with the GUID {userId}.";
                _logger.LogInformation(message);
                if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows) && _eventLog != null)
                {
                    _eventLog.WriteEntry(message, EventLogEntryType.Information);
                }

                return Ok(libCollection);
            }
        }

        // Returns the most recent books (by date read in descending order) from a user's library. The ReadingStatus enum is optional. If none is provided then all books will be included.
        [HttpGet("GetUserMostRecentBooks")]
        public async Task<IActionResult> GetUserMostRecentBooks(Guid userId, int numberOfResults, ReadingStatus? readingStatus)
        {
            List<V1LibraryEntry> libraries;
            if (readingStatus != null)
            {
                libraries = await (from library in _libraryCollectionContext.LibraryEntries
                                       where library.UserId == userId
                                       where library.ReadingStatus == readingStatus
                                       orderby library.DateRead descending
                                       select library).ToListAsync();
            } else
            {
                libraries = await (from library in _libraryCollectionContext.LibraryEntries
                                       where library.UserId == userId
                                       orderby library.DateRead descending
                                       select library).ToListAsync();
            }

            string message = "";
            

            if (libraries.IsNullOrEmpty())
            {
                message = $"GET method 'GetUserMostRecentBooks' was called but no user exists with the GUID {userId}, or user exists but does not have a library.";
                _logger.LogWarning(message);
                if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows) && _eventLog != null)
                {
                    _eventLog.WriteEntry(message, EventLogEntryType.Warning);
                }

                return NotFound("This user either does not exist, or has no entries in their library.");
            }
            else
            {
                int results = 0;
                var libCollection = new V1LibraryCollection();
                libCollection.UserId = userId;
                libCollection.Entries = new List<V1LibraryEntry>();
                libCollection.ItemsRead = 0;

                foreach (var libEntry in libraries)
                {
                    if (results < numberOfResults)
                    {
                        if (libEntry.ReadingStatus == readingStatus)
                        {
                            libCollection.Entries.Add(libEntry);
                            libCollection.ItemsRead++;
                            results++;
                        } else
                        {
                            libCollection.Entries.Add(libEntry);
                            libCollection.ItemsRead++;
                            results++;
                        }
                    }
                }

                libCollection.Items = libCollection.Entries.Count;

                message = $"GET method 'GetUserMostRecentBooks' was called successfully, and returned a library with from the user with the GUID {userId}.";
                _logger.LogInformation(message);
                if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows) && _eventLog != null)
                {
                    _eventLog.WriteEntry(message, EventLogEntryType.Information);
                }

                return Ok(libCollection);
            }
        }

        // Returns the most recent books (by date read in descending order) from a user's library. The ReadingStatus enum is optional. If none is provided then all books will be included.
        [HttpGet("GetUserHighestRatedBooks")]
        public async Task<IActionResult> GetUserHighestRatedBooks(Guid userId, int numberOfResults)
        {

            var libraries = await (from library in _libraryCollectionContext.LibraryEntries
                                where library.UserId == userId
                                orderby library.Rating descending
                                select library).ToListAsync();

            string message = "";

            if (libraries.IsNullOrEmpty())
            {
                message = $"GET method 'GetUserHighestRatedBooks' was called but no user exists with the GUID {userId}, or user exists but does not have a library.";
                _logger.LogWarning(message);
                if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows) && _eventLog != null)
                {
                    _eventLog.WriteEntry(message, EventLogEntryType.Warning);
                }

                return NotFound("This user either does not exist, or has no entries in their library.");
            }
            else
            {
                int results = 0;
                var libCollection = new V1LibraryCollection();
                libCollection.UserId = userId;
                libCollection.Entries = new List<V1LibraryEntry>();
                libCollection.ItemsRead = 0;

                foreach (var libEntry in libraries)
                {
                    if (results < numberOfResults)
                    {
                            libCollection.Entries.Add(libEntry);
                            libCollection.ItemsRead++;
                            results++;
                    }
                }

                libCollection.Items = libCollection.Entries.Count;

                message = $"GET method 'GetUserHighestRatedBooks' was called successfully, and returned a library with from the user with the GUID {userId}.";
                _logger.LogInformation(message);
                if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows) && _eventLog != null)
                {
                    _eventLog.WriteEntry(message, EventLogEntryType.Information);
                }

                return Ok(libCollection);
            }
        }

        [HttpDelete("deleteEntry")]
        public async Task<ActionResult> DeleteEntry(Guid entryId)
        {
            var libraryEntry = await _libraryCollectionContext.LibraryEntries.FirstOrDefaultAsync(l => l.Id == entryId);
            string message = "";

            if (libraryEntry == null)
            {
                message = $"DELETE method 'deleteEntry' was called but no entry exists with the GUID {entryId}.";
                _logger.LogWarning(message);
                if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows) && _eventLog != null)
                {
                    _eventLog.WriteEntry(message, EventLogEntryType.Warning);
                }

                return NotFound("An entry with the id '" + entryId + "' was not found.");
            }
            else
            {
                _libraryCollectionContext.LibraryEntries.Remove(libraryEntry);
                await _libraryCollectionContext.SaveChangesAsync();
            }

            message = $"DELETE method 'deleteEntry' was called successfully, and deleted an entry with the GUID {entryId}.";
            _logger.LogInformation(message);
            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows) && _eventLog != null)
            {
                _eventLog.WriteEntry(message, EventLogEntryType.Information);
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

            string message = "";

            if (libraries.IsNullOrEmpty())
            {
                message = "DELETE method 'deleteUserLibrary' was called but no user exists with the GUID {userId}.";
                _logger.LogWarning(message);
                if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows) && _eventLog != null)
                {
                    _eventLog.WriteEntry(message, EventLogEntryType.Warning);
                }

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

            message = $"DELETE method 'deleteUserLibrary' was called successfully, and deleted a userwith the GUID {userId}.";
            _logger.LogInformation(message);
            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows) && _eventLog != null)
            {
                _eventLog.WriteEntry(message, EventLogEntryType.Information);
            }

            return Ok();
        }

        // Changes the rating of an individual entry.
        [HttpPut("changeRating")]
        public async Task<ActionResult> ChangeRating(Guid entryId, int rating)
        {
            var libraryEntry = await _libraryCollectionContext.LibraryEntries.FirstOrDefaultAsync(l => l.Id == entryId);
            string message = "";

            if (libraryEntry == null)
            {
                message = $"PUT method 'changeRating' was called but no entry exists with the GUID {entryId}.";
                _logger.LogWarning(message);
                if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows) && _eventLog != null)
                {
                    _eventLog.WriteEntry(message, EventLogEntryType.Warning);
                }

                return NotFound("An entry with the id '" + entryId + "' was not found.");
            } else if (rating < 1 || rating > 10) 
            {
                // Rating must be between 1 and 10.
                message = $"PUT method 'changeRating' was called but value given ({rating}) for rating was either less than 1 or greater than 10.";
                _logger.LogWarning(message);
                if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows) && _eventLog != null)
                {
                    _eventLog.WriteEntry(message, EventLogEntryType.Warning);
                }

                return BadRequest("The book rating must be between 1 and 10.");
            } else
            {
                libraryEntry.Rating = rating;
                _libraryCollectionContext.SaveChanges();
            }

            message = $"PUT method 'changeRating' was called successfully, and modified an entry with the GUID {entryId}.";
            _logger.LogInformation(message);
            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows) && _eventLog != null)
            {
                _eventLog.WriteEntry(message, EventLogEntryType.Information);
            }

            return Ok();
        }

        // Changes the read date of an individual entry.
        [HttpPut("changeDateRead")]
        public async Task<ActionResult> ChangeDateRead(Guid entryId, DateTime dateTime)
        {
            var libraryEntry = await _libraryCollectionContext.LibraryEntries.FirstOrDefaultAsync(l => l.Id == entryId);
            string message = "";

            if (libraryEntry == null)
            {
                message = $"PUT method 'changeDateRead' was called but no entry exists with the GUID {entryId}.";
                _logger.LogWarning(message);
                if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows) && _eventLog != null)
                {
                    _eventLog.WriteEntry(message, EventLogEntryType.Warning);
                }
                return NotFound("An entry with the id '" + entryId + "' was not found.");
            }
            else
            {
                libraryEntry.DateRead = dateTime;
                _libraryCollectionContext.SaveChanges();
            }

            message = $"PUT method 'changeDateRead' was called successfully, and modified an entry with the GUID {entryId}.";
            _logger.LogInformation(message);
            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows) && _eventLog != null)
            {
                _eventLog.WriteEntry(message, EventLogEntryType.Information);
            }

            return Ok();
        }

        // Changes the reading status of an individual entry.
        [HttpPut("changeReadingStatus")]
        public async Task<ActionResult> ChangeReadingStatus(Guid entryId, ReadingStatus readingStatus)
        {
            var libraryEntry = await _libraryCollectionContext.LibraryEntries.FirstOrDefaultAsync(l => l.Id == entryId);
            string message = "";

            if (libraryEntry == null)
            {
                message = $"PUT method 'changeReadingStatus' was called but no entry exists with the GUID {entryId}.";
                _logger.LogWarning(message);
                if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows) && _eventLog != null)
                {
                    _eventLog.WriteEntry(message, EventLogEntryType.Warning);
                }

                return NotFound("An entry with the id '" + entryId + "' was not found.");
            }
            
            // A valid ReadingStatus enum is to be supplied (Completed, ToRead, Reading).
            if (!Enum.IsDefined(typeof(ReadingStatus), readingStatus))
            {
                message = $"PUT method 'changeReadingStatus' was called but value given ({readingStatus}) for reading status was invalid.";
                _logger.LogWarning(message);
                if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows) && _eventLog != null)
                {
                    _eventLog.WriteEntry(message, EventLogEntryType.Warning);
                }

                return BadRequest("'" + readingStatus + "' is not a valid reading status.");
            } else
            {
                libraryEntry.ReadingStatus = readingStatus;
                _libraryCollectionContext.SaveChanges();
            }

            message = $"PUT method 'changeReadingStatus' was called successfully, and modified an entry with the GUID {entryId}.";
            _logger.LogInformation(message);
            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows) && _eventLog != null)
            {
                _eventLog.WriteEntry(message, EventLogEntryType.Information);
            }

            return Ok();
        }
    }
}