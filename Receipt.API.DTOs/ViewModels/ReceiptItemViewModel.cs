namespace Receipt.API.DTOs.ViewModels;

public record ReceiptItemViewModel
{
    public string? ShortDescription { get; set; }
    public Decimal? Price { get; set; }
}