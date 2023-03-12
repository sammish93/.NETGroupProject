using System;
using System.Text;
using Hiof.DotNetCourse.V2023.Group14.ClassLibrary.Classes.V1;
using Hiof.DotNetCourse.V2023.Group14.UserAccountService.Data;

namespace Hiof.DotNetCourse.V2023.Group14.BackgroundTaskService.BackgroundJobs
{
	public class InactiveUsers
	{
		// Checks if a user has been inactive for 10 days.
		public void CheckInactiveUsers(UserAccountContext dbContext)
		{
			var inactiveTime = DateTime.Now.AddDays(-10);

            using (var context = dbContext)
			{
				var inactiveUsers = dbContext.Users.Where(u => u.LastActive < inactiveTime);

				sendMailToInactiveUsers(inactiveUsers);
            }
		}

		public void sendMailToInactiveUsers(IQueryable<V1User> inactiveUsers)
		{
			foreach (var user in inactiveUsers)
			{
				StringBuilder message = new StringBuilder();
				message.Append($"User {user.UserName} has been inactive for 10 days.");
				message.Append("\nPlease log in to the service soon.");

				sendMail(user.Email, message.ToString());
			}

		}

		public void sendMail(string email, string content)
		{
			Console.WriteLine($"Sending mail to: {email}.");
            Console.WriteLine($"Content:\n{content}");
        }
	}
}

