using Receipt.Models.Storage.Repositories;

namespace Receipt.Models.Storage.Tests.Repositories;

public class ReceiptRepositoryTests : BaseRepositoryTests<Data.Receipt>
{
    protected override BaseRepository<Data.Receipt> _repository(ReceiptContext ctx) => new ReceiptRepository(ctx);

    protected override Data.Receipt TestData(bool isDeleted) => new Data.Receipt
    {
        Retailer = "",
        DeletedAt = isDeleted ? DateTime.Now : null,
        PointsAwarded = 123,
        PurchasedAt = DateTime.Now,
        Total = (decimal)12.34,
        CreatedAt = DateTime.Now
    };
}