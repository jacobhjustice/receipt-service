using Receipt.Models.Storage.Repositories;

namespace Receipt.Models.Storage.Tests.Repositories;

public class ReceiptRepositoryTests : BaseRepositoryTests<Data.Receipt>
{
    protected override BaseRepository<Data.Receipt> _repository(ReceiptContext ctx) => new ReceiptRepository(ctx);

    protected override Data.Receipt TestData(Guid id) => new Data.Receipt
    {
        Id = id,
        Retailer = ""
    };
}