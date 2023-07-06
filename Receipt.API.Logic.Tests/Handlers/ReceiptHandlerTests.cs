using FluentValidation;
using FluentValidation.Results;
using Moq;
using Receipt.API.DTOs.Requests;
using Receipt.API.DTOs.ViewModels;
using Receipt.API.Logic.Handlers;
using Receipt.Models.Storage.Repositories;
using Xunit;

namespace Receipt.API.Logic.Tests.Handlers;

public class ReceiptHandlerTests
{
    private readonly Mock<IRepository<Models.Data.Receipt>> _receiptRepository;
    private readonly Mock<IRepository<Models.Data.ReceiptItem>> _receiptItemRepository;
    private readonly Mock<IValidator<ProcessReceiptRequest>> _processReceiptRequestValidator;

    public ReceiptHandlerTests()
    {
        this._receiptRepository = new Mock<IRepository<Models.Data.Receipt>>();
        this._receiptItemRepository = new Mock<IRepository<Models.Data.ReceiptItem>>();
        this._processReceiptRequestValidator = new Mock<IValidator<ProcessReceiptRequest>>();
    }

    private ReceiptHandler _handler() => new ReceiptHandler(this._receiptRepository.Object,
        this._receiptItemRepository.Object, this._processReceiptRequestValidator.Object);
    
    [Fact]
    public void GetTestFailure_Null()
    {
        this._receiptRepository.Setup(x => x.Get(It.IsAny<Guid>(), false)).Returns<Models.Data.Receipt>(null);
        Assert.Throws<KeyNotFoundException>(() => _handler().Get(Guid.NewGuid()));
    }
    
    [Fact]
    public void GetTestSuccess()
    {
        var guid = Guid.NewGuid();
        var expectedResult = new Models.Data.Receipt();
        this._receiptRepository.Setup(x => x.Get(guid, false)).Returns(expectedResult);
        var result = _handler().Get(guid);
        Assert.NotNull(result);
        Assert.Equal(expectedResult, result);
    }
    
    [Fact]
    public void ProcessTestFailure_Null()
    {
        Assert.Throws<ArgumentNullException>(() => _handler().Process(null));
    }
    
    [Fact]
    public void ProcessTestFailure_Invalid()
    {
        var request = new ProcessReceiptRequest();
        this._processReceiptRequestValidator.Setup(x => x.Validate(request))
            .Returns(new ValidationResult(new List<ValidationFailure>() {new ValidationFailure()}));
        Assert.Throws<InvalidDataException>(() => _handler().Process(request));
    }
    
    [Theory]
    [InlineData(
        "Target",
        35.35, 
        "13:01", 
        "2022-01-01", 
        new string[]
        {
            "Mountain Dew 12PK",
            "Emils Cheese Pizza",
            "Knorr Creamy Chicken",
            "Doritos Nacho Cheese",
            "   Klarbrunn 12-PK 12 FL OZ  ",
        }, 
        new double[]
        {
            6.49,
            12.25,
            1.26,
            3.35,
            12.00
        }, 
        28
    )]
    [InlineData(
        "M&M Corner Market",
        9, 
        "14:33", 
        "2022-03-20", 
        new string[]
        {
            "Gatorade",
            "Gatorade",
            "Gatorade",
            "Gatorade",
        }, 
        new double[]
        {
            2.25,
            2.25,
            2.25,
            2.25,
        }, 
        109
    )]
    public void ProcessTestSuccess(string retailer, decimal total, string purchaseTime, string purchaseDate, string[] itemShortDescriptions, double[] itemPrices, int expectedPoints)
    {
        var receiptItems = new List<ReceiptItemViewModel>();
        for (var i = 0; i < itemPrices.Length; i++)
        {
            receiptItems.Add(new ReceiptItemViewModel{ShortDescription = itemShortDescriptions[i], Price = (decimal)itemPrices[i]});
        }

        var request = new ProcessReceiptRequest
        {
            Retailer = retailer,
            Total = total,
            PurchaseTime = purchaseTime,
            PurchaseDate = purchaseDate,
            Items = receiptItems
        };
        this._processReceiptRequestValidator.Setup(x => x.Validate(request))
            .Returns(new ValidationResult());

        var expectedGuid = Guid.NewGuid();
        this._receiptRepository.Setup(x => x.Add(It.IsAny<Models.Data.Receipt>()))
            .Callback<Models.Data.Receipt>(x => x.Id = expectedGuid);
        
        var result = _handler().Process(request);
        this._receiptRepository.Verify(x => x.Add(result), Times.Once);
        this._receiptItemRepository.Verify(x => x.Add(It.Is<IList<Models.Data.ReceiptItem>>(y => 
            y.Count == itemPrices.Length && y.All(z => z.ReceiptId == expectedGuid)
        )), Times.Once);
        Assert.NotNull(result);
        Assert.Equal(expectedGuid, result.Id);
        Assert.Equal(retailer, result.Retailer);
        Assert.Equal(total, result.Total);
        Assert.Equal(expectedPoints, result.PointsAwarded);
        Assert.Equal(TimeOnly.Parse(purchaseTime), TimeOnly.FromDateTime(result.PurchasedAt));
        Assert.Equal(DateOnly.Parse(purchaseDate), DateOnly.FromDateTime(result.PurchasedAt));
    }
}