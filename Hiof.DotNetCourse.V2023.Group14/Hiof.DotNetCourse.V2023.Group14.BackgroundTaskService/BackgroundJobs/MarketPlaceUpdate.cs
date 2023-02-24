using System;
using Hangfire;
using Hangfire.Common;
using Hiof.DotNetCourse.V2023.Group14.ClassLibrary.Classes.V1;
using Newtonsoft.Json;

namespace Hiof.DotNetCourse.V2023.Group14.BackgroundTaskService.BackgroundJobs
{
	/*
		The class is designed to check for updates in the marketplace, by sending
		an http-Get request to the '/market' endpoint (not implemented yet). This
		is to retrieve the current list of posts for sale. It then deserialize the
		response into a list of 'V1MarketplacePost' objects. 
	 */
	public class MarketPlaceUpdate
	{
		private readonly ILogger<MarketPlaceUpdate> _logger;
		private readonly HttpClient _httpClient;

		public MarketPlaceUpdate(ILogger<MarketPlaceUpdate> logger, HttpClient client)
		{
			_logger = logger;
			_httpClient = client;
		}

		public void UpdateMarketPlace()
		{
			_logger.LogInformation("Background job for updating marketplace every hour");
			RecurringJob.AddOrUpdate(() => UpdateMarketPlaceJob("/market"), cronExpression: "0 * * * *");
		}

		public async Task UpdateMarketPlaceJob(string endpoint)
		{
			try
			{
				_logger.LogInformation("Updating market place...");

				var response = await _httpClient.GetAsync(endpoint);
				if (response.IsSuccessStatusCode)
				{
					var content = await response.Content.ReadAsStringAsync();
					var marketPlacePosts = JsonConvert.DeserializeObject<IEnumerable<V1MarketplacePost>>(content);
				}
			}
			catch (Exception ex)
			{
				_logger.LogError(ex, "Error updating marketplace.");
			}
		}
	}
}

