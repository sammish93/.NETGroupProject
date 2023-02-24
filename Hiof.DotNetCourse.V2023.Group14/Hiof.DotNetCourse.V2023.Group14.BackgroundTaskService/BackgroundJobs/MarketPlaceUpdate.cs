using System;
using Hangfire.Common;
using Hiof.DotNetCourse.V2023.Group14.ClassLibrary.Classes.V1;
using Newtonsoft.Json;

namespace Hiof.DotNetCourse.V2023.Group14.BackgroundTaskService.BackgroundJobs
{
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
		}

		public async Task UpdateMarketPlaceJob()
		{
			try
			{
				_logger.LogInformation("Updating market place...");

				var response = await _httpClient.GetAsync("/market");
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

