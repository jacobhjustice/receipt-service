using FluentValidation;
using Receipt.API.DTOs.ViewModels;

namespace Receipt.API.DTOs.Validators.ViewModels;

public class ReceiptItemViewModelValidator : AbstractValidator<ReceiptItemViewModel>
{
    public ReceiptItemViewModelValidator()
    {
        RuleFor(x => x.ShortDescription)
            .NotEmpty();

        RuleFor(x => x.Price)
            .NotNull()
            .GreaterThan(0);
    }
}