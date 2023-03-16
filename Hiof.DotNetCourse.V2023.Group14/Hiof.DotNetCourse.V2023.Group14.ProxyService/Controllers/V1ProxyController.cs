using System;
using System.Net.Http.Json;
using System.Text;
using Hiof.DotNetCourse.V2023.Group14.ClassLibrary.Classes.V1;
using Hiof.DotNetCourse.V2023.Group14.ClassLibrary.Classes.V1.Security;
using Hiof.DotNetCourse.V2023.Group14.ClassLibrary.Enums.V1;
using Hiof.DotNetCourse.V2023.Group14.ProxyService.Configuration;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;

namespace Hiof.DotNetCourse.V2023.Group14.ProxyService.Controllers
{
	[ApiController]
	[Route("proxy/1.0/")]
	public class V1ProxyController : ControllerBase
	{

		private readonly HttpClient _httpClient;
		private readonly IOptions<ProxySettings> _apiUrls;

		public V1ProxyController(IHttpClientFactory httpClientFactory, IOptions<ProxySettings> urls)
		{
			_httpClient = httpClientFactory.CreateClient();
			_apiUrls = urls;
		}

		[HttpPost("login/[action]")]
		public async Task<IActionResult> Verification(V1LoginInfo info)
		{
			var url = _apiUrls.Value.LoginVerification;
			var content = SerializeToJsonString(info);

			var response = await _httpClient.PostAsync(url, content);

            if (response.IsSuccessStatusCode)
                return Ok();
            else
                return BadRequest(response.ReasonPhrase);
        }


		[HttpGet("users/[action]")]
		public async Task<IActionResult> GetAll()
			=> await Proxy(_apiUrls.Value.GetUsers);


		[HttpGet("users/[action]")]
        public async Task<IActionResult> GetById(Guid id)
			=> await Proxy(_apiUrls.Value.GetUserById + $"?guid={id}");


		[HttpGet("users/[action]")]
        public async Task<IActionResult> GetByName(string name)
			=> await Proxy(_apiUrls.Value.GetUserByName + $"?userName={name}");


        [HttpGet("users/[action]")]
        public async Task<IActionResult> GetByEmail(string email)
			=> await Proxy(_apiUrls.Value.GetUserByEmail + $"?email={email}");


		[HttpPost("users/[action]")]
		public async Task<IActionResult> CreateUserAccount(V1User user)
		{
			var url = _apiUrls.Value.CreateUserAccount;
			using var content = SerializeToJsonString(user);
            using var response = await _httpClient.PostAsync(url, content);

            if (response.IsSuccessStatusCode)
                return Ok();
            else
                return BadRequest(response.ReasonPhrase);
        }


		[HttpPut("users/[action]")]
		public async Task<IActionResult> UpdateAccountById(V1User user)
		{
			var url = _apiUrls.Value.UpdateUserAccount;
			using var content = SerializeToJsonString(user);
			using var response = await _httpClient.PutAsync(url, content);

			if (response.IsSuccessStatusCode)
				return Ok();
			else
				return BadRequest(response.ReasonPhrase);

		}

		
		[HttpDelete("users/[action]")]
		public async Task<IActionResult> DeleteById(Guid id)
		{
			var url = _apiUrls.Value.Delete + $"?Id={id}";
			var response = await _httpClient.DeleteAsync(url);
			
			if (response.IsSuccessStatusCode)
				return Ok();
			else
				return BadRequest(response.ReasonPhrase);
		}


		[HttpDelete("users/[action]")]
		public async Task<IActionResult> DeleteByUsername(string username)
		{
			var url = _apiUrls.Value.DeleteByUsername + $"?username={username}";
			var response = await _httpClient.DeleteAsync(url);

            if (response.IsSuccessStatusCode)
                return Ok();
            else
                return BadRequest(response.ReasonPhrase);
        }


		[HttpGet("books/[action]")]
		public async Task<IActionResult> GetByIsbn(string isbn)
			=> await Proxy(_apiUrls.Value.GetBookByIsbn + $"?isbn={isbn}");


		[HttpGet("books/[action]")]
		public async Task<IActionResult> GetByTitle(string title, int? maxResults, string? langRestrict)
            => await Proxy(_apiUrls.Value.GetBookByTitle + ConcatUri("title", title, maxResults, langRestrict));


		[HttpGet("books/[action]")]
		public async Task<IActionResult> GetByAuthor(string name, int? maxResults, string? langRestrict)
			=> await Proxy(_apiUrls.Value.GetBookByAuthor + ConcatUri("authors", name, maxResults, langRestrict));


		[HttpGet("books/[action]")]
		public async Task<IActionResult> GetBookByCategory(string subject, int? maxResults, string? langRestrict)
			=> await Proxy(_apiUrls.Value.GetBookByCategory + ConcatUri("subject", subject, maxResults, langRestrict));


		[HttpPost("libraries/[action]")]
		public async Task<IActionResult> CreateEntry(V1LibraryEntry entry)
		{
			var url = _apiUrls.Value.LibraryEntry;
			using var content = SerializeToJsonString(entry);
            using var response = await _httpClient.PostAsync(url, content);

			if (response.IsSuccessStatusCode)
				return Ok();
			else
				return BadRequest(response.RequestMessage);
        }


		[HttpGet("libraries/[action]")]
		public async Task<IActionResult> GetAllEntries()
			=> await Proxy(_apiUrls.Value.LibraryGetEntries);


		[HttpGet("libraries/[action]")]
		public async Task<IActionResult> GetEntry()
			=> await Proxy(_apiUrls.Value.LibraryGetEntry);


		[HttpGet("libraries/[action]")]
		public async Task<IActionResult> GetUserLibrary(Guid userId)
			=> await Proxy(_apiUrls.Value.GetUserLibrary + $"?userId={userId}");


		[HttpDelete("libraries/[action]")]
		public async Task<IActionResult> DeleteEntry(Guid entry)
		{
			var url = _apiUrls.Value.LibraryDeleteEntry + $"?entryId={entry}";
			var response = await _httpClient.DeleteAsync(url);

			if (response.IsSuccessStatusCode)
				return Ok();
			else
				return BadRequest(response.ReasonPhrase);
		}


		[HttpDelete("libraries/[action]")]
		public async Task<IActionResult> DeleteUserLibrary(Guid userId)
		{
            var url = _apiUrls.Value.LibraryDeleteUserLibrary + $"?userId={userId}";
            var response = await _httpClient.DeleteAsync(url);

            if (response.IsSuccessStatusCode)
                return Ok();
            else
                return BadRequest(response.ReasonPhrase);
        }

		[HttpPut("libraries/[action]")]
		public async Task<IActionResult> ChangeRating(Guid entryId, int rating)
		{
			var url = $"{_apiUrls.Value.LibraryChangeRating}?entryId={entryId}&rating={rating}";
			var content = new StringContent(rating.ToString(), Encoding.UTF8, "application/json");

			var response = await _httpClient.PutAsync(url, content);

            if (response.IsSuccessStatusCode)
                return Ok();
            else
                return BadRequest(response.ReasonPhrase);
        }

		[HttpPut("libraries/[action]")]
		public async Task<IActionResult> ChangeDateRead(Guid entryId, DateTime dateTime)
		{
            var url = $"{_apiUrls.Value.LibraryChangeDateRead}?entryId={entryId}&dateTime={dateTime}";
			var content = new StringContent(dateTime.ToString(), Encoding.UTF8, "application/json");

			var response = await _httpClient.PutAsync(url, content);

            if (response.IsSuccessStatusCode)
                return Ok();
            else
                return BadRequest(response.ReasonPhrase);
        }


		[HttpPut("libraries/[action]")]
		public async Task<IActionResult> ChangeReadingStatus(Guid entryId, ReadingStatus readingStatus)
		{
			var url = $"{_apiUrls.Value.LibraryChangeReadingStatus}?entryId={entryId}&readingStatus={readingStatus}";
			var content = new StringContent(readingStatus.ToString(), Encoding.UTF8, "application/json");

			var response = await _httpClient.PutAsync(url, content);

            if (response.IsSuccessStatusCode)
                return Ok();
            else
                return BadRequest(response.ReasonPhrase);
        }


        // This is the method that executes the calls.
        private async Task<ContentResult> Proxy(string url)
			=> Content(await _httpClient.GetStringAsync(url));


		private static string ConcatUri(string search, string param, int? maxResults, string? langRestrict)
		{
			StringBuilder uri = new StringBuilder($"?{search}={param}");

            if (maxResults != null)
            {
                uri.Append("&maxResults=");
                uri.Append((int)maxResults.Value);
            }
            if (langRestrict != null)
            {
                uri.Append("&langRestrict=");
                uri.Append(langRestrict);
            }

			return uri.ToString();
        }


		// Used to convert objects to Json strings.
		private static StringContent SerializeToJsonString<T>(T data)
			=> new StringContent
			(
				JsonConvert.SerializeObject(data),
				Encoding.UTF8,
				"application/json"
			);
		
	}
}

