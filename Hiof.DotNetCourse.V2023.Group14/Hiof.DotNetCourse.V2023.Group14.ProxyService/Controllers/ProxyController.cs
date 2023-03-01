using System;
using Microsoft.AspNetCore.Mvc;

namespace Hiof.DotNetCourse.V2023.Group14.ProxyService.Controllers
{
	public class ProxyController : ControllerBase
	{
		private readonly HttpClient _httpClient;

		public ProxyController(IHttpClientFactory httpClientFactory)
		{
			_httpClient = httpClientFactory.CreateClient();
		}
	}
}

