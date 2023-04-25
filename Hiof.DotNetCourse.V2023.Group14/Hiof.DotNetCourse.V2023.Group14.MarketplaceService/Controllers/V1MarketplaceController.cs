using Hiof.DotNetCourse.V2023.Group14.ClassLibrary.Classes.V1;
using Hiof.DotNetCourse.V2023.Group14.ClassLibrary.Enums.V1;
using Hiof.DotNetCourse.V2023.Group14.MarketplaceService.Services;
using Microsoft.AspNetCore.Mvc;

namespace Hiof.DotNetCourse.V2023.Group14.MarketplaceService.Controllers;

[ApiController]
[Route("marketplace/1.0")]
public class V1MarketplaceController : ControllerBase
{
    private readonly ILogger<V1MarketplaceController> _logger;
    private readonly V1MarketplaceService _service;

    public V1MarketplaceController(ILogger<V1MarketplaceController> logger, V1MarketplaceService service)
    {
        _logger = logger;
        _service = service;
    }


    [HttpPost]
    [Route("[action]")]
    public async Task<IActionResult> CreateNewPost(Guid ownerId, V1Currency currency, V1BookStatus status, [FromBody] V1MarketplaceBook post)
    {
        // TODO: Need to add input validation.
        var newPost = await _service.CreateNewPost(ownerId, currency, status, post);
        if (newPost)
        {
            return Ok("New post successfully added!");
        }
        else
        {
            return BadRequest("Could not add the post.");
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

