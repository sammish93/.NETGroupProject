// THIS IS JUST AN EXAMPLE CLASS - WILL BE DELETED LATER
// Controller class that manages the book items

using System;
using Hiof.DotNetCourse.V2023.Group14.First.Service.DataTransferObject;
using Microsoft.AspNetCore.Mvc;

namespace Hiof.DotNetCourse.V2023.Group14.First.Service.Controllers
{
	// Each web API controller should inherit from 'ControllerBase'.
	// It provides many methods for handling HTTP-requests.
	[ApiController]
	[Route("books")]
	public class BookController : ControllerBase
	{
		// This is the list we will return when someone request books from
		// our API. Since we do not have a database yet.
		private static readonly List<BookDto> books = new()
		{
			// Guid is used to create a unique ID.
			new BookDto(Guid.NewGuid(), ISBN: "1234", Title: "C# and .NET", Author: "Stian xD"),
			new BookDto(Guid.NewGuid(), ISBN: "9876", Title: "Lord of the Rings", Author: "Tolkien"),
			new BookDto(Guid.NewGuid(), ISBN: "5364", Title: "I dont know", Author: "Mr lowalowa"),
		};

		// This will return a list of the books in the method above
		[HttpGet]
		public IEnumerable<BookDto> Get()
		{
			return books;
		}

		// This will return a book specified by the id.
		[HttpGet("{id}")]
		public BookDto? GetById(Guid id)
		{

			var book = books.Where(book => book.Id == id).SingleOrDefault();
			return book;
		}

		// This will create a new book and add it to the static list.
		[HttpPost]
		public ActionResult<BookDto> Post(CreateBookDto createBookDto)
		{
			var book = new BookDto(Guid.NewGuid(), createBookDto.ISBN, createBookDto.Title, createBookDto.Author);
			books.Add(book);

			return CreatedAtAction(nameof(GetById), new { id = book.Id }, book);
		}

		// This will update a book in the static list.
		[HttpPut("{id}")]
		public IActionResult Put(Guid id, UpdateBookDto updateBookDto)
		{
			var existingBook = books.Where(book => book.Id == id).SingleOrDefault();
			if (existingBook is not null)
			{
                var updatedBook = existingBook with
                {
                    ISBN = updateBookDto.ISBN,
                    Title = updateBookDto.Title,
                    Author = updateBookDto.Author
                };

                var index = books.FindIndex(existingBook => existingBook.Id == id);
                books[index] = updatedBook;
            }
			
			return NoContent();
		}

		// This will delete a book in the static list.
		[HttpDelete("{id}")]
		public IActionResult Delete(Guid id)
		{
			var index = books.FindIndex(existingBook => existingBook.Id == id);
			books.RemoveAt(index);

			return NoContent();
		}

	}
}


