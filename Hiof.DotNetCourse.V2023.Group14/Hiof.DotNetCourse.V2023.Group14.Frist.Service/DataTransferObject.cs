// THIS IS JUST AN EXAMPLE CLASS - WILL BE DELETED LATER
// References: https://www.youtube.com/watch?v=CqCDOosvZIk

using System;
namespace Hiof.DotNetCourse.V2023.Group14.First.Service.DataTransferObject
{
    // This DTO is going to return information from our GET-operation.
    public record BookDto(Guid Id, string ISBN, string Title, string Author);

    // This DTO is used to create Books
    public record CreateBookDto(string ISBN, string Title, string Author);

    // DTO to update a book
    public record UpdateBookDto(string ISBN, string Title, string Author);

}

