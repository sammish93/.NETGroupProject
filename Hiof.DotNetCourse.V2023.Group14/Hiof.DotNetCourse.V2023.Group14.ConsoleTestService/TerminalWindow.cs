using Hiof.DotNetCourse.V2023.Group14.ClassLibrary.Classes;
using Hiof.DotNetCourse.V2023.Group14.ClassLibrary.Enums;
using Hiof.DotNetCourse.V2023.Group14.ClassLibrary.Security;
using Microsoft.IdentityModel.Tokens;

namespace Hiof.DotNetCourse.V2023.Group14.ConsoleTestService
{
    internal class TerminalWindow
    {
        static void Main(string[] args)
        {
            
            var user = new User("sammish", "sam@samland.no", "Afdkjfsd453kgfFGk43", "sam", "davies", "Norway", "Aalesund", "EN", UserRole.Admin);

            Console.WriteLine("User Information");
            Console.WriteLine("Username: " + user.UserName);
            Console.WriteLine("Hashed Password: " + user.Password);
            Console.WriteLine("Role: " + user.Role);
            Console.WriteLine("Registration Date: " + user.RegistrationDate);
           

            
            // Testing encryption of password.
            var (hash, salt) = PasswordEncryption.Encrypt("Leon");

            Console.WriteLine($"\nPassword hash: {hash}");
            Console.WriteLine($"Generated Salt: {Convert.ToHexString(salt)}");
      

            // Test to se if the password is the same by passing the same
            // hash and salt.
            var verify = PasswordEncryption.Verify("Leon", hash, salt);
            Console.WriteLine($"Same password?: {verify}");

            String? OperatingSystem = Environment.GetEnvironmentVariable("OS");

            if (OperatingSystem != null && OperatingSystem.ToLower().Contains("windows"))
            {
                Console.WriteLine("You have Windows!");
            }

            Console.WriteLine(OperatingSystem);


        }
    }
}