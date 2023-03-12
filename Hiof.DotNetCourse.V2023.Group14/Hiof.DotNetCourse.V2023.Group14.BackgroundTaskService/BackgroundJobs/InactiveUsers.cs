using System;
using System.Text;
using Hiof.DotNetCourse.V2023.Group14.ClassLibrary.Classes.V1;
using Hiof.DotNetCourse.V2023.Group14.UserAccountService.Data;
using Microsoft.EntityFrameworkCore;

namespace Hiof.DotNetCourse.V2023.Group14.BackgroundTaskService.BackgroundJobs
{
	public class InactiveUsers
	{
		// Checks if a user has been inactive for 10 days.
		public async Task CheckInactiveUsers(UserAccountContext dbContext)
		{
			var inactiveTime = DateTime.Now.AddDays(-10);

            using (var context = dbContext)
			{
				var inactiveUsers = await dbContext.Users.Where(u => u.LastActive < inactiveTime).ToListAsync();

				sendMailToInactiveUsers(inactiveUsers);
            }
		}

		private static void sendMailToInactiveUsers(List<V1User> inactiveUsers)
		{
			foreach (var user in inactiveUsers)
			{
				StringBuilder message = new StringBuilder();
				message.Append($"User {user.UserName} has been inactive for 10 days.");
				message.Append("\nPlease log in to the service soon.");

				sendMail(user.Email, message.ToString());
			}

		}

		private static void sendMail(string email, string content)
		{
			Console.WriteLine($"Sending mail to: {email}.");
            Console.WriteLine($"Content:\n{content}");
        }
	}
}

