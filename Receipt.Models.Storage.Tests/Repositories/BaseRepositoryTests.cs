using Microsoft.EntityFrameworkCore;
using Receipt.Models.Data;
using Receipt.Models.Storage.Repositories;
using Xunit;

namespace Receipt.Models.Storage.Tests.Repositories;

public abstract class BaseRepositoryTests<TModel> where TModel : class, IDataModel, new()
{
    protected abstract BaseRepository<TModel> _repository(ReceiptContext ctx);
    protected abstract TModel TestData(Guid id);
    private ReceiptContext SetupContext()
    {
        return new ReceiptContext();
    }
    
    protected void AddData(ReceiptContext ctx, TModel dataToAdd)
    {
        ctx.Set<TModel>().Add(dataToAdd);
        ctx.SaveChanges();
    }
    
    [Fact]
    public void TestGet()
    {
        var ctx = this.SetupContext();
        this.AddData(ctx, this.TestData(Guid.Parse("7cb3db22-fa01-4b07-9a9d-9932a6e33130")));
        this.AddData(ctx, this.TestData(Guid.Parse("4ee1bdbb-2686-492d-97d4-c99c226bb689")));
        var repository = this._repository(ctx);

        var guid1 = Guid.Parse("7cb3db22-fa01-4b07-9a9d-9932a6e33130");
        var result1 = repository.Get(guid1);
        Assert.NotNull(result1);
        Assert.Equal(guid1, result1.Id);
        
        var guid2 = Guid.Parse("4ee1bdbb-2686-492d-97d4-c99c226bb689");
        var result2 = repository.Get(guid2);
        Assert.NotNull(result1);
        Assert.Equal(guid2, result2.Id);
        
        var guid3 = Guid.Parse("b4304ade-b861-4048-bb58-e1e216a5e10c");
        var result3 = repository.Get(guid3);
        Assert.Null(result3);
    }
    
    [Fact]
    public void TestAdd()
    {
        var ctx = this.SetupContext();
        var repository = this._repository(ctx);
        this._repository(ctx).Add(this.TestData(Guid.NewGuid()));
        Assert.Equal(1, ctx.Set<TModel>().Count());
    }
}