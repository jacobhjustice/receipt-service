namespace Receipt.Models.Storage.Repositories;

public class ReceiptRepository : BaseRepository<Data.Receipt>
{
    public ReceiptRepository(ReceiptContext ctx) : base(ctx) {}
}