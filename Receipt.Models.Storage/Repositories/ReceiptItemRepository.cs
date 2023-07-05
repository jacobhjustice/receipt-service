using Microsoft.EntityFrameworkCore;

namespace Receipt.Models.Storage.Repositories;

public class ReceiptItemRepository : BaseRepository<Data.ReceiptItem>
{
    public ReceiptItemRepository(ReceiptContext ctx) : base(ctx) {}
}