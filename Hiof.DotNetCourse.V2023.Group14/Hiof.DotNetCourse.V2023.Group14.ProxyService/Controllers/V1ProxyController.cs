using System;
using System.Net.Http.Json;
using System.Text;
using Hiof.DotNetCourse.V2023.Group14.ClassLibrary.Classes.V1;
using Hiof.DotNetCourse.V2023.Group14.ClassLibrary.Enums.V1;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace Hiof.DotNetCourse.V2023.Group14.ProxyService.Controllers
{
	[ApiController]
	[Route("proxy/1.0/")]
	public class V1ProxyController : ControllerBase
	{

		private readonly HttpClient _httpClient;
		private readonly V1ApiUrls _apiUrls;

		public V1ProxyController(IHttpClientFactory httpClientFactory, V1ApiUrls urls)
		{
			_httpClient = httpClientFactory.CreateClient();
			_apiUrls = urls;
		}


		[HttpGet("users/[action]")]
		public async Task<IActionResult> GetAll()
			=> await Proxy(_apiUrls.GetUsers);


		[HttpGet("users/[action]")]
        public async Task<IActionResult> GetById(Guid id)
			=> await Proxy(_apiUrls.GetUserById + $"?guid={id}");


		[HttpGet("users/[action]")]
        public async Task<IActionResult> GetByName(string name)
			=> await Proxy(_apiUrls.GetUserByName + $"?userName={name}");


        [HttpGet("users/[action]")]
        public async Task<IActionResult> GetByEmail(string email)
			=> await Proxy(_apiUrls.GetUserByEmail + $"?email={email}");


		[HttpPost("users/[action]")]
		public async Task<IActionResult> CreateUserAccount(V1User user)
		{
			var url = _apiUrls.CreateUserAccount;
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
			var url = _apiUrls.UpdateUserAccount;
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
			var url = _apiUrls.Delete + $"?Id={id}";
			var response = await _httpClient.DeleteAsync(url);
			
			if (response.IsSuccessStatusCode)
				return Ok();
			else
				return BadRequest(response.ReasonPhrase);
		}


		[HttpDelete("users/[action]")]
		public async Task<IActionResult> DeleteByUsername(string username)
		{
			var url = _apiUrls.DeleteByUsername + $"?username={username}";
			var response = await _httpClient.DeleteAsync(url);

            if (response.IsSuccessStatusCode)
                return Ok();
            else
                return BadRequest(response.ReasonPhrase);
        }


		[HttpGet("books/[action]")]
		public async Task<IActionResult> GetByIsbn(string isbn)
			=> await Proxy(_apiUrls.GetBookByIsbn + $"?isbn={isbn}");


		[HttpGet("books/[action]")]
		public async Task<IActionResult> GetByTitle(string title, int? maxResults, string? langRestrict)
            => await Proxy(_apiUrls.GetBookByTitle + ConcatUri("title", title, maxResults, langRestrict));


		[HttpGet("books/[action]")]
		public async Task<IActionResult> GetByAuthor(string name, int? maxResults, string? langRestrict)
			=> await Proxy(_apiUrls.GetBookByAuthor + ConcatUri("authors", name, maxResults, langRestrict));


		[HttpGet("books/[action]")]
		public async Task<IActionResult> GetBookByCategory(string subject, int? maxResults, string? langRestrict)
			=> await Proxy(_apiUrls.GetBookByCategory + ConcatUri("subject", subject, maxResults, langRestrict));


		[HttpPost("libraries/[action]")]
		public async Task<IActionResult> CreateEntry(V1LibraryEntry entry)
		{
			var url = _apiUrls.LibraryEntry;
			using var content = SerializeToJsonString(entry);
            using var response = await _httpClient.PostAsync(url, content);

			if (response.IsSuccessStatusCode)
				return Ok();
			else
				return BadRequest(response.RequestMessage);
        }


		[HttpGet("libraries/[action]")]
		public async Task<IActionResult> GetAllEntries()
			=> await Proxy(_apiUrls.LibraryGetEntries);


		[HttpGet("libraries/[action]")]
		public async Task<IActionResult> GetEntry()
			=> await Proxy(_apiUrls.LibraryGetEntry);


		[HttpGet("libraries/[action]")]
		public async Task<IActionResult> GetUserLibrary(Guid userId)
			=> await Proxy(_apiUrls.GetUserLibrary + $"?userId={userId}");


		[HttpDelete("libraries/[action]")]
		public async Task<IActionResult> DeleteEntry(Guid entry)
		{
			var url = _apiUrls.LibraryDeleteEntry + $"?entryId={entry}";
			var response = await _httpClient.DeleteAsync(url);

			if (response.IsSuccessStatusCode)
				return Ok();
			else
				return BadRequest(response.ReasonPhrase);
		}


		[HttpDelete("libraries/[action]")]
		public async Task<IActionResult> DeleteUserLibrary(Guid userId)
		{
            var url = _apiUrls.LibraryDeleteUserLibrary + $"?userId={userId}";
            var response = await _httpClient.DeleteAsync(url);

            if (response.IsSuccessStatusCode)
                return Ok();
            else
                return BadRequest(response.ReasonPhrase);
        }

		[HttpPut("libraries/[action]")]
		public async Task<IActionResult> ChangeRating(Guid entryId, int rating)
		{
			var url = $"{_apiUrls.LibraryChangeRating}?entryId={entryId}&rating={rating}";
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
            var url = $"{_apiUrls.LibraryChangeDateRead}?entryId={entryId}&dateTime={dateTime}";
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
			var url = $"{_apiUrls.LibraryChangeReadingStatus}?entryId={entryId}&readingStatus={readingStatus}";
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

