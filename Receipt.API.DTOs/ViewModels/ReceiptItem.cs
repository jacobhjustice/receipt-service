namespace Receipt.API.DTOs.ViewModels;

public record ReceiptItem
{
    public string? ShortDescription { get; set; }
    public Decimal? Price { get; set; }
}