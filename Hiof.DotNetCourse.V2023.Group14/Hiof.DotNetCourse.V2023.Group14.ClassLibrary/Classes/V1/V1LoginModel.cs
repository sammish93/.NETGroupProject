using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace Hiof.DotNetCourse.V2023.Group14.ClassLibrary.Classes.V1
{
    [Table("login_verification", Schema = "dbo")]
    public class V1LoginModel
    {
        [Key]
        [Column("id", TypeName = "char(36)")]
        public Guid Id { get; set; }

        [Required]
        [Column("username", TypeName = "nvarchar(500)")]
        public string? UserName { get; set; }

        [Required]
        [Column("password", TypeName = "nvarchar(500)")]
        public string? Password { get; set; }

        [Column("salt", TypeName = "nvarchar(500)")]
        public string? Salt { get; set; }

        [JsonIgnore]
        [Column("token", TypeName = "nvarchar(500)")]
        public string? Token { get; set; }

        public V1LoginModel(Guid id, string username, string password, string salt)
        {
            Id = id;
            UserName = username;
            Password = password;
            Salt = salt;
        }

        public V1LoginModel()
        {

        }
    }
}

