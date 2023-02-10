using Hiof.DotNetCourse.V2023.Group14.ClassLibrary.Classes;
using Hiof.DotNetCourse.V2023.Group14.ClassLibrary.Enums;

namespace Hiof.DotNetCourse.V2023.Group14.ConsoleTestService
{
    internal class TerminalWindow
    {
        static void Main(string[] args)
        {
            /*
            var user = new User("sammish", "sam@samland.no", "Afdkjfsd453kgfFGk43", "sam", "davies", "Norway", "Aalesund", "EN", UserRole.Admin);

            Console.WriteLine("User Information");
            Console.WriteLine("Username: " + user.UserName);
            Console.WriteLine("Hashed Password: " + user.Password);
            Console.WriteLine("Role: " + user.Role);
            Console.WriteLine("Registration Date: " + user.RegistrationDate);
            */


            PasswordEncryption encryption = new PasswordEncryption();
            var hash = encryption.EncryptPassword("Leon", out var salt);

            Console.WriteLine($"Password hash: {hash}");
            Console.WriteLine($"Generated Salt: {Convert.ToHexString(salt)}");

            var verify = encryption.verifyPassword("Leon", hash, salt);
            Console.WriteLine($"Same password?: {verify}");

        }
    }
}