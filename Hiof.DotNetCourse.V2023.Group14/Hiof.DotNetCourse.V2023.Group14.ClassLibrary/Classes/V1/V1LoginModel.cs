using Hiof.DotNetCourse.V2023.Group14.ClassLibrary.Enums.V1;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data;
using System.Diagnostics.Metrics;
using System.Text.Json.Serialization;

namespace Hiof.DotNetCourse.V2023.Group14.ClassLibrary.Classes.V1
{
    [Table("login_verification", Schema = "dbo")]
    public class V1LoginModel
    {
        [Key]
        [Column("id")]
        public int Id { get; set; }

        [Required]
        [Column("username", TypeName = "nvarchar(500)")]
        public string? UserName { get; set; }

        [Required]
        [Column("password", TypeName = "nvarchar(500)")]
        public string? Password { get; set; }

        [JsonIgnore]
        [Column("token", TypeName = "nvarchar(500)")]
        public string? Token { get; set; }

        [JsonIgnore]
        [Column("salt", TypeName = "nvarchar(500)")]
        public string? Salt { get; set; }
    }
}

