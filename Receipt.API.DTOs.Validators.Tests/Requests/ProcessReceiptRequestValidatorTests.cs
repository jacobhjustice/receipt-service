using FluentValidation;
using FluentValidation.Results;
using Moq;
using Receipt.API.DTOs.Requests;
using Receipt.API.DTOs.Validators.Requests;
using Receipt.API.DTOs.ViewModels;
using Xunit;

namespace Receipt.API.DTOs.Validators.Tests.Requests;

public class ProcessReceiptRequestValidatorTests
{
    private readonly Mock<IValidator<ReceiptItemViewModel>> _receiptItemValidator;
    IValidator<ProcessReceiptRequest> _validator() => new ProcessReceiptRequestValidator(this._receiptItemValidator.Object);

    public ProcessReceiptRequestValidatorTests()
    {
        this._receiptItemValidator = new Mock<IValidator<ReceiptItemViewModel>>();
    }
    
    [Fact]
    public void NullTest()
    {
        Assert.Throws<ArgumentNullException>(() => this._validator().Validate(null));
    }
    
    [Theory]
    [InlineData("1", "2023-07-04", "12:34", 1.0, new double []{1})]
    [InlineData("adfazr", "1997-07-12", "23:59", .75, new double []{.25, .4, .1})]
    [InlineData("$!@#$!@#?", "2024-02-29", "00:14", 100, new double []{10, 2, 50, 5, 5, 7, 1, 20})]
    [InlineData("z", "2012-01-01", "2:19 AM", 2, new double []{1, 1})]
    public void SuccessfulValidationTests(string? retailer, string? purchaseDate, string purchaseTime, Decimal total, double[] itemTotals)
    {
        this._receiptItemValidator.Setup(x => x.Validate(It.IsAny<ReceiptItemViewModel>()))
            .Returns(new ValidationResult());

        var result = this._validator().Validate(new ProcessReceiptRequest()
        {
            Retailer = retailer,
            PurchaseDate = purchaseDate,
            PurchaseTime = purchaseTime,
            Total = total,
            Items = itemTotals.Select(x => new ReceiptItemViewModel()
            {
                Price = (decimal)x
            }).ToList(),
        });
        
        Assert.NotNull(result);
        Assert.True(result.IsValid);
    }
    
    [Fact]
    public void FailureValidationTests_Items_Null()
    {
        this._receiptItemValidator.Setup(x => x.Validate(It.IsAny<ReceiptItemViewModel>()))
            .Returns(new ValidationResult());

        var result = this._validator().Validate(new ProcessReceiptRequest()
        {
            Retailer = "retailer",
            PurchaseDate = "2000-01-01",
            PurchaseTime = "12:34",
            Total = 1,
            Items = null
        });
        
        Assert.NotNull(result);
        Assert.False(result.IsValid);
        Assert.Contains(result.Errors, x => x.PropertyName == nameof(ProcessReceiptRequest.Items));
    }
    
    [Fact]
    public void FailureValidationTests_Items_NullChild()
    {
        this._receiptItemValidator.Setup(x => x.Validate(It.IsAny<ReceiptItemViewModel>()))
            .Returns(new ValidationResult());

        var result = this._validator().Validate(new ProcessReceiptRequest()
        {
            Retailer = "retailer",
            PurchaseDate = "2000-01-01",
            PurchaseTime = "12:34",
            Total = 1,
            Items = new List<ReceiptItemViewModel>{null}
        });
        
        Assert.NotNull(result);
        Assert.False(result.IsValid);
        Assert.Contains(result.Errors, x => x.PropertyName == $"{nameof(ProcessReceiptRequest.Items)}[0]");
    }
    
    [Fact]
    public void FailureValidationTests_Items_FailedChild()
    {
        this._receiptItemValidator.Setup(x => x.Validate(It.IsAny<ReceiptItemViewModel>()))
            .Returns(new ValidationResult(new List<ValidationFailure>{new ValidationFailure()}));

        var result = this._validator().Validate(new ProcessReceiptRequest()
        {
            Retailer = "retailer",
            PurchaseDate = "2000-01-01",
            PurchaseTime = "12:34",
            Total = 1,
            Items = new List<ReceiptItemViewModel>{new ReceiptItemViewModel()}
        });
        
        Assert.NotNull(result);
        Assert.False(result.IsValid);
        Assert.Contains(result.Errors, x => x.PropertyName == $"{nameof(ProcessReceiptRequest.Items)}[0]");
    }
    
    [Theory]
    [InlineData(null, new double []{1,.5})]
    [InlineData(1.51, new double []{1,.5})]
    [InlineData(1.49, new double []{1,.5})]
    [InlineData(1.99, new double []{1,1})]
    [InlineData(2.01, new double []{1,1})]
    [InlineData(-2, new double []{-1,-1})]
    public void FailureValidationTests_Total(Decimal total, double[] itemPrices)
    {
        this._receiptItemValidator.Setup(x => x.Validate(It.IsAny<ReceiptItemViewModel>()))
            .Returns(new ValidationResult());

        var result = this._validator().Validate(new ProcessReceiptRequest()
        {
            Retailer = "retailer",
            PurchaseDate = "2000-01-01",
            PurchaseTime = "12:34",
            Total = total,
            Items = itemPrices.Select(x => 
                new ReceiptItemViewModel
                {
                    Price = (decimal?) x
                }).ToList()
        });
        
        Assert.NotNull(result);
        Assert.False(result.IsValid);
        Assert.Contains(result.Errors, x => x.PropertyName == nameof(ProcessReceiptRequest.Total));
    }
    
    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("     ")]
    public void FailureValidationTests_Retailer(string? retailer)
    {
        this._receiptItemValidator.Setup(x => x.Validate(It.IsAny<ReceiptItemViewModel>()))
            .Returns(new ValidationResult());

        var result = this._validator().Validate(new ProcessReceiptRequest()
        {
            Retailer = retailer,
            PurchaseDate = "2000-01-01",
            PurchaseTime = "12:34",
            Total = 1,
            Items = new List<ReceiptItemViewModel>{new ReceiptItemViewModel{Price = 1}}
        });
        
        Assert.NotNull(result);
        Assert.False(result.IsValid);
        Assert.Contains(result.Errors, x => x.PropertyName == nameof(ProcessReceiptRequest.Retailer));
    }

    [Theory]
    [InlineData(null)]
    [InlineData("asfsdfs")]
    [InlineData("2000")]
    [InlineData("2000-01-01-01")]
    [InlineData("2000-01-32")]
    [InlineData("-2000-01-01")]
    [InlineData("2000-13-01")]
    [InlineData("2000-0-01")]
    public void FailureValidationTests_PurchaseDate(string? purchaseDate)
    {
        this._receiptItemValidator.Setup(x => x.Validate(It.IsAny<ReceiptItemViewModel>()))
            .Returns(new ValidationResult());

        var result = this._validator().Validate(new ProcessReceiptRequest()
        {
            Retailer = "retailer",
            PurchaseDate = purchaseDate,
            PurchaseTime = "12:34",
            Total = 1,
            Items = new List<ReceiptItemViewModel>{new ReceiptItemViewModel{Price = 1}}
        });
        
        Assert.NotNull(result);
        Assert.False(result.IsValid);
        Assert.Contains(result.Errors, x => x.PropertyName == nameof(ProcessReceiptRequest.PurchaseDate));
    }
    
    [Theory]
    [InlineData(null)]
    [InlineData("1")]
    [InlineData("abcd")]
    [InlineData("27:12")]
    [InlineData("-1:51")]
    [InlineData("12:111")]
    public void FailureValidationTests_PurchaseTime(string? purchaseTime)
    {
        this._receiptItemValidator.Setup(x => x.Validate(It.IsAny<ReceiptItemViewModel>()))
            .Returns(new ValidationResult());

        var result = this._validator().Validate(new ProcessReceiptRequest()
        {
            Retailer = "retailer",
            PurchaseDate = "2000-01-01",
            PurchaseTime = purchaseTime,
            Total = 1,
            Items = new List<ReceiptItemViewModel>{new ReceiptItemViewModel{Price = 1}}
        });
        
        Assert.NotNull(result);
        Assert.False(result.IsValid);
        Assert.Contains(result.Errors, x => x.PropertyName == nameof(ProcessReceiptRequest.PurchaseTime));
    }
}