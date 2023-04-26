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
        _logger.LogInformation("GetAllPosts: Featching all posts from the marketplace.");
        var response = await _service.GetAllPosts();
        if (response != null)
        {
            _logger.LogInformation("GetAllPosts: Successfully featched all posts!");
            return Ok(response);
        }
        else
        {
            _logger.LogWarning("GetAllPosts: No posts found in the marketplace.");
            return NotFound("There are no posts currently in the marketplace.");
        }
    }

    [HttpGet]
    [Route("[action]")]
    public async Task<IActionResult> GetPostById(Guid postId)
    {
        _logger.LogInformation("GetPostById: Fetching post with ID {PostId}.", postId);
        var response = await _service.GetPostById(postId);
        if (response == null)
        {
            _logger.LogWarning("GetPostById: No post found with ID {PostId}.", postId);
            return NotFound("No post has the provided id.");
        }
        else
        {
            _logger.LogInformation("GetPostById: Successfully fetched post with ID {PostId}.", postId);
            return Ok(response);
        }

    }


    [HttpPost]
    [Route("[action]")]
    public async Task<IActionResult> CreateNewPost([Required] Guid ownerId, V1Currency currency, V1BookStatus status, [FromBody] V1MarketplaceBook post)
    {
        _logger.LogInformation("CreateNewPost: Creating a new post with owner ID {OwnerId}", ownerId);
        var newPost = await _service.CreateNewPost(ownerId, currency, status, post);
        if (newPost)
        {
            if (post.Condition.Equals("string"))
            {
                _logger.LogWarning("CreateNewPost: Invalid book condition provided.");
                return BadRequest("Please write about the condition to the book.");
            }
            else if (post.Price == 0)
            {
                _logger.LogWarning("CreateNewPost: Price not set on the book");
                return BadRequest("Need to set a price one the book!");
            }
            _logger.LogInformation("CreateNewPost: New post successfully added with owner ID {OwnerId}", ownerId);
            return Ok("New post successfully added!");
        }
        else
        {
            _logger.LogError("CreateNewPost: Failed to add new post with owner ID {ownerId}", ownerId);
            return BadRequest("Could not add the post.");
        }
    }

    [HttpPut]
    [Route("[action]")]
    public async Task<IActionResult> UpdatePost(Guid postId, V1MarketplaceBookUpdated post)
    {
        _logger.LogInformation("UpdatePost: Updating post with ID {PostId}", postId);
        var response = await _service.UpdatePost(postId, post);

        if (response.Contains("Successfully updated the post!"))
        {
            if (post.Condition.Equals("string"))
            {
                _logger.LogWarning("UpdatePost: Invalid book condition provided.");
                return BadRequest("Please write about the condition to the book.");
            }
            else if (post.Price == 0)
            {
                _logger.LogWarning("UpdatePost: Price not set on the book.");
                return BadRequest("Need to set a price one the book!");
            }
            _logger.LogInformation("UpdatePost: Successfully updated post with ID: {PostId}", postId);
            return Ok(response);
        }
        else if (response.Contains("Wrong ownerId, please provide the right one."))
        {
            _logger.LogWarning("UpdatePost: Wrong owner ID provided for post ID {PostId}", postId);
            return BadRequest(response);
        }
        else
        {
            _logger.LogError("UpdatePost: Failed to update the post with ID {PostId}", postId);
            return NotFound(response);
        }
    }

    [HttpDelete]
    [Route("[action]")]
    public async Task<IActionResult> DeletePost(Guid postId)
    {
        _logger.LogInformation("DeletePost: Deleting post with ID {PostId}", postId);
        var response = await _service.DeletePost(postId);
        if (response)
        {
            _logger.LogInformation("DeletePost: Successfully deleted post with ID {PostId}", postId);
            return Ok($"Post with id: {postId} - successfully deleted!");
        }
        else
        {
            _logger.LogError("DeletePost: Failed to delete post with ID {PostId}. Provided ID does not exist.", postId);
            return NotFound("Provided id does not exist, please provide another one");
        }
    }
   
}

