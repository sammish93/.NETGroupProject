using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Hiof.DotNetCourse.V2023.Group14.ClassLibrary.Enums.V1;

namespace Hiof.DotNetCourse.V2023.Group14.ClassLibrary.Classes.V1
{
    [Table("user_comments", Schema = "dbo")]
    public class V1Comments
    {
        [Key]
        [Column("id", TypeName = "char(36)")]
        public Guid Id { get; set; }

        [Column("body", TypeName = "nvarchar(max)")]
        public string Body { get; set; }

        [Column("created_at", TypeName = "datetime")]
        public DateTime CreatedAt { get; set; }

        [Column("upvotes")]
        public int? Upvotes { get; set; }
        
        [Column("author_id", TypeName = "char(36)")]
        public Guid AuthorId { get; set; }
       
        [Column("parent_comment_id", TypeName = "char(36)")]
        public Guid? ParentCommentId { get; set; }

        public V1Comments ParentComment { get; set; }

        public List<V1Comments> Replies { get; set; }

        [Column("comment_type")]
        public CommentType CommentType { get; set; }

        [Column("isbn_10")]
        public string? ISBN10 { get; set; }

        [Column("isbn_13")]
        public string? ISBN13 { get; set; }

        [Column("user_id", TypeName = "char(36)")]
        public Guid? UserId { get; set; }


    }
}
