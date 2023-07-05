using Receipt.Models.Storage.Repositories;

namespace Receipt.Models.Storage.Tests.Repositories;

public class ReceiptItemRepositoryTests : BaseRepositoryTests<Data.ReceiptItem>
{
    protected override BaseRepository<Data.ReceiptItem> _repository(ReceiptContext ctx) => new ReceiptItemRepository(ctx);

    protected override Data.ReceiptItem TestData(bool isDeleted) => new Data.ReceiptItem
    {
        ShortDescription = "Test",
        DeletedAt = isDeleted ? DateTime.Now : null,
        CreatedAt = DateTime.Now
    };
}