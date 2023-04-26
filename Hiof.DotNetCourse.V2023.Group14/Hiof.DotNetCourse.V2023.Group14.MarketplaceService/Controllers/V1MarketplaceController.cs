using System.ComponentModel.DataAnnotations;
using Hiof.DotNetCourse.V2023.Group14.ClassLibrary.Classes.V1;
using Hiof.DotNetCourse.V2023.Group14.ClassLibrary.Classes.V1.MarketplaceModels;
using Hiof.DotNetCourse.V2023.Group14.ClassLibrary.Enums.V1;
using Hiof.DotNetCourse.V2023.Group14.MarketplaceService.Services;
using Microsoft.AspNetCore.Mvc;

namespace Hiof.DotNetCourse.V2023.Group14.MarketplaceService.Controllers;

[ApiController]
[Route("marketplace/1.0")]
public class V1MarketplaceController : ControllerBase
{
    // TODO: Implement logging in the API-service.
    // TODO: Write unit tests for the API.
    private readonly ILogger<V1MarketplaceController> _logger;
    private readonly V1MarketplaceService _service;

    public V1MarketplaceController(ILogger<V1MarketplaceController> logger, V1MarketplaceService service)
    {
        _logger = logger;
        _service = service;
    }

    [HttpGet]
    [Route("[action]")]
    public async Task<IActionResult> GetAllPosts()
    {
        var response = await _service.GetAllPosts();
        if (response != null)
        {
            return Ok(response);
        }
        else
        {
            return NotFound("There are no posts currently in the marketplace.");
        }
    }

    [HttpGet]
    [Route("[action]")]
    public async Task<IActionResult> GetPostById(Guid postId)
    {
        var response = await _service.GetPostById(postId);
        if (response == null)
        {
            return NotFound("No post has the provided id.");
        }
        else
        {
            return Ok(response);
        }

    }


    [HttpPost]
    [Route("[action]")]
    public async Task<IActionResult> CreateNewPost([Required] Guid ownerId, V1Currency currency, V1BookStatus status, [FromBody] V1MarketplaceBook post)
    {
        var newPost = await _service.CreateNewPost(ownerId, currency, status, post);
        if (newPost)
        {
            if (post.Condition.Equals("string"))
            {
                return BadRequest("Please write about the condition to the book.");
            }
            else if (post.Price == 0)
            {
                return BadRequest("Need to set a price one the book!");
            }
            return Ok("New post successfully added!");
        }
        else
        {
            return BadRequest("Could not add the post.");
        }
    }

    [HttpPut]
    [Route("[action]")]
    public async Task<IActionResult> UpdatePost(Guid postId, V1MarketplaceBookUpdated post)
    {
        var response = await _service.UpdatePost(postId, post);
        if (response.Contains("Successfully updated the post!"))
        {
            if (post.Condition.Equals("string"))
            {
                return BadRequest("Please write about the condition to the book.");
            }
            else if (post.Price == 0)
            {
                return BadRequest("Need to set a price one the book!");
            }
            return Ok(response);
        }
        else if (response.Contains("Wrong ownerId, please provide the right one."))
        {
            return BadRequest(response);
        }
        else
        {
            return NotFound(response);
        }
    }

    [HttpDelete]
    [Route("[action]")]
    public async Task<IActionResult> DeletePost(Guid postId)
    {
        var response = await _service.DeletePost(postId);
        if (response)
        {
            return Ok($"Post with id: {postId} - successfully deleted!");
        }
        else
        {
            return NotFound("Provided id does not exist, please provide another one");
        }
    }
   
}

