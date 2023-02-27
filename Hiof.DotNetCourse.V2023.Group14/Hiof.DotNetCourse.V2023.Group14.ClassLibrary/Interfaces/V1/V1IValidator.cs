using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Hiof.DotNetCourse.V2023.Group14.ClassLibrary.Interfaces.V1
{
    interface V1IValidator
    {
        // Email must be a correct email format.
        abstract static bool ValidEmail(string email);

        // Password must have at least one lower-case letter, one upper-case letter, one number, and one special character, and be at least 5 characters long.
        abstract static bool ValidPassword(string password);

        // Username must be alphanumeric and be between 5 and 20 characters long.
        abstract static bool ValidUsername(string username);
    }
}
