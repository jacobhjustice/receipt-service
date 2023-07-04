using Receipt.API.DTOs.ViewModels;

namespace Receipt.API.DTOs.Requests;

public record ReceiptRequest
{
    public string? Retailer { get; set; }
    public string? PurchaseDate { get; set; }
    public string? PurchaseTime { get; set; }
    public Decimal? Total { get; set; }
    public IList<ReceiptItem> Items { get; set; }
}