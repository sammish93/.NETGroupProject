using Hiof.DotNetCourse.V2023.Group14.ClassLibrary.Classes.V1;
using Hiof.DotNetCourse.V2023.Group14.ClassLibrary.Classes.V1.Security;
using Hiof.DotNetCourse.V2023.Group14.ClassLibrary.Enums.V1;
using Microsoft.IdentityModel.Tokens;
using System.Runtime.InteropServices;

namespace Hiof.DotNetCourse.V2023.Group14.ConsoleTestService
{
    internal class TerminalWindow
    {
        static void Main(string[] args)
        {
            
            var user = new V1User("sammish", "sam@samland.no", "Afdkjfsd453kgfFGk43", "sam", "davies", "Norway", "Aalesund", "EN", UserRole.Admin);

            Console.WriteLine("User Information");
            Console.WriteLine("Username: " + user.UserName);
            Console.WriteLine("Hashed Password: " + user.Password);
            Console.WriteLine("Role: " + user.Role);
            Console.WriteLine("Registration Date: " + user.RegistrationDate); 
           

            
            // Testing encryption of password.
            var (hash, salt) = V1PasswordEncryption.Encrypt("Leon");

            Console.WriteLine($"\nPassword hash: {hash}");
            Console.WriteLine($"Generated Salt: {Convert.ToHexString(salt)}");
      

            // Test to se if the password is the same by passing the same
            // hash and salt.
            var verify = V1PasswordEncryption.Verify("Leon", hash, salt);
            Console.WriteLine($"Same password?: {verify}");
        }
    }
}