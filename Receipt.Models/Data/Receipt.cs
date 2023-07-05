namespace Receipt.Models.Data;

public class Receipt : IDataModel
{
    public Guid Id { get; set; }
    public int PointsAwarded { get; set; }
    public string Retailer { get; set; }
    public DateTime PurchasedAt { get; set; }
    public Decimal Total { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
    public DateTime? DeletedAt { get; set; }
}