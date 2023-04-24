using Microsoft.AspNetCore.Mvc;

namespace Hiof.DotNetCourse.V2023.Group14.MarketplaceService.Controllers;

[ApiController]
[Route("[controller]")]
public class V1MarketplaceController : ControllerBase
{
    private readonly ILogger<V1MarketplaceController> _logger;

    public V1MarketplaceController(ILogger<V1MarketplaceController> logger)
    {
        _logger = logger;
    }
   
}

