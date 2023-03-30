using System;
using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace Hiof.DotNetCourse.V2023.Group14.ClassLibrary.Classes.V1
{
    // Model used to handle files in proxy.
    public class V1AddIconInputModel
    {
        [Required]
        public Guid Id { get; set; }

        [Required]
        public string Username { get; set; }

        [Required]
        public IFormFile File { get; set; }
    }
}

