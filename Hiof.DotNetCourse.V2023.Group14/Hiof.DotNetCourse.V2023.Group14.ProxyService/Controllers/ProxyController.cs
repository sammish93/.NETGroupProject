using System;
using Microsoft.AspNetCore.Mvc;

namespace Hiof.DotNetCourse.V2023.Group14.ProxyService.Controllers
{
	[Route("[action]")]
	[ApiController]
	public class ProxyController : ControllerBase
	{
		// We will use httpclient to make calls to the other
		// microservices.
		private readonly HttpClient _httpClient;

		public ProxyController(IHttpClientFactory httpClientFactory)
		{
			_httpClient = httpClientFactory.CreateClient();
		}

	}
}

