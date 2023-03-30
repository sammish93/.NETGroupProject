using Hiof.DotNetCourse.V2023.Group14.ClassLibrary.Enums.V1;
using Hiof.DotNetCourse.V2023.Group14.ClassLibrary.Interfaces.V1;
using Microsoft.EntityFrameworkCore;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data;
using System.Security.Cryptography;
using System.Text.RegularExpressions;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Hiof.DotNetCourse.V2023.Group14.ClassLibrary.Classes.V1
{

    public class V1UserWithDisplayPicture
    {
        public V1User User { get; set; }
        public byte[] DisplayPicture { get; set; }

        public V1UserWithDisplayPicture(V1User user, byte[] displayPicture) 
        { 
            User = user;
            DisplayPicture = displayPicture;
        }
        
    }
        
}