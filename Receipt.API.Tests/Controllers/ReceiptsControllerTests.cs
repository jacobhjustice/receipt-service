using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Moq;
using Receipt.API.Controllers;
using Receipt.API.DTOs.Requests;
using Receipt.API.DTOs.Responses;
using Receipt.API.Logic.Handlers;

namespace Receipt.API.Tests.Controllers;

public class ReceiptsControllerTests
{
    private Mock<IReceiptHandler> _handler;
    private ReceiptsController _controller() => new ReceiptsController(this._handler.Object);

    public ReceiptsControllerTests()
    {
        this._handler = new Mock<IReceiptHandler>();
    }
        
    [Fact]
    public void FailureProcessTests_ArgumentNullException()
    {
        this._handler
            .Setup(x => x.Process(It.IsAny<ProcessReceiptRequest>()))
            .Throws<ArgumentNullException>();

        var result = this._controller().Process(null);
        Assert.NotNull(result);
        Assert.Equal(StatusCodes.Status400BadRequest, result.StatusCode);
    }
    
    [Fact]
    public void FailureProcessTests_InvalidDataException()
    {
        this._handler
            .Setup(x => x.Process(It.IsAny<ProcessReceiptRequest>()))
            .Throws<InvalidDataException>();

        var result = this._controller().Process(null);
        Assert.NotNull(result);
        Assert.Equal(StatusCodes.Status400BadRequest, result.StatusCode);
    }
    
    [Fact]
    public void FailureProcessTests_Exception()
    {
        this._handler
            .Setup(x => x.Process(It.IsAny<ProcessReceiptRequest>()))
            .Throws<Exception>();

        var result = this._controller().Process(null);
        Assert.NotNull(result);
        Assert.Equal(StatusCodes.Status500InternalServerError, result.StatusCode);
    }
    
    [Fact]
    public void SuccessProcessTests()
    {
        var req = new ProcessReceiptRequest();
        var expectedId = Guid.NewGuid();
        this._handler
            .Setup(x => x.Process(req))
            .Returns(new Models.Data.Receipt {Id = expectedId});

        var result = (ObjectResult)this._controller().Process(req);
        Assert.NotNull(result);
        Assert.Equal(StatusCodes.Status200OK, result.StatusCode);
        Assert.IsType<IdResponse>(result.Value);
        Assert.Equal(expectedId, ((IdResponse)result.Value).Id);
    }
    
    [Fact]
    public void FailureGetTests_KeyNotFoundException()
    {
        this._handler
            .Setup(x => x.Get(It.IsAny<Guid>()))
            .Throws<KeyNotFoundException>();

        var result = this._controller().Get(Guid.NewGuid());
        Assert.NotNull(result);
        Assert.Equal(StatusCodes.Status404NotFound, result.StatusCode);
    }

    [Fact]
    public void FailureGetTests_Exception()
    {
        this._handler
            .Setup(x => x.Get(It.IsAny<Guid>()))
            .Throws<Exception>();

        var result = this._controller().Get(Guid.NewGuid());
        Assert.NotNull(result);
        Assert.Equal(StatusCodes.Status500InternalServerError, result.StatusCode);
    }
    
    [Theory]
    [InlineData(123)]
    [InlineData(0)]
    [InlineData(-1)]
    public void SuccessGetTests(int pointsAwarded)
    {
        var expectedId = Guid.NewGuid();
        this._handler
            .Setup(x => x.Get(expectedId))
            .Returns(new Models.Data.Receipt {PointsAwarded = pointsAwarded});

        var result = (ObjectResult)this._controller().Get(expectedId);
        Assert.NotNull(result);
        Assert.Equal(StatusCodes.Status200OK, result.StatusCode);
        Assert.IsType<PointsResponse>(result.Value);
        Assert.Equal(pointsAwarded, ((PointsResponse)result.Value).Points);
    }
}