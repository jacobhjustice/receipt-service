using Microsoft.AspNetCore.Mvc;
using Receipt.API.DTOs.Requests;

namespace Receipt.API.Controllers;

[ApiController]
[Route("receipts")]
public class ReceiptsController : Controller
{
    private readonly ILogger<ReceiptsController> _logger;

    public ReceiptsController(ILogger<ReceiptsController> logger)
    {
        _logger = logger;
    }

    [HttpPost("process")]
    public IActionResult Process([FromBody] ReceiptRequest body)
    {
        throw new NotImplementedException();
    }
    
    [HttpGet("{id}/points")]
    public IActionResult Process([FromRoute] int id)
    {
        throw new NotImplementedException();
    }
}