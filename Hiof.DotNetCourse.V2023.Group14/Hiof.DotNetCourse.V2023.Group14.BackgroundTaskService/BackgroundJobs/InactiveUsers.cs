using System;
namespace Hiof.DotNetCourse.V2023.Group14.BackgroundTaskService.BackgroundJobs
{
	public class InactiveUsers
	{
		public InactiveUsers()
		{
		}

		public void CheckInactivity()
		{
            var inactiveTime = DateTime.Now.AddDays(-10);
        }
	}
}

