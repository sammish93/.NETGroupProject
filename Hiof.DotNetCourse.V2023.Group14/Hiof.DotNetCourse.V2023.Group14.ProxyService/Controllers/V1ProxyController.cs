using System;
using System.ComponentModel.DataAnnotations;
using System.Net;
using System.Text;
using Hiof.DotNetCourse.V2023.Group14.ClassLibrary.Classes.V1;
using Hiof.DotNetCourse.V2023.Group14.ClassLibrary.Classes.V1.MessageModels;
using Hiof.DotNetCourse.V2023.Group14.ClassLibrary.Classes.V1.Security;
using Hiof.DotNetCourse.V2023.Group14.ClassLibrary.Enums.V1;
using Hiof.DotNetCourse.V2023.Group14.ProxyService.Configuration;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using static System.Collections.Specialized.BitVector32;

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
        public async Task<IActionResult> GetUsersByName(string name)
            => await Proxy(_apiUrls.Value.GetUsersByName + $"?userName={name}");

        [HttpGet("users/[action]")]
        public async Task<IActionResult> GetUsersByCity(string city)
           => await Proxy(_apiUrls.Value.GetUsersByCity + $"?city={city}");

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
        public async Task<IActionResult> GetEntry(Guid entryId)
            => await Proxy(_apiUrls.Value.LibraryGetEntry + $"?entryId={entryId}");

        [HttpGet("libraries/[action]")]
        public async Task<IActionResult> GetEntryFromSpecificUser(Guid userId, String isbn)
            => await Proxy(_apiUrls.Value.GetEntryFromSpecificUser + $"?userId={userId}&isbn={isbn}");


        [HttpGet("libraries/[action]")]
        public async Task<IActionResult> GetUserLibrary(Guid userId)
            => await Proxy(_apiUrls.Value.GetUserLibrary + $"?userId={userId}");

        [HttpGet("libraries/[action]")]
        public async Task<IActionResult> GetUserMostRecentBooks(Guid userId, int numberOfResults, ReadingStatus? readingStatus)
        {
            if (readingStatus != null)
            {
                return await Proxy(_apiUrls.Value.GetUserMostRecentBooks + $"?userId={userId}&numberOfResults={numberOfResults}&readingStatus={readingStatus}");
            }
            else
            {
                return await Proxy(_apiUrls.Value.GetUserMostRecentBooks + $"?userId={userId}&numberOfResults={numberOfResults}");
            }

        }

        [HttpGet("libraries/[action]")]
        public async Task<IActionResult> GetUserHighestRatedBooks(Guid userId, int numberOfResults)
            => await Proxy(_apiUrls.Value.GetUserHighestRatedBooks + $"?userId={userId}&numberOfResults={numberOfResults}");


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


        [HttpGet("icons/[action]")]
        public async Task<IActionResult> GetIconById([Required] Guid id)
            => await Proxy($"{_apiUrls.Value.GetIconById}?id={id}");


        [HttpGet("icons/[action]")]
        public async Task<IActionResult> GetIconByName([Required] string username)
            => await Proxy($"{_apiUrls.Value.GetIconByName}?username={username}");


        [HttpPost("icons/[action]")]
        public async Task<IActionResult> AddIcon([FromForm] V1AddIconInputModel icon)
        {
            var url = _apiUrls.Value.AddIcon;

            using var content = new MultipartFormDataContent
            {
                { new StringContent(icon.Username), "Username" },
                { new StringContent(icon.Id.ToString()), "Id" },
                { new StreamContent(icon.File.OpenReadStream()), "file", icon.File.FileName }
            };

            using var response = await _httpClient.PostAsync(url, content);

            if (response.IsSuccessStatusCode)
                return Ok("Image successfully added!");
            else
                return BadRequest("Could not add the image.");
        }


        // TODO: Fix the bug in this mehtod.
        [HttpPut("icons/[action]")]
        public async Task<IActionResult> UpdateFromForm([FromForm] V1AddIconInputModel icon)
        {
            var url = _apiUrls.Value.UpdateIconFromForm;

            using var content = new MultipartFormDataContent
            {
                { new StringContent(icon.Username), "Username" },
                { new StringContent(icon.Id.ToString()), "Id" },
                { new StreamContent(icon.File.OpenReadStream()), "File" }
            };

            using var response = await _httpClient.PutAsync(url, content);

            if (response.IsSuccessStatusCode)
                return Ok("Image successfully updated!");
            else
                return BadRequest("Could not update the image.");
        }

        [HttpPut("icons/[action]")]
        public async Task<IActionResult> Update(V1UserIcon icon)
        {
            var url = _apiUrls.Value.UpdateIcon;


            var requestBodyJson = JsonConvert.SerializeObject(icon);
            var requestContent = new StringContent(requestBodyJson, Encoding.UTF8, "application/json");

            var response = await _httpClient.PutAsync(url, requestContent);

            if (response.IsSuccessStatusCode)
                return Ok("Image successfully updated!");
            else
                return BadRequest("Could not update the image.");
        }


        [HttpDelete("icons/[action]")]
        public async Task<IActionResult> DeleteIcon(Guid id)
        {
            var url = _apiUrls.Value.DeleteIcon + $"?id={id}";

            var response = await _httpClient.DeleteAsync(url);

            if (response.IsSuccessStatusCode)
                return Ok("Image successfully deleted!");
            else
                return BadRequest("Could not delete the image.");

        }

        [HttpPost("goals/[action]")]
        public async Task<IActionResult> CreateReadingGoal(V1ReadingGoals readingGoal)
        {
            var url = _apiUrls.Value.CreateReadingGoal;
            using var content = SerializeToJsonString(readingGoal);
            using var response = await _httpClient.PostAsync(url, content);

            if (response.IsSuccessStatusCode)
                return Ok("New Reading goal added");
            else
                return BadRequest(response.RequestMessage);

        }

        [HttpGet("goals/[action]")]
        public async Task<IActionResult> GetAllGoals([Required] Guid userId)
            => await Proxy($"{_apiUrls.Value.GetAllGoals}?userId={userId}");

        [HttpGet("goals/[action]")]
        public async Task<IActionResult> GetGoalId([Required] Guid userId, DateTime goalDate)
         => await Proxy($"{_apiUrls.Value.GetGoalId}?userId={userId}&goalDate={goalDate.ToString("yyyy/MM/dd")}");

        [HttpGet("goals/[action]")]
        public async Task<IActionResult> GetRecentGoal([Required] Guid userId)
            => await Proxy($"{_apiUrls.Value.GetRecentGoal}?userId={userId}");

        [HttpPut("goals/[action]")]

        public async Task<IActionResult> IncrementReadingGoal(Guid id, int amount)
        {
            var url = $"{_apiUrls.Value.IncrementReadingGoal}?id={id}&incrementAmount={amount}";
            var content = new StringContent(amount.ToString(), Encoding.UTF8, "application/json");

            var response = await _httpClient.PutAsync(url, content);

            if (response.IsSuccessStatusCode)
                return Ok();
            else
                return BadRequest(response.RequestMessage);

        }

        [HttpPut("goals/[action]")]

        public async Task<IActionResult> ModifyReadingGoal(Guid id, V1ReadingGoals readingGoal)
        {
            var url = $"{_apiUrls.Value.ModifyReadingGoal}?id={id}";

            using var content = SerializeToJsonString(readingGoal);
            using var response = await _httpClient.PutAsync(url, content);

            if (response.IsSuccessStatusCode)
                return Ok();
            else
                return BadRequest(response.RequestMessage);
        }

        [HttpDelete("goals/[action]")]
        public async Task<IActionResult> DeleteReadingGoal(Guid id)
        {
            var url = $"{_apiUrls.Value.DeleteReadingGoal}?id={id}";
            var response = await _httpClient.DeleteAsync(url);

            if (response.IsSuccessStatusCode)
                return Ok();
            else
                return BadRequest(await response.Content.ReadAsStringAsync());
        }


        [HttpGet("messages/[action]")]
        public async Task<IActionResult> GetByConversationId(Guid conversationId)
            => await Proxy($"{_apiUrls.Value.GetByConversationId}?id={conversationId}");


        [HttpGet("messages/[action]")]
        public async Task<IActionResult> GetByParticipant(string name)
            => await Proxy($"{_apiUrls.Value.GetByParticipant}?name={name}");


        [HttpPost("messages/[action]")]
        public async Task<IActionResult> CreateNewConversation(Guid conversationId, [FromBody] IEnumerable<string> participants)
        {
            var url = $"{_apiUrls.Value.CreateNewConversation}?conversationId={conversationId}";
            using var content = SerializeToJsonString(participants);

            using var response = await _httpClient.PostAsync(url, content);

            if (response.IsSuccessStatusCode)
            {
                return Ok("New conversation successfully created!");
            }
            else
            {
                var errorString = await response.Content.ReadAsStringAsync();
                return BadRequest(errorString);
            }
        }


        [HttpPost("messages/[action]")]
        public async Task<IActionResult> AddMessageToConversation(Guid conversationId, string sender, [FromBody] string message)
        {
            var url = $"{_apiUrls.Value.AddMessageToConversation}?conversationId={conversationId}&sender={sender}";

            using var content = SerializeToJsonString(message);
            using var response = await _httpClient.PostAsync(url, content);

            if (response.IsSuccessStatusCode)
            {
                return Ok(await response.Content.ReadAsStringAsync());
            }
            else
            {
                return BadRequest(await response.Content.ReadAsStringAsync());
            }
        }


        [HttpPost("messages/[action]")]
        public async Task<IActionResult> AddReactionToMessage(Guid messageId, ReactionType reaction)
        {
            var url = $"{_apiUrls.Value.AddReactionToMessage}?messageId={messageId}&reaction={reaction}";
            using var content = SerializeToJsonString(new { reaction });
            using var response = await _httpClient.PostAsync(url, content);

            if (response.IsSuccessStatusCode)
            {
                return Ok(await response.Content.ReadAsStringAsync());
            }
            else
            {
                return BadRequest(await response.Content.ReadAsStringAsync());
            }
        }


        [HttpPut("messages/[action]")]
        public async Task<IActionResult> UpdateMessage(Guid messageId, string newMessage)
        {
            var url = $"{_apiUrls.Value.UpdateMessage}?messageId={messageId}&newMessage={newMessage}";
            using var content = SerializeToJsonString(new { newMessage });
            using var response = await _httpClient.PutAsync(url, content);

            if (response.IsSuccessStatusCode)
            {
                return Ok(await response.Content.ReadAsStringAsync());
            }
            else
            {
                return BadRequest(await response.Content.ReadAsStringAsync());
            }
        }

        [HttpPut("messages/[action]")]
        public async Task<IActionResult> UpdateIsRead(Guid conversationId, string participantId, bool isRead)
        {
            var url = $"{_apiUrls.Value.UpdateIsRead}?conversationId={conversationId}&participantId={participantId}&isRead={isRead}";
            var content = new StringContent(isRead.ToString(), Encoding.UTF8, "application/json");
            using var response = await _httpClient.PutAsync(url, content);

            if (response.IsSuccessStatusCode)
            {
                return Ok(await response.Content.ReadAsStringAsync());
            }
            else
            {
                return BadRequest(await response.Content.ReadAsStringAsync());
            }
        }


        [HttpDelete("messages/[action]")]
        public async Task<IActionResult> DeleteConversation(Guid conversationId)
        {
            var url = $"{_apiUrls.Value.DeleteConversation}?conversationId={conversationId}";
            using var response = await _httpClient.DeleteAsync(url);

            if (response.IsSuccessStatusCode)
            {
                return Ok(await response.Content.ReadAsStringAsync());
            }
            else
            {
                return BadRequest(await response.Content.ReadAsStringAsync());
            }
        }


        [HttpDelete("messages/[action]")]
        public async Task<IActionResult> DeleteMessage(Guid messageId)
        {
            var url = $"{_apiUrls.Value.DeleteMessage}?messageId={messageId}";
            using var response = await _httpClient.DeleteAsync(url);

            if (response.IsSuccessStatusCode)
            {
                return Ok(await response.Content.ReadAsStringAsync());
            }
            else
            {
                return BadRequest(await response.Content.ReadAsStringAsync());
            }
        }


        // TODO: Implement marketplace service to the proxy.

        [HttpGet]
        [Route("marketplace/[action]")]
        public async Task<IActionResult> GetAllPosts()
            => await Proxy(_apiUrls.Value.GetAllPosts);


        [HttpGet]
        [Route("marketplace/[action]")]
        public async Task<IActionResult> GetPostById(Guid postId)
            => await Proxy($"{_apiUrls.Value.GetPostById}?postId={postId}");


        [HttpPost]
        [Route("marketplace/[action]")]
        public async Task<IActionResult> CreateNewPost(Guid ownerId, V1Currency currency, V1BookStatus status, [FromBody] V1MarketplaceBook post)
        {
            var url = $"{_apiUrls.Value.CreateNewPost}?ownerId={ownerId}&currency={currency}&status={status}";
            using var content = SerializeToJsonString(post);
            using var response = await _httpClient.PostAsync(url, content);

            if (response.IsSuccessStatusCode)
            {
                return Ok(await response.Content.ReadAsStringAsync());
            }
            else
            {
                return BadRequest(await response.Content.ReadAsStringAsync());
            }

        }


        private async Task<IActionResult> Proxy(string url)
        {
            var response = await _httpClient.GetAsync(url);

            if (response.IsSuccessStatusCode)
            {
                return Content(await response.Content.ReadAsStringAsync(), "application/json");
            }
            else if (response.StatusCode == HttpStatusCode.NotFound)
            {
                return NotFound(await response.Content.ReadAsStringAsync());
            }
            else
            {
                return StatusCode((int)response.StatusCode);
            }
        }


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

