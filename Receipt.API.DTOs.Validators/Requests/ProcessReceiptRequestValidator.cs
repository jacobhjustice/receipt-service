using FluentValidation;
using Receipt.API.DTOs.Requests;
using Receipt.API.DTOs.ViewModels;

namespace Receipt.API.DTOs.Validators.Requests;

public class ProcessReceiptRequestValidator : AbstractValidator<ProcessReceiptRequest>
{
    public ProcessReceiptRequestValidator(IValidator<ReceiptItemViewModel> receiptItemValidator)
    {
        RuleFor(x => x.Items)
            .NotEmpty()
            .DependentRules(() =>
            {
                RuleForEach(x => x.Items)
                    .NotNull()
                    .Must(x => receiptItemValidator.Validate(x).IsValid);
            });
        
        
        RuleFor(x => x.Total)
            .NotNull()
            .GreaterThan(0)
            .Must((receipt, total) =>
            {
                return (total ?? -1) == (receipt.Items?.Sum(x => x?.Price ?? 0) ?? 0);
            });

        RuleFor(x => x.Retailer)
            .NotEmpty();

        RuleFor(x => x.PurchaseDate)
            .NotEmpty()
            .Must(dateString =>
            {
                DateOnly date;
                return DateOnly.TryParse(dateString, out date);
            });
        
        RuleFor(x => x.PurchaseTime)
            .NotEmpty()
            .Must(timeString =>
            {
                TimeOnly time;
                return TimeOnly.TryParse(timeString, out time);
            });
    }
}