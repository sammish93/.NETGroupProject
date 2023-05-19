using Hiof.DotNetCourse.V2023.Group14.ClassLibrary.Classes.V1;
using Hiof.DotNetCourse.V2023.Group14.UserDisplayPictureService.Services;
using Hiof.DotNetCourse.V2023.Group14.UserDisplayPictureService.Data;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;
using Hiof.DotNetCourse.V2023.Group14.ClassLibrary.Interfaces.V1;

namespace Hiof.DotNetCourse.V2023.Group14.UserDisplayPictureService.Controllers;

[ApiController]
[Route("icons/1.0")]
public class V1DisplayPictureController : ControllerBase
{

    private readonly V1UserIconService _userIconService;
    private readonly UserIconContext _userIconContext;


    public V1DisplayPictureController(V1UserIconService service, UserIconContext userIconContext)
    {
        _userIconService = service;
        _userIconContext = userIconContext;
    }

    /// <summary>
    /// Get an icon by its ID.
    /// </summary>
    /// <param name="id">The ID of the icon to retrieve.</param>
    /// <response code="200">Returns an V1UserIcon object for the specified id</response>
    /// <response code="400">Bad request error</response>
    /// <response code="404">Not found error.</response>
    [HttpGet("[action]")]
    [Produces("application/json")]
    public async Task<ActionResult> GetById([Required] Guid id)
    {
        if (id == Guid.Empty)
        {
            return BadRequest("ID parameter cannot be an emtpy guid!");
        }
        var userIcon = await _userIconService.GetById(id);

        if (CheckForNull(userIcon))
        {
            return NotFound("User does not exists.");
        }
        else
        {
            return Ok(userIcon);
        }
    }

    /// <summary>
    /// Gets the user icon for the specified username.
    /// </summary>
    /// <param name="username">The username of the user whose icon to retrieve.</param>
    /// <response code="200">Returns an object of V1UserIcon for the specified username.</response>
    /// <response code="400">Bad request error</response>
    /// <response code="404">Not found error.</response>
    [HttpGet("[action]")]
    [Produces("application/json")]
    public async Task<ActionResult> GetByUsername([Required] string username)
    {
        if (string.IsNullOrEmpty(username))
        {
            return BadRequest("Username parameter cannot be null or empty!");
        }

        var userIcon = await _userIconService.GetByUsername(username);

        if (CheckForNull(userIcon))
        {
            return NotFound("User does not exists.");
        }
        else
        {
            return Ok(userIcon);
        }
    }

    /// <summary>
    /// Endpoint to add a new user icon to the database.
    /// </summary>
    /// <param name="icon">The user icon object containing the ID, username,
    /// and display picture as a byte array.</param>
    /// <param name="file">The image file to be uploaded.</param>
    /// <response code="200">Returns an Ok result if the upload was successful</response>
    /// <response code="400">Bad request error with corresponding errormessage.</response>
    [HttpPost("[action]")]
    [Produces("application/json")]
    public async Task<ActionResult> Add([FromForm] V1AddIconInputModel icon)
    {
        if (icon.File == null || icon.File.Length == 0)
        {
            return BadRequest("File parameter cannot be null or empty.");
        }

        var nameSize = icon.Username.Length;

        if (nameSize < 5 || nameSize > 15)
        {
            return BadRequest("Username length must be between 5 - 15 characters.");
        }

        var idSize = icon.Id.ToString().Length;

        if (idSize > 36 || idSize <= 0)
        {
            string msg = "Id cannot be bigger or smaller than 36 characters ";
            return BadRequest(msg);
        }

        if (string.IsNullOrEmpty(icon.Username))
        {
            return BadRequest("Username parameter cannot be null!");
        }

        if (icon.Id.ToString() == null)
        {
            return BadRequest("ID parameter cannot be null!");
        }

        using var memoryStream = new MemoryStream();
        await icon.File.CopyToAsync(memoryStream);
        var byteArray = memoryStream.ToArray();

        var result = new V1UserIcon
        {
            Id = icon.Id,
            Username = icon.Username,
            DisplayPicture = byteArray
        };

        await _userIconService.Add(result);
        return Ok(result);
    }

    /// <summary>
    /// Updates a user icon with the provided V1UserIcon object and image file.
    /// </summary>
    /// <param name="icon">The V1UserIcon object to update.</param>
    /// <param name="file">The image file to update the user icon with.</param>
    /// <response code="200">An ActionResult indicating success of the update operation.</response>
    /// <response code="400">Bad request error with corresponding errormessage.</response>
    [HttpPut("[action]")]
    [Produces("application/json")]
    public async Task<ActionResult> UpdateFromForm([FromForm] V1AddIconInputModel icon)
    {
        if (icon.Id == Guid.Empty)
        {
            return BadRequest("ID parameter cannot be an empty guid!");
        }

        var nameSize = icon.Username.Length;

        if (nameSize < 5 || nameSize > 15)
        {
            return BadRequest("Username length must be between 5 - 15 characters.");
        }

        var idSize = icon.Id.ToString().Length;

        if (idSize > 36 || idSize <= 0)
        {
            string msg = "Id cannot be bigger or smaller than 36 characters ";
            return BadRequest(msg);
        }

        if (icon.Username == null)
        {
            return BadRequest("Username parameter cannot be null!");
        }

        if (icon.Id.ToString() == null)
        {
            return BadRequest("ID parameter cannot be null!");
        }

        using var memoryStream = new MemoryStream();
        await icon.File.CopyToAsync(memoryStream);
        var byteArray = memoryStream.ToArray();

        var result = new V1UserIcon
        {
            Id = icon.Id,
            Username = icon.Username,
            DisplayPicture = byteArray
        };

        await _userIconService.Update(result);
        return Ok("User successfully updated!");
    }

    [HttpPut("[action]")]
    [Produces("application/json")]
    public async Task<ActionResult> Update(V1UserIcon icon)
    {
        if (icon.Id == Guid.Empty)
        {
            return BadRequest("ID parameter cannot be an empty guid!");
        }

        var nameSize = icon.Username.Length;

        if (nameSize < 5 || nameSize > 15)
        {
            return BadRequest("Username length must be between 5 - 15 characters.");
        }

        var idSize = icon.Id.ToString().Length;

        if (idSize > 36 || idSize <= 0)
        {
            string msg = "Id cannot be bigger or smaller than 36 characters ";
            return BadRequest(msg);
        }

        if (icon.Username == null)
        {
            return BadRequest("Username parameter cannot be null!");
        }

        if (icon.Id.ToString() == null)
        {
            return BadRequest("ID parameter cannot be null!");
        }

        var entry = await _userIconContext.UserIcons.FirstOrDefaultAsync(l => l.Id == icon.Id);

            if (entry == null)
            {
                return NotFound("An entry with the id '" + icon.Id + "' was not found.");
            } else
            {
                entry.DisplayPicture = icon.DisplayPicture;
                _userIconContext.SaveChanges();
            }

        return Ok();
    }

    /// <summary>
    /// Deletes a user icon from the database based on the provided ID.
    /// </summary>
    /// <param name="id">The ID of the user icon to be deleted.</param>
    /// <response code="200">An ActionResult indicating success or failure of the operation.</response>
    /// <response code="400">Bad request error with corresponding errormessage.</response>
    [HttpDelete("[action]")]
    [Produces("application/json")]
    public async Task<ActionResult> Delete([Required] Guid id)
    {
        if (id == Guid.Empty)
        {
            return BadRequest("ID parameter cannot be an empty guid.");
        }

        await _userIconService.Delete(id);
        return Ok($"ID: {id} - This user was successfully deleted.");
    }


    private static bool CheckForNull(V1UserIcon value)
    {
        return value == null;
    }
}

