using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using System.Text.Json.Serialization;

namespace Hiof.DotNetCourse.V2023.Group14.ClassLibrary.Classes.V1
{
    [Table("user_icons", Schema = "dbo")]
    public class V1UserIcon
    {
        public Guid Id { get; set; }
        public string Username { get; set; }
        public byte[] DisplayPicture { get; set; }
    }
}

