namespace Receipt.Models.Data;

public class ReceiptItem : IDataModel
{
    public Guid Id { get; set; }
    public Guid ReceiptId { get; set; }
    public string ShortDescription { get; set; }
    public Decimal Price { get; set; }
}