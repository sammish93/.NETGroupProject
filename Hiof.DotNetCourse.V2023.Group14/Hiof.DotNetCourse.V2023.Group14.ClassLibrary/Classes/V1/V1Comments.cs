using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Hiof.DotNetCourse.V2023.Group14.ClassLibrary.Enums.V1;
using Newtonsoft.Json;

namespace Hiof.DotNetCourse.V2023.Group14.ClassLibrary.Classes.V1
{
    [Table("user_comments", Schema = "dbo")]
    public class V1Comments
    {
        [Key]
        
        [Column("id", TypeName = "char(36)")]
        [JsonProperty("id")]
        public Guid Id { get; set; }
        
        [Column("body", TypeName = "nvarchar(max)")]
        [JsonProperty("body")]
        public string Body { get; set; }

        
        [Column("created_at", TypeName = "datetime")]
        [JsonProperty("createdAt")]
        public DateTime CreatedAt { get; set; }
        
        [Column("upvotes")]
        [JsonProperty("upvotes")]
        public int? Upvotes { get; set; }
        [JsonProperty("authorId")]
        [Column("author_id", TypeName = "char(36)")]
        public Guid AuthorId { get; set; }
       
        [Column("parent_comment_id", TypeName = "char(36)")]
        [JsonProperty("parentCommentId")]
        public Guid? ParentCommentId { get; set; }

        public V1Comments? ParentComment { get; set; }

        public List<V1Comments>? Replies { get; set; }

        [Column("comment_type")]
        [JsonProperty("commentType")]
        public CommentType CommentType { get; set; }
        [Column("isbn_10")]
        [JsonProperty("ISBN10")]
        
        public string? ISBN10 { get; set; }

        [Column("isbn_13")]
        [JsonProperty("ISBN13")]
        public string? ISBN13 { get; set; }

        [Column("user_id", TypeName = "char(36)")]
        [JsonProperty("userId")]
        public Guid? UserId { get; set; }

        [NotMapped]
        [JsonIgnore]
        public V1UserWithDisplayPicture? AuthorObject { get; set; }
    }
}
