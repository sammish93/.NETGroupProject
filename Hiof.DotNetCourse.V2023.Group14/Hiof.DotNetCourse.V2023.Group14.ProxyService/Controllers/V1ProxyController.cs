using System;
using Microsoft.AspNetCore.Mvc;

namespace Hiof.DotNetCourse.V2023.Group14.ProxyService.Controllers
{
	[Route("[action]")]
	[ApiController]
	public class V1ProxyController : ControllerBase
	{
		// We will use httpclient to make calls to the other
		// microservices.
		private readonly HttpClient _httpClient;

		public V1ProxyController(IHttpClientFactory httpClientFactory)
		{
			_httpClient = httpClientFactory.CreateClient();
		}

		[HttpGet]
		public async Task<IActionResult> GetUsers()
			=> await ProxyTo("https://localhost:7021/api/1.0/users/getUsers");

		[HttpGet]
		public async Task<IActionResult> GetUserById(Guid id)
			=> await ProxyTo($"https://localhost:7021/api/1.0/users/getUserById?guid={id}");


		private async Task<ContentResult> ProxyTo(string url)
			=> Content(await _httpClient.GetStringAsync(url));
	}
}

