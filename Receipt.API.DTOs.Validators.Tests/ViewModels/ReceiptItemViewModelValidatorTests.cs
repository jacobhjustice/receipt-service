using FluentValidation;
using Receipt.API.DTOs.Validators.ViewModels;
using Receipt.API.DTOs.ViewModels;
using Xunit;

namespace Receipt.API.DTOs.Validators.Tests.ViewModels;

public class ReceiptItemViewModelValidatorTests
{
    IValidator<ReceiptItemViewModel> _validator() => new ReceiptItemViewModelValidator();

    [Fact]
    public void NullTest()
    {
        Assert.Throws<ArgumentNullException>(() => this._validator().Validate(null));
    }

    [Theory]
    [InlineData("test", 1)]
    [InlineData("1", 1.01)]
    [InlineData("z", 423412.412341234)]
    public void SuccessfulValidationTests(string? description, Decimal price)
    {
        var result = this._validator().Validate(new ReceiptItemViewModel
        {
            ShortDescription = description,
            Price = price
        });
        
        Assert.NotNull(result);
        Assert.True(result.IsValid);
    }
}