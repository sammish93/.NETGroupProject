using System;
using Hiof.DotNetCourse.V2023.Group14.ClassLibrary.Classes.V1;
using Hiof.DotNetCourse.V2023.Group14.ClassLibrary.Enums.V1;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;

namespace Hiof.DotNetCourse.V2023.Group14.CommentService.Data
{
    public class CommentServiceContext : DbContext
    {
        public DbSet<V1Comments> Comments { get; set; }
       
        public CommentServiceContext(DbContextOptions<CommentServiceContext> options) : base(options)
        {

        }



        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            
             

            modelBuilder.Entity<V1Comments>().HasData(new V1Comments
            {
                Id = Guid.Parse("4a14dc4d-aa39-4ee5-bc34-b46701c3ca09"),
                Body = "This book was good. At times it was a bit boring and difficult to read.",
                CreatedAt = DateTime.Parse("2023-02-24T12:55:19.113"),
                AuthorId = Guid.Parse("3fa85f64-5717-4562-b3fc-2c963f66afa6"),
               
                Upvotes = 0,
                CommentType = ClassLibrary.Enums.V1.CommentType.Book,
                ISBN10 = "1440674132",
                ISBN13 = "9781440674136",
                


            },
             new V1Comments
             {
                 Id = Guid.Parse("0e6145cc-4150-43a8-ae24-81e8a69fad7f"),
                 Body = "I agree with how difficult it was to read at times",
                 CreatedAt = DateTime.Parse("2023-02-25T11:55:19.113"),
                 AuthorId = Guid.Parse("54AF86BF-346A-4CBA-B36F-527748E1CB93"),
                 ParentCommentId = Guid.Parse("4a14dc4d-aa39-4ee5-bc34-b46701c3ca09"),
                 Upvotes = 0,
                 CommentType = ClassLibrary.Enums.V1.CommentType.Reply,
                


             },
              new V1Comments
              {
                  Id = Guid.Parse("8f899b47-6229-4795-8cba-ba644a479d55"),
                  Body = "It was an okay book",
                  CreatedAt = DateTime.Parse("2023-02-25T11:59:19.113"),
                  AuthorId = Guid.Parse("E8CC12BA-4DF6-4B06-B96E-9AD00A927A93"),
                 
                  Upvotes = 0,
                  CommentType = ClassLibrary.Enums.V1.CommentType.Book,
                  ISBN10 = "1440674132",
                  ISBN13 = "9781440674136"
              },
            new V1Comments
            {
                Id = Guid.Parse("48dca027-f9c8-4c92-bbf9-db1dfdcc0a00"),
                Body = "Great collection of books!",
                CreatedAt = DateTime.Parse("2023-02-27T11:55:19.113"),
                AuthorId = Guid.Parse("54AF86BF-346A-4CBA-B36F-527748E1CB93"),
               
                Upvotes = 0,
                CommentType = ClassLibrary.Enums.V1.CommentType.User,
                
                UserId = Guid.Parse("3FA85F64-5717-4562-B3FC-2C963F66AFA6")
            });






        }

    }
}
