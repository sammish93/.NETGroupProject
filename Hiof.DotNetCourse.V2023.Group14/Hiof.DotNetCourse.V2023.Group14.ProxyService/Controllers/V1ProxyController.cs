﻿using System;
using Hiof.DotNetCourse.V2023.Group14.ClassLibrary.Classes.V1;
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
		private readonly V1UserAccountApiUrls _apiUrl;

		public V1ProxyController(IHttpClientFactory httpClientFactory, V1UserAccountApiUrls url)
		{
			_httpClient = httpClientFactory.CreateClient();
			_apiUrl = url;
		}

		[HttpGet]
		public async Task<IActionResult> GetUsers()
			=> await Proxy(_apiUrl?.GetUsers);

		[HttpGet]
		public async Task<IActionResult> GetUserById(Guid id)
			=> await Proxy(_apiUrl.GetUserById + $"?guid={id}");

		[HttpGet]
		public async Task<IActionResult> GetUserByName(string name)
			=> await Proxy(_apiUrl.GetUserByName + $"?userName={name}");
		

		[HttpGet]
		public async Task<IActionResult> GetUserByEmail(string email)
			=> await Proxy(_apiUrl.GetUserByEmail + $"?email={email}");

		// This is the method that executes the calls.
		private async Task<ContentResult> Proxy(string url)
			=> Content(await _httpClient.GetStringAsync(url));
	}
}

