using Hiof.DotNetCourse.V2023.Group14.ClassLibrary.Classes;
using Hiof.DotNetCourse.V2023.Group14.ClassLibrary.Enums;
using System.Xml;

namespace Hiof.DotNetCourse.V2023.Group14.ConsoleTestService
{
    internal class TerminalWindow
    {
        static void Main(string[] args)
        {
            var user = new User("sammish", "sam@samland.no", "Afdkjfsd453kgfFGk43", "sam", "davies", "Norway", "Aalesund", "EN", UserRole.Admin);

            Console.WriteLine("User Information");
            Console.WriteLine("Username: " + user.Username);
            Console.WriteLine("Hashed Password: " + user.Password);
            Console.WriteLine("Role: " + user.Role);
            Console.WriteLine("Registration Date: " + user.RegistrationDate);

            Console.WriteLine();
            Console.WriteLine();
            string currentDirectory = Environment.CurrentDirectory;
            var projectDirectory = Directory.GetParent(Environment.CurrentDirectory).Parent.Parent.Parent.FullName;

            string stringLib = projectDirectory + "\\Models\\Hiof.DotNetCourse.V2023.Group14.ClassLibrary";

            var doc = new XmlDocument();
            doc.Load(stringLib + "\\Secrets.xml");




            //XmlNodeList secrets = doc.GetElementsByTagName("secrets");

        }
    }
}