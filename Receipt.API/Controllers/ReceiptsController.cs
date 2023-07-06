using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Receipt.API.DTOs.Requests;
using Receipt.API.DTOs.Responses;
using Receipt.API.Logic.Handlers;

namespace Receipt.API.Controllers;

[ApiController]
[Route("receipts")]
public class ReceiptsController : Controller
{
    private readonly IReceiptHandler _receiptHandler;
    public ReceiptsController(IReceiptHandler receiptHandler)
    {
        this._receiptHandler = receiptHandler;
    }

    [HttpPost("process")]
    public IStatusCodeActionResult Process([FromBody] ProcessReceiptRequest body)
    {
        try
        {
            var receipt = this._receiptHandler.Process(body);
            return StatusCode(StatusCodes.Status200OK, new IdResponse {Id = receipt.Id});
        }
        catch (Exception ex) when (ex is ArgumentNullException || ex is InvalidDataException)
        {
            return StatusCode(StatusCodes.Status400BadRequest);
        }
        catch (Exception ex)
        {
            return StatusCode(StatusCodes.Status500InternalServerError);
        }
    }
    
    [HttpGet("{id}/points")]
    public IStatusCodeActionResult Get([FromRoute] Guid id)
    {
        try
        {
            var receipt = this._receiptHandler.Get(id);
            return StatusCode(StatusCodes.Status200OK, new PointsResponse {Points = receipt.PointsAwarded});
        }
        catch (Exception ex) when (ex is KeyNotFoundException)
        {
            return StatusCode(StatusCodes.Status404NotFound);
        }
        catch (Exception ex)
        {
            return StatusCode(StatusCodes.Status500InternalServerError);
        }
    }
}