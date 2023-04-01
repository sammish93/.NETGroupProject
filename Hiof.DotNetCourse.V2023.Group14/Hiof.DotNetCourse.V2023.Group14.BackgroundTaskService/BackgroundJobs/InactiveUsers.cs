using System;
using System.Text;
using System.Text.Json;
using Hiof.DotNetCourse.V2023.Group14.ClassLibrary.Classes.V1;

namespace Hiof.DotNetCourse.V2023.Group14.BackgroundTaskService.BackgroundJobs
{
	public class InactiveUsers
	{
		private readonly HttpClient _httpClient;

		public InactiveUsers(HttpClient client)
		{
			_httpClient = client;
		}

		public async Task CheckInactivity()
		{
            var inactiveTime = DateTime.Now.AddDays(-10);
            var response = await _httpClient.GetAsync("https://localhost:7021/api/1.0/users/getUsers");

            if (!response.IsSuccessStatusCode)
            {
                throw new Exception("Failed to get users from the API");
            }

			var json = await response.Content.ReadAsStringAsync();
			var users = JsonSerializer.Deserialize<List<V1User>>(json);

            var inactiveUsers = users.Where(u => u.LastActive < inactiveTime).ToList();
        }

		private static void SendMailToInactiveUsers(List<V1User> inactiveUsers)
		{
            foreach (var user in inactiveUsers)
            {
                StringBuilder message = new StringBuilder();
                message.Append($"\nUser {user.UserName} has been inactive for 10 days.");
                message.Append("\nPlease log in to the service soon.");

                var result = $"\nMail sent to: {user.Email}.";
                var content = $"\nContent: {message}";
                WriteResultToFile(result, content);
            }
        }

        private static void WriteResultToFile(string result, string content)
        {
            using StreamWriter file = new("TextFile/log.txt", append: true);
            file.Write(result + content);
        }
    }
}

